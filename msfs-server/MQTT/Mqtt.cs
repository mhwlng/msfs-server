using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using msfs_server.msfs;
using Newtonsoft.Json;
using Serilog;

namespace msfs_server.MQTT
{
    public class Mqtt
    {
        private readonly object mqttLock = new object();

        private static readonly MqttFactory Factory = new();
        private static readonly IManagedMqttClient MqttClient = Factory.CreateManagedMqttClient();
        private static readonly string ClientId = Guid.NewGuid().ToString();
        
        private static void MqttOnConnectingFailed(ConnectingFailedEventArgs e)
        {
            Log.Error($"MQTT Client: Connection Failed", e.Exception);
        }

        private static void MqttOnConnected(MqttClientConnectedEventArgs e)
        {

            Log.Information($"MQTT Client: Connected with result: {e.ConnectResult?.ResultCode}");
        }

        private static void MqttOnDisconnected(MqttClientDisconnectedEventArgs e)
        {

            Log.Error($"MQTT Client: Connection lost with reason: {e.Reason}.");
        }


        public Mqtt()
        {
            var mqttUri = Common.ConfigurationRoot.GetValue<string>("MQTT:mqttURI");
            var mqttUser = Common.ConfigurationRoot.GetValue<string>("MQTT:mqttUser");
            var mqttPassword = Common.ConfigurationRoot.GetValue<string>("MQTT:mqttPassword");
            var mqttPort = Common.ConfigurationRoot.GetValue<int>("MQTT:mqttPort");
            var mqttSecure = Common.ConfigurationRoot.GetValue<bool>("MQTT:mqttSecure");


            var messageBuilder = new MqttClientOptionsBuilder()
                //.WithProtocolVersion(MqttProtocolVersion.V500)
                .WithClientId(ClientId)
                .WithCredentials(mqttUser, mqttPassword)
                .WithTcpServer(mqttUri, mqttPort)

                .WithCleanSession();

            var options = mqttSecure
                ? messageBuilder
                    .WithTlsOptions(o =>
                    {

                    })
                    .Build()
                : messageBuilder
                    .Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(30))
                .WithClientOptions(options)
                .Build();

            try
            {
                MqttClient.ConnectedAsync += e =>
                {
                    MqttOnConnected(e);
                    return Task.CompletedTask;
                };
                MqttClient.DisconnectedAsync += e =>
                {
                    MqttOnDisconnected(e);
                    return Task.CompletedTask;
                };
                MqttClient.ConnectingFailedAsync += e =>
                {
                    MqttOnConnectingFailed(e);
                    return Task.CompletedTask;
                };


                MqttClient.StartAsync(managedOptions).GetAwaiter().GetResult();


            }
            catch (Exception ex)
            {
                Log.Error($"MQTT CONNECT FAILED", ex);
            }

        }

        public void Publish(object obj, string topic)
        {
            try
            {
                //Log.Information("Publish MQTT");

                var fields = obj.GetType()
                    .GetFields()
                    .Select(field => new { name = field.Name, value = field.GetValue(obj) })
                    .ToList();

                foreach (var fieldValue in fields)
                {
                    lock (mqttLock)
                    {
                        var message = new MqttApplicationMessageBuilder()
                            .WithTopic($"msfs/{topic}/{fieldValue.name}")
                            .WithPayload(fieldValue.value.ToString())
                            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                            .Build();
                        MqttClient.EnqueueAsync(message).GetAwaiter().GetResult();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error($"MQTT Client : Enqueue Failed", ex);
            }


        }

    }
}
