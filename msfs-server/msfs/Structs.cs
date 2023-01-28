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

            [DataDefinition("INDICATED ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double IndicatedAltitude;

            [DataDefinition("VERTICAL SPEED", "feet per second", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public int VerticalSpeed;

            [DataDefinition("AIRSPEED INDICATED", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double AirspeedIndicated;

            [DataDefinition("GPS GROUND SPEED", "meters per second", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GpsGroundSpeed;

            [DataDefinition("PLANE HEADING DEGREES MAGNETIC", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double PlaneHeadingMagnetic;

            [DataDefinition("KOHLSMAN SETTING MB", "millibars", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double KohlsmanSetting;

            [DataDefinition("AUTOPILOT MASTER", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotMaster;

            [DataDefinition("AUTOPILOT ALTITUDE LOCK VAR", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double AutoPilotAltitudeLockVar;

            [DataDefinition("AUTOPILOT ALTITUDE LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAltitudeLock;

            [DataDefinition("AUTOPILOT HEADING LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotHeadingLock;

            [DataDefinition("AUTOPILOT HEADING LOCK DIR", "degrees", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public int AutoPilotHeadingLockDir;


            [DataDefinition("TURN COORDINATOR BALL", "Position 128", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double TurnCoordinatorBall;

            [DataDefinition("NAV CDI", "number", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double NavCDI;

            [DataDefinition("NAV GSI", "number", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double NavGSI;


        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct AircraftStatusSlowStruct
        {
            [DataDefinition("PLANE LATITUDE", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Latitude;

            [DataDefinition("PLANE LONGITUDE", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Longitude;

            [DataDefinition("PLANE HEADING DEGREES TRUE", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double TrueHeading;

            [DataDefinition("GPS IS ACTIVE FLIGHT PLAN", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool GPSFlightPlanActive;

            [DataDefinition("GPS WP NEXT LAT", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSNextWPLatitude;

            [DataDefinition("GPS WP NEXT LON", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSNextWPLongitude;

            [DataDefinition("GPS WP PREV LAT", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSPrevWPLatitude;

            [DataDefinition("GPS WP PREV LON", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSPrevWPLongitude;

            /*
            [DataDefinition("PLANE ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Altitude;

            [DataDefinition("FUEL TOTAL QUANTITY", "gallons", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double CurrentFuel;

            [DataDefinition("FUEL TOTAL CAPACITY", "gallons", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double TotalFuel;

 
            [DataDefinition("AIRSPEED TRUE", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double AirspeedTrue;

            [DataDefinition("NAV HAS NAV", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool NavHasSignal;

            [DataDefinition("NAV HAS DME", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool NavHasDME;

            [DataDefinition("NAV DME", "nautical miles", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double DMEDistance;


            [DataDefinition("GPS IS ACTIVE WAY POINT", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool GPSWaypointModeActive;

            [DataDefinition("GPS FLIGHT PLAN WP INDEX", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public int GPSWaypointIndex;

            [DataDefinition("GPS WP DISTANCE", "meters", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSWaypointDistance;

            [DataDefinition("GPS WP ETE", "seconds", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GPSWPETE;

            [DataDefinition("AUTOPILOT AVAILABLE", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAvailable;

        
            [DataDefinition("AUTOPILOT WING LEVELER", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotWingLevel;


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

            [DataDefinition("AUTOPILOT NAV1 LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotNav1;



            
            data.AddField("ATC ID", type: DataType.String32);
            data.AddField("TITLE", type: DataType.String256);
            data.AddField("PLANE PITCH DEGREES", units: "DEGREES", type: DataType.Float64, epsilon: 1.0f);
            data.AddField("SIM ON GROUND", units: "BOOL", type: DataType.Int32);



  
            
        [SimConnectVariable(Name = "PLANE HEADING DEGREES GYRO", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64, Minimum = 0, Maximum = 360, Default = nameof(MagneticHeading))]
        public double GyroHeading;
        [SimConnectVariable(Name = "PLANE HEADING DEGREES TRUE", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64, Minimum = 0, Maximum = 360)]
        public double TrueHeading;
        [SimConnectVariable(Name = "PLANE HEADING DEGREES MAGNETIC", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64, Minimum = 0, Maximum = 360)]
        public double MagneticHeading;

        [SimConnectVariable(Name = "VELOCITY BODY X", Unit = "Feet per second", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double VelocityBodyX;
        [SimConnectVariable(Name = "VELOCITY BODY Y", Unit = "Feet per second", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double VelocityBodyY;
        [SimConnectVariable(Name = "VELOCITY BODY Z", Unit = "Feet per second", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double VelocityBodyZ;

        [SimConnectVariable(Name = "AILERON POSITION", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double AileronPosition;
        [SimConnectVariable(Name = "ELEVATOR POSITION", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double ElevatorPosition;
        [SimConnectVariable(Name = "RUDDER POSITION", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double RudderPosition;

        [SimConnectVariable(Name = "ELEVATOR TRIM POSITION", Unit = "Radians", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double ElevatorTrimPosition;
        [SimConnectVariable(Name = "AILERON TRIM PCT", Unit = "Percent Over 100", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double AileronTrimPercent;
        [SimConnectVariable(Name = "RUDDER TRIM PCT", Unit = "Percent Over 100", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double RudderTrimPercent;

        [SimConnectVariable(Name = "FLAPS HANDLE INDEX", Unit = "Number", Type = SIMCONNECT_DATATYPE.INT32)]
        public uint FlapsHandleIndex;
        [SimConnectVariable(Name = "TRAILING EDGE FLAPS LEFT PERCENT", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double TrailingEdgeFlapsLeftPercent;
        [SimConnectVariable(Name = "TRAILING EDGE FLAPS RIGHT PERCENT", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double TrailingEdgeFlapsRightPercent;
        [SimConnectVariable(Name = "LEADING EDGE FLAPS LEFT PERCENT", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double LeadingEdgeFlapsLeftPercent;
        [SimConnectVariable(Name = "LEADING EDGE FLAPS RIGHT PERCENT", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double LeadingEdgeFlapsRightPercent;

        [SimConnectVariable(Name = "GENERAL ENG THROTTLE LEVER POSITION:1", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double ThrottleLeverPosition1;
        [SimConnectVariable(Name = "GENERAL ENG THROTTLE LEVER POSITION:2", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double ThrottleLeverPosition2;
        [SimConnectVariable(Name = "GENERAL ENG THROTTLE LEVER POSITION:3", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double ThrottleLeverPosition3;
        [SimConnectVariable(Name = "GENERAL ENG THROTTLE LEVER POSITION:4", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double ThrottleLeverPosition4;

        [SimConnectVariable(Name = "GENERAL ENG PROPELLER LEVER POSITION:1", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double PropellerLeverPosition1;
        [SimConnectVariable(Name = "GENERAL ENG PROPELLER LEVER POSITION:2", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double PropellerLeverPosition2;
        [SimConnectVariable(Name = "GENERAL ENG PROPELLER LEVER POSITION:3", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double PropellerLeverPosition3;
        [SimConnectVariable(Name = "GENERAL ENG PROPELLER LEVER POSITION:4", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double PropellerLeverPosition4;

        [SimConnectVariable(Name = "SPOILERS HANDLE POSITION", Unit = "Percent", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double SpoilerHandlePosition;
        [SimConnectVariable(Name = "GEAR HANDLE POSITION", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32)]
        public uint GearHandlePosition;
        [SimConnectVariable(Name = "WATER RUDDER HANDLE POSITION", Unit = "Percent Over 100", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double WaterRudderHandlePosition;

        [SimConnectVariable(Name = "BRAKE LEFT POSITION", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double BrakeLeftPosition;
        [SimConnectVariable(Name = "BRAKE RIGHT POSITION", Unit = "Position", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double BrakeRightPosition;

        // Some variables that can only be set by triggering events
        [SimConnectVariable(Name = "BRAKE PARKING POSITION", Unit = "Position", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "PARKING_BRAKES")]
        public uint BrakeParkingPosition;

        [SimConnectVariable(Name = "LIGHT TAXI", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "TOGGLE_TAXI_LIGHTS")]
        public uint LightTaxi;
        [SimConnectVariable(Name = "LIGHT LANDING", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "LANDING_LIGHTS_TOGGLE")]
        public uint LightLanding;
        [SimConnectVariable(Name = "LIGHT STROBE", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "STROBES_TOGGLE")]
        public uint LightStrobe;
        [SimConnectVariable(Name = "LIGHT BEACON", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "TOGGLE_BEACON_LIGHTS")]
        public uint LightBeacon;
        [SimConnectVariable(Name = "LIGHT NAV", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "TOGGLE_NAV_LIGHTS")]
        public uint LightNav;
        [SimConnectVariable(Name = "LIGHT WING", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "TOGGLE_WING_LIGHTS")]
        public uint LightWing;
        [SimConnectVariable(Name = "LIGHT LOGO", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "TOGGLE_LOGO_LIGHTS")]
        public uint LightLogo;
        [SimConnectVariable(Name = "LIGHT RECOGNITION", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "TOGGLE_RECOGNITION_LIGHTS")]
        public uint LightRecognition;
        [SimConnectVariable(Name = "LIGHT CABIN", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.Event, SetByEvent = "TOGGLE_CABIN_LIGHTS")]
        public uint LightCabin;

        // Some variables that are only for info and display
        [SimConnectVariable(Name = "SIMULATION RATE", Unit = "Number", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.None)]
        public uint SimulationRate;
        [SimConnectVariable(Name = "ABSOLUTE TIME", Unit = "Seconds", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double AbsoluteTime;
        [SimConnectVariable(Name = "PLANE ALT ABOVE GROUND", Unit = "Feet", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double AltitudeAboveGround;
        [SimConnectVariable(Name = "SIM ON GROUND", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.None)]
        public uint IsOnGround;
        [SimConnectVariable(Name = "AMBIENT WIND VELOCITY", Unit = "Knots", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double WindVelocity;
        [SimConnectVariable(Name = "AMBIENT WIND DIRECTION", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double WindDirection;
        [SimConnectVariable(Name = "G FORCE", Unit = "GForce", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double GForce;
        [SimConnectVariable(Name = "PLANE TOUCHDOWN NORMAL VELOCITY", Unit = "Feet per minute", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double TouchdownNormalVelocity;
        [SimConnectVariable(Name = "WING FLEX PCT:1", Unit = "Percent over 100", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double WingFlexPercent1;
        [SimConnectVariable(Name = "WING FLEX PCT:2", Unit = "Percent over 100", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double WingFlexPercent2;
        [SimConnectVariable(Name = "WING FLEX PCT:3", Unit = "Percent over 100", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double WingFlexPercent3;
        [SimConnectVariable(Name = "WING FLEX PCT:4", Unit = "Percent over 100", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double WingFlexPercent4;
        [SimConnectVariable(Name = "AIRSPEED TRUE", Unit = "Knots", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double TrueAirspeed;
        [SimConnectVariable(Name = "AIRSPEED INDICATED", Unit = "Knots", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double IndicatedAirspeed;
        [SimConnectVariable(Name = "AIRSPEED MACH", Unit = "Mach", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double MachAirspeed;
        [SimConnectVariable(Name = "GPS GROUND SPEED", Unit = "Knots", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double GpsGroundSpeed;
        [SimConnectVariable(Name = "GROUND VELOCITY", Unit = "Knots", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double GroundSpeed;
        [SimConnectVariable(Name = "HEADING INDICATOR", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double HeadingIndicator;
        [SimConnectVariable(Name = "ATTITUDE INDICATOR PITCH DEGREES", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double AIPitch;
        [SimConnectVariable(Name = "ATTITUDE INDICATOR BANK DEGREES", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double AIBank;
        [SimConnectVariable(Name = "RECIP ENG MANIFOLD PRESSURE:1", Unit = "Psi", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double EngineManifoldPressure1;
        [SimConnectVariable(Name = "RECIP ENG MANIFOLD PRESSURE:2", Unit = "Psi", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double EngineManifoldPressure2;
        [SimConnectVariable(Name = "RECIP ENG MANIFOLD PRESSURE:3", Unit = "Psi", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double EngineManifoldPressure3;
        [SimConnectVariable(Name = "RECIP ENG MANIFOLD PRESSURE:4", Unit = "Psi", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double EngineManifoldPressure4;
        [SimConnectVariable(Name = "TURN COORDINATOR BALL", Unit = "Position 128", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double TurnCoordinatorBall;
        [SimConnectVariable(Name = "HSI CDI NEEDLE", Unit = "Number", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double HsiCDI;
        [SimConnectVariable(Name = "STALL WARNING", Unit = "Bool", Type = SIMCONNECT_DATATYPE.INT32, SetType = SetType.None)]
        public uint StallWarning;

        [SimConnectVariable(Name = "ROTATION VELOCITY BODY X", Unit = "Radians per second", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double RotationVelocityBodyX;
        [SimConnectVariable(Name = "ROTATION VELOCITY BODY Y", Unit = "Radians per second", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double RotationVelocityBodyY;
        [SimConnectVariable(Name = "ROTATION VELOCITY BODY Z", Unit = "Radians per second", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double RotationVelocityBodyZ;
        [SimConnectVariable(Name = "ACCELERATION BODY X", Unit = "Feet per second squared", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double AccelerationBodyX;
        [SimConnectVariable(Name = "ACCELERATION BODY Y", Unit = "Feet per second squared", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double AccelerationBodyY;
        [SimConnectVariable(Name = "ACCELERATION BODY Z", Unit = "Feet per second squared", Type = SIMCONNECT_DATATYPE.FLOAT64, SetType = SetType.None)]
        public double AccelerationBodyZ;
             */
        }
    }
}

