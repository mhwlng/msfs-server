using System;
using System.Linq;
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

    public class GarminG5HsiModel(IHubContext<MyHub> myHub, Mqtt mqtt) : AircraftModel
    {
        public SimConnectStructs.GarminG5HsiStruct Data { get; private set; }

        private SimConnectStructs.GarminG5HsiStruct? _lastSentData;

        public void SetData(SimConnectStructs.GarminG5HsiStruct data)
        {
            Data = (SimConnectStructs.GarminG5HsiStruct)TruncateDoubles(data);

            var force = _lastSentData == null;

            _lastSentData ??= new SimConnectStructs.GarminG5HsiStruct();

            if (!force && _lastSentData == Data) return;

            myHub.Clients.All.SendAsync("MsFsGarminG5HSIRefresh");

            mqtt.Publish(Data, _lastSentData, force, "garming5hsi");

            _lastSentData = Data;

        }
    }

    public class GarminG5ApbarModel(IHubContext<MyHub> myHub, Mqtt mqtt) : AircraftModel
    {
        public SimConnectStructs.GarminG5ApbarStruct Data { get; private set; }

        private SimConnectStructs.GarminG5ApbarStruct? _lastSentData;

        public void SetData(SimConnectStructs.GarminG5ApbarStruct data)
        {
            Data = (SimConnectStructs.GarminG5ApbarStruct)TruncateDoubles(data);

            var force = _lastSentData == null;

            _lastSentData ??= new SimConnectStructs.GarminG5ApbarStruct();

            if (!force && _lastSentData == Data) return;

            myHub.Clients.All.SendAsync("MsFsGarminG5APBARRefresh");

            mqtt.Publish(Data, _lastSentData,force, "garming5apbar");

            _lastSentData = Data;

        }
    }


    public class GarminG5Model(IHubContext<MyHub> myHub, Mqtt mqtt) : AircraftModel
    {
        public SimConnectStructs.GarminG5Struct Data { get; private set; }

        private SimConnectStructs.GarminG5Struct? _lastSentData;
        
        public void SetData(SimConnectStructs.GarminG5Struct data)
        {
            //data.GeneralEngineOilTemperature = ((5.0 / 9.0) * data.GeneralEngineOilTemperature) - 273.15; // convert to celcius

            //data.GeneralEngineOilPressure /= 144.0; // convert to psi

            Data = (SimConnectStructs.GarminG5Struct)TruncateDoubles(data);

            var force = _lastSentData == null;

            _lastSentData ??= new SimConnectStructs.GarminG5Struct();

            if (!force && _lastSentData == Data) return;

            myHub.Clients.All.SendAsync("MsFsGarminG5Refresh");

            mqtt.Publish(Data, _lastSentData,force, "garming5");

            _lastSentData = Data;

        }
    }

    public class MovingMapModel(IHubContext<MyHub> myHub, Mqtt mqtt) : AircraftModel
    {
        public SimConnectStructs.MovingMapStruct Data { get; private set; }

        private SimConnectStructs.MovingMapStruct? _lastSentData;
        
        public void SetData(SimConnectStructs.MovingMapStruct data)
        {

            Data = (SimConnectStructs.MovingMapStruct)TruncateDoubles(data);

            var force = _lastSentData == null;

            _lastSentData ??= new SimConnectStructs.MovingMapStruct();

            if (!force && _lastSentData == Data) return;

            /* null island ???
              0.00040748246254171134 0.01397450300629543
              0.000407520306147189   0.01397450300629543
              */

            myHub.Clients.All.SendAsync("MsFsMovingMapRefresh");

            mqtt.Publish(Data, _lastSentData,force, "movingmap");

            _lastSentData = Data;

        }
    }
}
