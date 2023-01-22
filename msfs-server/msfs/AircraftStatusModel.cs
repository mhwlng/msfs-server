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

    public class AircraftStatusFastModel
    {
        private readonly IHubContext<MyHub> _myHub;

        public double BankDegrees { get; set; }

        public double PitchDegrees { get; set; }

        public double IndicatedAltitude { get; set; }


        public AircraftStatusFastModel(IHubContext<MyHub> myHub)
        {
            _myHub = myHub;
        }

        public void SetData(SimConnectStructs.AircraftStatusFastStruct statusSlow)
        {
            BankDegrees = statusSlow.BankDegrees;
            PitchDegrees = statusSlow.PitchDegrees;
            IndicatedAltitude = statusSlow.IndicatedAltitude;

            _myHub.Clients.All.SendAsync("MsFsFastRefresh");

        }
    }

    public class AircraftStatusSlowModel
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

      public AircraftStatusSlowModel(IHubContext<MyHub> myHub)
      {
          _myHub = myHub;
      }

      public void SetData(SimConnectStructs.AircraftStatusSlowStruct statusSlow)
      {
         Latitude = statusSlow.Latitude;
         Longitude = statusSlow.Longitude;
         Altitude = statusSlow.Altitude;
         TotalFuel = statusSlow.TotalFuel;
         CurrentFuel = statusSlow.CurrentFuel;
         TrueHeading = statusSlow.TrueHeading;
         AirspeedIndicated = statusSlow.AirspeedIndicated;
         AirspeedTrue = statusSlow.AirspeedTrue;

         NavHasSignal = statusSlow.NavHasSignal;
         NavHasDME = statusSlow.NavHasDME;
         DMEDistance = statusSlow.DMEDistance;
         GPSFlightPlanActive = statusSlow.GPSFlightPlanActive;
         GPSWaypointModeActive = statusSlow.GPSWaypointModeActive;
         GPSWaypointIndex = statusSlow.GPSWaypointIndex;
         GPSWaypointDistance = statusSlow.GPSWaypointDistance;
         GPSNextWPLatitude = statusSlow.GPSNextWPLatitude;
         GPSNextWPLongitude = statusSlow.GPSNextWPLongitude;
         GPSPrevWPLatitude = statusSlow.GPSPrevWPLatitude;
         GPSPrevWPLongitude = statusSlow.GPSPrevWPLongitude;
         GPSWPETE = statusSlow.GPSWPETE;

         Autopilot = new AutoPilot()
         {
            Available = statusSlow.AutopilotAvailable,
            Master = statusSlow.AutopilotMaster,
            FlightDirector = statusSlow.AutopilotFlightDirector,
            Airspeed = statusSlow.AutopilotAirspeed,
            Altitude = statusSlow.AutopilotAltitude,
            Approach = statusSlow.AutopilotApproach,
            Autothrottle = statusSlow.AutopilotAutothrottle,
            Backcourse = statusSlow.AutopilotBackcourse,
            Heading = statusSlow.AutopilotHeading,
            Level = statusSlow.AutopilotWingLevel,
            Mach = statusSlow.AutopilotMach,
            Nav1 = statusSlow.AutopilotNav1,
            VerticalHold = statusSlow.AutopilotVerticalHold,
            YawDamper = statusSlow.AutopilotYawDamper
         };

         _myHub.Clients.All.SendAsync("MsFsSlowRefresh");

      }



    }
}
