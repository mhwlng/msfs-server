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
        public SimConnectStructs.AircraftStatusFastStruct StatusFast = new();
        
        public void SetData(SimConnectStructs.AircraftStatusFastStruct statusFast)
        {
            StatusFast = statusFast;

            myHub.Clients.All.SendAsync("MsFsFastRefresh");

            mqtt.Publish(this,"fast");


        }
    }

    public class AircraftStatusSlowModel(IHubContext<MyHub> myHub, Mqtt mqtt)
    {
        public SimConnectStructs.AircraftStatusSlowStruct StatusSlow = new();
        
        /*
        public double Altitude { get; set; }
        public double TotalFuel { get; set; }
        public double CurrentFuel { get; set; }
         public double AirspeedTrue { get; set; }
        public bool NavHasSignal { get; set; }
        public bool NavHasDME { get; set; }
        public double DMEDistance { get; set; }
        public bool GPSWaypointModeActive { get; set; }
        public int GPSWaypointIndex { get; set; }
        public double GPSWaypointDistance { get; set; }
        public double GPSWPETE { get; set; }

        public bool Available { get; set; }
        public bool Level { get; set; }
        public bool Approach { get; set; }
        public bool Airspeed { get; set; }
        public bool Mach { get; set; }
        public bool Autothrottle { get; set; }

        */


        public void SetData(SimConnectStructs.AircraftStatusSlowStruct statusSlow)
      {

          StatusSlow = statusSlow;
       

            /*
           Altitude = statusSlow.Altitude;
           TotalFuel = statusSlow.TotalFuel;
           CurrentFuel = statusSlow.CurrentFuel;
           AirspeedTrue = statusSlow.AirspeedTrue;

           NavHasSignal = statusSlow.NavHasSignal;
           NavHasDME = statusSlow.NavHasDME;
           DMEDistance = statusSlow.DMEDistance;
           GPSWaypointModeActive = statusSlow.GPSWaypointModeActive;
           GPSWaypointIndex = statusSlow.GPSWaypointIndex;
           GPSWaypointDistance = statusSlow.GPSWaypointDistance;
           GPSWPETE = statusSlow.GPSWPETE;


           Available = statusSlow.AutopilotAvailable;
           Airspeed = statusSlow.AutopilotAirspeed;
           Approach = statusSlow.AutopilotApproach;
           Autothrottle = statusSlow.AutopilotAutothrottle;
   
           Level = statusSlow.AutopilotWingLevel;
           Mach = statusSlow.AutopilotMach;
           */

            myHub.Clients.All.SendAsync("MsFsSlowRefresh");

            mqtt.Publish(this, "slow");


        }



    }
}
