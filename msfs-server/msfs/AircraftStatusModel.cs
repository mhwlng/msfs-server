using System;
using System.Windows.Controls.Primitives;
using Microsoft.AspNetCore.SignalR;
using Microsoft.FlightSimulator.SimConnect;
using msfs_server.Hubs;

namespace msfs_server.msfs
{

    public class AircraftStatusFastModel
    {
        private readonly IHubContext<MyHub> _myHub;

        public double BankDegrees { get; set; }

        public double PitchDegrees { get; set; }

        public double IndicatedAltitude { get; set; }

        public double VerticalSpeed { get; set; }

        public double AirspeedIndicated { get; set; }

        public double GpsGroundSpeed { get; set; }

        public double KohlsmanSetting { get; set; }

        public double PlaneHeadingMagnetic { get; set; }
  
        public double TurnCoordinatorBall { get; set; }

        public double Nav1CDI { get; set; }

        public double Nav1GSI { get; set; }

        public double Nav1OBS { get; set; }

        public double ElevatorTrimPosition { get; set; }

        public double AutoPilotAltitudeLockVar { get; set; }

        public double AutoPilotHeadingLockDir { get; set; }


        public bool AutopilotMaster { get; set; }

        public bool AutopilotHeadingLock { get; set; }

        public bool AutopilotAltitudeLock { get; set; }

        public bool AutopilotNav1Lock { get; set; }

        public bool AutopilotFlightDirectorActive { get; set; }

        public bool AutopilotBackcourseHold { get; set; }

        public bool AutopilotVerticalHold { get; set; }

        public bool AutopilotYawDamper { get; set; }

        public bool AutopilotApproachHold { get; set; }

        public double GeneralEngineOilPressure { get; set; }

        public double GeneralEngineOilTemperature { get; set; }
        

        public AircraftStatusFastModel(IHubContext<MyHub> myHub)
        {
            _myHub = myHub;
        }

        public void SetData(SimConnectStructs.AircraftStatusFastStruct statusFast)
        {
            BankDegrees = statusFast.BankDegrees;
            PitchDegrees = statusFast.PitchDegrees;
            IndicatedAltitude = statusFast.IndicatedAltitude;
            VerticalSpeed = statusFast.VerticalSpeed;
            AirspeedIndicated = statusFast.AirspeedIndicated;

            GpsGroundSpeed = statusFast.GpsGroundSpeed;
            PlaneHeadingMagnetic = statusFast.PlaneHeadingMagnetic;
            KohlsmanSetting = statusFast.KohlsmanSetting;

            TurnCoordinatorBall = statusFast.TurnCoordinatorBall;

            Nav1CDI = statusFast.Nav1CDI;
            Nav1GSI = statusFast.Nav1GSI;
            Nav1OBS = statusFast.Nav1OBS;

            ElevatorTrimPosition = statusFast.ElevatorTrimPosition;

            AutoPilotAltitudeLockVar = statusFast.AutoPilotAltitudeLockVar;
            AutoPilotHeadingLockDir = statusFast.AutoPilotHeadingLockDir;
            
            AutopilotMaster = statusFast.AutopilotMaster;

            AutopilotAltitudeLock = statusFast.AutopilotAltitudeLock;

            AutopilotHeadingLock = statusFast.AutopilotHeadingLock;

            AutopilotNav1Lock = statusFast.AutopilotNav1Lock;

            AutopilotFlightDirectorActive = statusFast.AutopilotFlightDirectorActive;

            AutopilotBackcourseHold = statusFast.AutopilotBackcourseHold;

            AutopilotVerticalHold = statusFast.AutopilotVerticalHold;

            AutopilotYawDamper = statusFast.AutopilotYawDamper;

            AutopilotApproachHold = statusFast.AutopilotApproachHold;

            GeneralEngineOilTemperature = ((5.0/9.0 * 1.086) * statusFast.GeneralEngineOilTemperature) - 273.15; // convert to celcius

            GeneralEngineOilPressure = statusFast.GeneralEngineOilPressure / 144.0; // convert to psi
            
            _myHub.Clients.All.SendAsync("MsFsFastRefresh");

        }
    }

    public class AircraftStatusSlowModel
    {
      private readonly IHubContext<MyHub> _myHub;

      public double Latitude { get; set; }
      public double Longitude { get; set; }
      public double TrueHeading { get; set; }

      public bool GPSFlightPlanActive { get; set; }

      public double GPSNextWPLatitude { get; set; }
      public double GPSNextWPLongitude { get; set; }
      public double GPSPrevWPLatitude { get; set; }
      public double GPSPrevWPLongitude { get; set; }

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


        public AircraftStatusSlowModel(IHubContext<MyHub> myHub)
      {
          _myHub = myHub;
      }

      public void SetData(SimConnectStructs.AircraftStatusSlowStruct statusSlow)
      {
          Latitude = statusSlow.Latitude;
          Longitude = statusSlow.Longitude;
          TrueHeading = statusSlow.TrueHeading;

          GPSFlightPlanActive = statusSlow.GPSFlightPlanActive;

          GPSNextWPLatitude = statusSlow.GPSNextWPLatitude;
          GPSNextWPLongitude = statusSlow.GPSNextWPLongitude;
          GPSPrevWPLatitude = statusSlow.GPSPrevWPLatitude;
          GPSPrevWPLongitude = statusSlow.GPSPrevWPLongitude;

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

            _myHub.Clients.All.SendAsync("MsFsSlowRefresh");

      }



    }
}
