using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Controls.Primitives;
using Microsoft.AspNetCore.SignalR;
using Microsoft.FlightSimulator.SimConnect;
using msfs_server.Hubs;
using msfs_server.MQTT;

namespace msfs_server.msfs
{
    public class AircraftModel
    {
        protected static object TruncateDoubles(
            object data)
        {

            var fields = data.GetType()
                .GetFields()
                .Where(x => x.FieldType.Name == "Double");

            foreach (var field in fields)
            {
                var x = field.GetValue(data);

                // truncate to 5 decimals
                field.SetValue(data, Math.Truncate((double)x! * 100000d) / 100000d);
            }

            return data;
        }
    }

    public class GarminG5HsiModel : AircraftModel
    {
        public SimConnectStructs.GarminG5HsiStruct Data { get; private set; }

        private static SimConnectStructs.GarminG5HsiStruct? _lastSentData;

        private Thread _thread;

        private static IHubContext<MyHub> _myHub;

        private static Mqtt _mqtt;

        public GarminG5HsiModel(IHubContext<MyHub> myHub, Mqtt mqtt)
        {
            _myHub = myHub;
            _mqtt = mqtt;
        }

        private static void MqttThread(object threadData)
        {
            var argArray = (Array)threadData;
            var data = (SimConnectStructs.GarminG5HsiStruct)argArray.GetValue(0)!;
            var lastData = (SimConnectStructs.GarminG5HsiStruct)argArray.GetValue(1)!;

            var force = (bool)argArray.GetValue(2)!;

            _myHub.Clients.All.SendAsync("MsFsGarminG5HSIRefresh").GetAwaiter().GetResult();

            // make sure MQTT decimal point is a '.'
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            _mqtt.Publish(data, lastData, force, "garming5hsi");
        }

        public void SetData(SimConnectStructs.GarminG5HsiStruct data)
        {
            Data = (SimConnectStructs.GarminG5HsiStruct)TruncateDoubles(data);

            if (_thread?.IsAlive != true)
            {
                var force = _lastSentData == null;

                _lastSentData ??= new SimConnectStructs.GarminG5HsiStruct();

                if (force || _lastSentData != Data)
                {
                    _thread = new Thread(MqttThread);
                    _thread.Start(new object[3] { Data, _lastSentData, force });

                    _lastSentData = Data;
                }
            }
        }
    }

    public class GarminG5ApbarModel : AircraftModel
    {
        public SimConnectStructs.GarminG5ApbarStruct Data { get; private set; }

        private static SimConnectStructs.GarminG5ApbarStruct? _lastSentData;

        private Thread _thread;

        private static IHubContext<MyHub> _myHub;

        private static Mqtt _mqtt;

        public GarminG5ApbarModel(IHubContext<MyHub> myHub, Mqtt mqtt)
        {
            _myHub = myHub;
            _mqtt = mqtt;
        }

        private static void MqttThread(object threadData)
        {
            var argArray = (Array)threadData;
            var data = (SimConnectStructs.GarminG5ApbarStruct)argArray.GetValue(0)!;
            var lastData = (SimConnectStructs.GarminG5ApbarStruct)argArray.GetValue(1)!;

            var force = (bool)argArray.GetValue(2)!;

            _myHub.Clients.All.SendAsync("MsFsGarminG5APBARRefresh").GetAwaiter().GetResult();

            // make sure MQTT decimal point is a '.'
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            _mqtt.Publish(data, lastData, force, "garming5apbar");
            
        }

        public void SetData(SimConnectStructs.GarminG5ApbarStruct data)
        {
            Data = (SimConnectStructs.GarminG5ApbarStruct)TruncateDoubles(data);

            if (_thread?.IsAlive != true)
            {
                var force = _lastSentData == null;

                _lastSentData ??= new SimConnectStructs.GarminG5ApbarStruct();

                if (force || _lastSentData != Data)
                {
                    _thread = new Thread(MqttThread);
                    _thread.Start(new object[3] { Data, _lastSentData, force });

                    _lastSentData = Data;
                }
            }
        }
    }


    public class GarminG5Model : AircraftModel
    {
        public SimConnectStructs.GarminG5Struct Data { get; private set; }

        private static SimConnectStructs.GarminG5Struct? _lastSentData;

        private Thread _thread;

        private static IHubContext<MyHub> _myHub;

        private static Mqtt _mqtt;

        public GarminG5Model(IHubContext<MyHub> myHub, Mqtt mqtt)
        {
            _myHub = myHub;
            _mqtt = mqtt;
        }

        private static void MqttThread(object threadData)
        {
            var argArray = (Array)threadData;
            var data = (SimConnectStructs.GarminG5Struct)argArray.GetValue(0)!;
            var lastData = (SimConnectStructs.GarminG5Struct)argArray.GetValue(1)!;

            var force = (bool)argArray.GetValue(2)!;

            _myHub.Clients.All.SendAsync("MsFsGarminG5Refresh").GetAwaiter().GetResult();

            // make sure MQTT decimal point is a '.'
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            _mqtt.Publish(data, lastData, force, "garming5");
        }

        public void SetData(SimConnectStructs.GarminG5Struct data)
        {
            //data.GeneralEngineOilTemperature = ((5.0 / 9.0) * data.GeneralEngineOilTemperature) - 273.15; // convert to celcius

            //data.GeneralEngineOilPressure /= 144.0; // convert to psi

            Data = (SimConnectStructs.GarminG5Struct)TruncateDoubles(data);

            if (_thread?.IsAlive != true)
            {
                var force = _lastSentData == null;

                _lastSentData ??= new SimConnectStructs.GarminG5Struct();

                if (force || _lastSentData != Data)
                {
                    _thread = new Thread(MqttThread);
                    _thread.Start(new object[3] { Data, _lastSentData, force });

                    _lastSentData = Data;

                }
            }
        }
    }

    public class MovingMapModel : AircraftModel
    {
        public SimConnectStructs.MovingMapStruct Data { get; private set; }

        private static SimConnectStructs.MovingMapStruct? _lastSentData;

        private Thread _thread;

        private static IHubContext<MyHub> _myHub;

        private static Mqtt _mqtt;

        public MovingMapModel(IHubContext<MyHub> myHub, Mqtt mqtt)
        {
            _myHub = myHub;
            _mqtt = mqtt;
        }

        private static void MqttThread(object threadData)
        {
            var argArray = (Array)threadData;
            var data = (SimConnectStructs.MovingMapStruct)argArray.GetValue(0)!;
            var lastData = (SimConnectStructs.MovingMapStruct)argArray.GetValue(1)!;

            var force = (bool)argArray.GetValue(2)!;

            _myHub.Clients.All.SendAsync("MsFsMovingMapRefresh").GetAwaiter().GetResult();

            // make sure MQTT decimal point is a '.'
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            _mqtt.Publish(data, lastData, force, "movingmap");
        }

        public void SetData(SimConnectStructs.MovingMapStruct data)
        {

            Data = (SimConnectStructs.MovingMapStruct)TruncateDoubles(data);

            if (_thread?.IsAlive != true)
            {
                var force = _lastSentData == null;

                _lastSentData ??= new SimConnectStructs.MovingMapStruct();

                if (force || _lastSentData != Data)
                {
                    _thread = new Thread(MqttThread);
                    _thread.Start(new object[3] { Data, _lastSentData, force });

                    _lastSentData = Data;

                }
            }

            /* null island ???
              0.00040748246254171134 0.01397450300629543
              0.000407520306147189   0.01397450300629543
              */
            
        }
    }
}
