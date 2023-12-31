using System;
using System.Windows.Controls.Primitives;
using Microsoft.AspNetCore.SignalR;
using Microsoft.FlightSimulator.SimConnect;
using msfs_server.Hubs;
using msfs_server.MQTT;

namespace msfs_server.msfs
{

    public class AircraftStatusFastModel(IHubContext<MyHub> myHub, Mqtt mqtt)
    {
        public SimConnectStructs.AircraftStatusFastStruct Data { get; private set; }

        private SimConnectStructs.AircraftStatusFastStruct? _lastSentData;

        public void SetData(SimConnectStructs.AircraftStatusFastStruct data)
        {
            Data = data;

            var refresh = _lastSentData == null;
            _lastSentData ??= new SimConnectStructs.AircraftStatusFastStruct();

            refresh = refresh || _lastSentData != data;
            
            if (!refresh) return;

            _lastSentData = data;

            myHub.Clients.All.SendAsync("MsFsFastRefresh");

            mqtt.Publish(data, "fast");
        }
    }

    public class AircraftStatusSlowModel(IHubContext<MyHub> myHub, Mqtt mqtt)
    {
        public SimConnectStructs.AircraftStatusSlowStruct Data { get; private set; }

        private SimConnectStructs.AircraftStatusSlowStruct? _lastSentData;


        public void SetData(SimConnectStructs.AircraftStatusSlowStruct data)
        {
            Data = data;

            var refresh = _lastSentData == null;
            _lastSentData ??= new SimConnectStructs.AircraftStatusSlowStruct();

            refresh = refresh || _lastSentData != data;

            if (data.Latitude == 0 || data.Longitude == 0 || !refresh) return;

            _lastSentData = data;

            /* null island ???
              0.00040748246254171134 0.01397450300629543
              0.000407520306147189   0.01397450300629543
              */

            myHub.Clients.All.SendAsync("MsFsSlowRefresh");

            mqtt.Publish(data, "slow");
        }
    }
}
