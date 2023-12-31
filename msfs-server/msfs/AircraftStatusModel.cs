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

    public class AircraftStatusFastModel(IHubContext<MyHub> myHub, Mqtt mqtt) : AircraftModel
    {
        public SimConnectStructs.AircraftStatusFastStruct Data { get; private set; }

        private SimConnectStructs.AircraftStatusFastStruct? _lastSentData;
        
        public void SetData(SimConnectStructs.AircraftStatusFastStruct data)
        {
            data.GeneralEngineOilTemperature = ((5.0 / 9.0) * data.GeneralEngineOilTemperature) - 273.15; // convert to celcius

            data.GeneralEngineOilPressure /= 144.0; // convert to psi

            Data = (SimConnectStructs.AircraftStatusFastStruct)TruncateDoubles(data);

            var refresh = _lastSentData == null;
            _lastSentData ??= new SimConnectStructs.AircraftStatusFastStruct();

            refresh = refresh || _lastSentData != Data;
            
            if (!refresh) return;

            _lastSentData = Data;

            myHub.Clients.All.SendAsync("MsFsFastRefresh");

            mqtt.Publish(Data, "fast");
        }
    }

    public class AircraftStatusSlowModel(IHubContext<MyHub> myHub, Mqtt mqtt) : AircraftModel
    {
        public SimConnectStructs.AircraftStatusSlowStruct Data { get; private set; }

        private SimConnectStructs.AircraftStatusSlowStruct? _lastSentData;
        
        public void SetData(SimConnectStructs.AircraftStatusSlowStruct data)
        {

            Data = (SimConnectStructs.AircraftStatusSlowStruct)TruncateDoubles(data);


            var refresh = _lastSentData == null;
            _lastSentData ??= new SimConnectStructs.AircraftStatusSlowStruct();

            refresh = refresh || _lastSentData != Data;

            if (Data.Latitude == 0 || Data.Longitude == 0 || !refresh) return;

            _lastSentData = Data;

            /* null island ???
              0.00040748246254171134 0.01397450300629543
              0.000407520306147189   0.01397450300629543
              */

            myHub.Clients.All.SendAsync("MsFsSlowRefresh");

            mqtt.Publish(Data, "slow");
        }
    }
}
