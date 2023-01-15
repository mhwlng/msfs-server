using System;
using Microsoft.AspNetCore.SignalR;
using msfs_server.Hubs;

namespace msfs_server.msfs
{
    public class AutoPilot
    {
        public bool Available { get; set; }
        public bool Master { get; set; }
        public bool Level { get; set; }
        public bool Altitude { get; set; }
        public bool Approach { get; set; }
        public bool Backcourse { get; set; }
        public bool FlightDirector { get; set; }
        public bool Airspeed { get; set; }
        public bool Mach { get; set; }
        public bool YawDamper { get; set; }
        public bool Autothrottle { get; set; }
        public bool VerticalHold { get; set; }
        public bool Heading { get; set; }
        public bool Nav1 { get; set; }
    }

    public class AircraftStatusModel
    {
      private readonly IHubContext<MyHub> _myHub;
        
      public double Latitude { get; set; }
      public double Longitude { get; set; }
      public double Altitude { get; set; }
      public double TotalFuel { get; set; }
      public double CurrentFuel { get; set; }
      public double TrueHeading { get; set; }
      public double AirspeedIndicated { get; set; }
      public double AirspeedTrue { get; set; }
      public bool NavHasSignal { get; set; }
      public bool NavHasDME { get; set; }
      public double DMEDistance { get; set; }
      public bool GPSFlightPlanActive { get; set; }
      public bool GPSWaypointModeActive { get; set; }
      public int GPSWaypointIndex { get; set; }
      public double GPSWaypointDistance { get; set; }
      public double GPSNextWPLatitude { get; set; }
      public double GPSNextWPLongitude { get; set; }
      public double GPSPrevWPLatitude { get; set; }
      public double GPSPrevWPLongitude { get; set; }
      public double GPSWPETE { get; set; }
      
      public AutoPilot Autopilot { get; set; } = new AutoPilot();

      public AircraftStatusModel(IHubContext<MyHub> myHub)
      {
          _myHub = myHub;
      }

      public void SetData(SimConnectStructs.AircraftStatusStruct status)
      {
         Latitude = status.Latitude;
         Longitude = status.Longitude;
         Altitude = status.Altitude;
         TotalFuel = status.TotalFuel;
         CurrentFuel = status.CurrentFuel;
         TrueHeading = status.TrueHeading;
         AirspeedIndicated = status.AirspeedIndicated;
         AirspeedTrue = status.AirspeedTrue;

         NavHasSignal = status.NavHasSignal;
         NavHasDME = status.NavHasDME;
         DMEDistance = status.DMEDistance;
         GPSFlightPlanActive = status.GPSFlightPlanActive;
         GPSWaypointModeActive = status.GPSWaypointModeActive;
         GPSWaypointIndex = status.GPSWaypointIndex;
         GPSWaypointDistance = status.GPSWaypointDistance;
         GPSNextWPLatitude = status.GPSNextWPLatitude;
         GPSNextWPLongitude = status.GPSNextWPLongitude;
         GPSPrevWPLatitude = status.GPSPrevWPLatitude;
         GPSPrevWPLongitude = status.GPSPrevWPLongitude;
         GPSWPETE = status.GPSWPETE;

         Autopilot = new AutoPilot()
         {
            Available = status.AutopilotAvailable,
            Master = status.AutopilotMaster,
            FlightDirector = status.AutopilotFlightDirector,
            Airspeed = status.AutopilotAirspeed,
            Altitude = status.AutopilotAltitude,
            Approach = status.AutopilotApproach,
            Autothrottle = status.AutopilotAutothrottle,
            Backcourse = status.AutopilotBackcourse,
            Heading = status.AutopilotHeading,
            Level = status.AutopilotWingLevel,
            Mach = status.AutopilotMach,
            Nav1 = status.AutopilotNav1,
            VerticalHold = status.AutopilotVerticalHold,
            YawDamper = status.AutopilotYawDamper
         };

         _myHub.Clients.All.SendAsync("MsFsRefresh");

      }



    }
}
