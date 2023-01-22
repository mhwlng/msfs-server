using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Microsoft.FlightSimulator.SimConnect;

namespace msfs_server.msfs
{
    public class SimConnectStructs
    {
        public enum DEFINITIONS
        {
            AircraftStatusSlow,
            AircraftStatusFast
        }

        public enum DATA_REQUEST
        {
            AircraftStatusSlow,
            AircraftStatusFast
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct AircraftStatusFastStruct
        {
            [DataDefinition("PLANE BANK DEGREES", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double BankDegrees;

            [DataDefinition("PLANE PITCH DEGREES", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double PitchDegrees;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct AircraftStatusSlowStruct
        {
            [DataDefinition("PLANE LATITUDE", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Latitude;

            [DataDefinition("PLANE LONGITUDE", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Longitude;

            [DataDefinition("PLANE ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Altitude;

            [DataDefinition("FUEL TOTAL QUANTITY", "gallons", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double CurrentFuel;

            [DataDefinition("FUEL TOTAL CAPACITY", "gallons", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double TotalFuel;

            [DataDefinition("PLANE HEADING DEGREES TRUE", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double TrueHeading;

            [DataDefinition("AIRSPEED INDICATED", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double AirspeedIndicated;

            [DataDefinition("AIRSPEED TRUE", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double AirspeedTrue;

            [DataDefinition("NAV HAS NAV", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool NavHasSignal;

            [DataDefinition("NAV HAS DME", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool NavHasDME;

            [DataDefinition("NAV DME", "nautical miles", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double DMEDistance;

            [DataDefinition("GPS IS ACTIVE FLIGHT PLAN", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool GPSFlightPlanActive;

            [DataDefinition("GPS IS ACTIVE WAY POINT", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool GPSWaypointModeActive;

            [DataDefinition("GPS FLIGHT PLAN WP INDEX", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public int GPSWaypointIndex;

            [DataDefinition("GPS WP DISTANCE", "meters", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSWaypointDistance;

            [DataDefinition("GPS WP NEXT LAT", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSNextWPLatitude;

            [DataDefinition("GPS WP NEXT LON", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSNextWPLongitude;

            [DataDefinition("GPS WP PREV LAT", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSPrevWPLatitude;

            [DataDefinition("GPS WP PREV LON", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSPrevWPLongitude;

            [DataDefinition("GPS WP ETE", "seconds", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSWPETE;

            [DataDefinition("AUTOPILOT AVAILABLE", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAvailable;

            [DataDefinition("AUTOPILOT MASTER", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotMaster;

            [DataDefinition("AUTOPILOT WING LEVELER", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotWingLevel;

            [DataDefinition("AUTOPILOT ALTITUDE LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAltitude;

            [DataDefinition("AUTOPILOT APPROACH HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotApproach;

            [DataDefinition("AUTOPILOT BACKCOURSE HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotBackcourse;

            [DataDefinition("AUTOPILOT FLIGHT DIRECTOR ACTIVE", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotFlightDirector;

            [DataDefinition("AUTOPILOT AIRSPEED HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAirspeed;

            [DataDefinition("AUTOPILOT MACH HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotMach;

            [DataDefinition("AUTOPILOT YAW DAMPER", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotYawDamper;

            [DataDefinition("AUTOTHROTTLE ACTIVE", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAutothrottle;

            [DataDefinition("AUTOPILOT VERTICAL HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotVerticalHold;

            [DataDefinition("AUTOPILOT HEADING LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotHeading;

            [DataDefinition("AUTOPILOT NAV1 LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotNav1;



            /*
            data.AddField("ATC ID", type: DataType.String32);
            data.AddField("TITLE", type: DataType.String256);
            data.AddField("PLANE PITCH DEGREES", units: "DEGREES", type: DataType.Float64, epsilon: 1.0f);
            data.AddField("SIM ON GROUND", units: "BOOL", type: DataType.Int32);
             */
        }
    }
}

