using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using msfs_server.msfs;
using Newtonsoft.Json;
using Serilog;

namespace msfs_server.MQTT
{
    public class Mqtt : IDisposable
    {
        private static readonly MqttFactory Factory = new();
        private static readonly string ClientId = Guid.NewGuid().ToString();
        private static MqttClientOptions _options;
        private static MqttClient _mqttClient;
        private readonly object _thisLock = new();
        public Mqtt()
        {
            var mqttUri = Common.ConfigurationRoot.GetValue<string>("MQTT:mqttURI");
            var mqttUser = Common.ConfigurationRoot.GetValue<string>("MQTT:mqttUser");
            var mqttPassword = Common.ConfigurationRoot.GetValue<string>("MQTT:mqttPassword");
            var mqttPort = Common.ConfigurationRoot.GetValue<int>("MQTT:mqttPort");
            //var mqttSecure = Common.ConfigurationRoot.GetValue<bool>("MQTT:mqttSecure");

            _options = new MqttClientOptionsBuilder()
                //.WithProtocolVersion(MqttProtocolVersion.V500)
                //.WithTlsOptions(o => { })
                .WithClientId(ClientId)
                .WithCredentials(mqttUser, mqttPassword)
                .WithTcpServer(mqttUri, mqttPort)
                .WithCleanSession()
                .Build();

            
            _mqttClient = (MqttClient)Factory.CreateMqttClient();

            Connect();

        }
        public void Connect()
        {
            _mqttClient.ConnectAsync(_options, CancellationToken.None).GetAwaiter().GetResult();

        }

        public void Disconnect()
        {
            _mqttClient.DisconnectAsync().GetAwaiter().GetResult();

        }
        public void Publish(object obj, object oldobj, bool force, string topic)
        {
            try
            {
                //Log.Information("Publish MQTT");

                var fields = obj.GetType()
                    .GetFields()
                    .Select(field => new { name = field.Name, value = field.GetValue(obj) })
                    .ToList();

                var oldfields = oldobj.GetType()
                    .GetFields()
                    .Select(oldfield => new { name = oldfield.Name, value = oldfield.GetValue(oldobj) })
                    .ToList();

                lock (_thisLock)
                {
                    for (var index = 0; index < fields.Count; index++)
                    {
                        var fieldValue = fields[index];

                        var oldfieldValue = oldfields[index];

                        if (force || fieldValue.value.ToString() != oldfieldValue.value.ToString())
                        {
                            var message = new MqttApplicationMessageBuilder()
                                .WithTopic($"msfs/{topic}/{fieldValue.name}")
                                .WithPayload(fieldValue.value.ToString())
                                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                                .Build();
                                
                            _mqttClient.PublishAsync(message, CancellationToken.None).GetAwaiter().GetResult();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"MQTT Client : Failed", ex);
            }

        }
        public void Dispose()
        {
            Disconnect();
        }
    }
}
