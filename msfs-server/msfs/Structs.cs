using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Xml.Linq;
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

            [DataDefinition("VERTICAL SPEED", "feet per second", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double VerticalSpeed;

            [DataDefinition("AIRSPEED INDICATED", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double AirspeedIndicated;

            [DataDefinition("GPS GROUND SPEED", "meters per second", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GpsGroundSpeed;

            [DataDefinition("PLANE HEADING DEGREES MAGNETIC", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double PlaneHeadingMagnetic;

            [DataDefinition("KOHLSMAN SETTING MB", "millibars", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double KohlsmanSetting;

            [DataDefinition("TURN COORDINATOR BALL", "Position 128", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double TurnCoordinatorBall;

            [DataDefinition("NAV CDI:1", "number", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Nav1CDI;

            [DataDefinition("NAV GSI:1", "number", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Nav1GSI;
            
            [DataDefinition("NAV OBS:1", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double Nav1OBS;

            [DataDefinition("ELEVATOR TRIM POSITION",  "Degrees",  SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double ElevatorTrimPosition;





            [DataDefinition("AUTOPILOT ALTITUDE LOCK VAR", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double AutoPilotAltitudeLockVar;


            [DataDefinition("AUTOPILOT HEADING LOCK DIR", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double AutoPilotHeadingLockDir;


            [DataDefinition("AUTOPILOT MASTER", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotMaster;

            [DataDefinition("AUTOPILOT HEADING LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotHeadingLock;

            [DataDefinition("AUTOPILOT ALTITUDE LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAltitudeLock;

            [DataDefinition("AUTOPILOT NAV1 LOCK", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotNav1Lock;

            [DataDefinition("AUTOPILOT FLIGHT DIRECTOR ACTIVE", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotFlightDirectorActive;

            [DataDefinition("AUTOPILOT BACKCOURSE HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotBackcourseHold;

            [DataDefinition("AUTOPILOT VERTICAL HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotVerticalHold;

            [DataDefinition("AUTOPILOT YAW DAMPER", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotYawDamper;

            [DataDefinition("AUTOPILOT APPROACH HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotApproachHold;


            [DataDefinition("GENERAL ENG OIL TEMPERATURE:1", "rankine", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GeneralEngineOilTemperature;

            [DataDefinition("GENERAL ENG OIL PRESSURE:1", "psf", SIMCONNECT_DATATYPE.FLOAT64, 0.0f)]
            public double GeneralEngineOilPressure;


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

 
            [DataDefinition("AUTOPILOT AIRSPEED HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAirspeed;

            [DataDefinition("AUTOPILOT MACH HOLD", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotMach;


            [DataDefinition("AUTOTHROTTLE ACTIVE", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
            public bool AutopilotAutothrottle;


        


            
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


                         ("TITLE", "Title", null, SIMCONNECT_DATATYPE.STRING256),
                ("PLANE_LATITUDE", "PLANE LATITUDE", "degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("PLANE_LONGITUDE", "PLANE LONGITUDE", "degrees", SIMCONNECT_DATATYPE.FLOAT64),

                ("PLANE_HEADING_DEGREES_MAGNETIC", "PLANE HEADING DEGREES MAGNETIC", "degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("PLANE_HEADING_DEGREES_TRUE", "PLANE HEADING DEGREES TRUE", "degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("PLANE_ALTITUDE", "PLANE ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64),
                ("PLANE_ALT_ABOVE_GROUND", "PLANE ALT ABOVE GROUND", "feet", SIMCONNECT_DATATYPE.FLOAT64),
                ("AIRSPEED_INDICATED", "AIRSPEED INDICATED", "knots", SIMCONNECT_DATATYPE.FLOAT64),
                ("VERTICAL_SPEED", "VERTICAL SPEED", "feet/minute", SIMCONNECT_DATATYPE.FLOAT64),
                ("PLANE_PITCH_DEGREES", "PLANE PITCH DEGREES", "degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("PLANE_BANK_DEGREES", "PLANE BANK DEGREES", "degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("KOHLSMAN_SETTING_HG", "KOHLSMAN SETTING HG", "inHg", SIMCONNECT_DATATYPE.FLOAT64),

                ("FLAPS_HANDLE_INDEX", "FLAPS HANDLE INDEX", "number", SIMCONNECT_DATATYPE.FLOAT64),
                ("FLAPS_HANDLE_PERCENT", "FLAPS HANDLE PERCENT", "percent", SIMCONNECT_DATATYPE.FLOAT64),
                ("TRAILING_EDGE_FLAPS_LEFT_ANGLE", "TRAILING EDGE FLAPS LEFT ANGLE", "degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("ELEVATOR_TRIM_PCT", "ELEVATOR TRIM PCT", "percent", SIMCONNECT_DATATYPE.FLOAT64),
                ("AILERON_RIGHT_DEFLECTION", "AILERON RIGHT DEFLECTION", "radians", SIMCONNECT_DATATYPE.FLOAT64),

                ("GEAR_POSITION", "GEAR POSITION", "enum", SIMCONNECT_DATATYPE.FLOAT64),
                ("GEAR_CENTER_POSITION", "GEAR CENTER POSITION", "percent", SIMCONNECT_DATATYPE.FLOAT64),
                ("GEAR_LEFT_POSITION", "GEAR LEFT POSITION", "percent", SIMCONNECT_DATATYPE.FLOAT64),
                ("GEAR_RIGHT_POSITION", "GEAR RIGHT POSITION", "percent", SIMCONNECT_DATATYPE.FLOAT64),

                ("TRANSPONDER_CODE", "TRANSPONDER CODE:1", "number", SIMCONNECT_DATATYPE.FLOAT64),

                ("GPS_DRIVES_NAV1", "GPS DRIVES NAV1", "bool", SIMCONNECT_DATATYPE.FLOAT64),

                ("AUTOPILOT_ALTITUDE_LOCK_VAR", "AUTOPILOT ALTITUDE LOCK VAR:1", "feet", SIMCONNECT_DATATYPE.FLOAT64),
                ("AUTOPILOT_HEADING_LOCK_DIR", "AUTOPILOT HEADING LOCK DIR:1", "degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("AUTOPILOT_VERTICAL_HOLD_VAR", "AUTOPILOT VERTICAL HOLD VAR", "feet/minute", SIMCONNECT_DATATYPE.FLOAT64),
                ("AUTOPILOT_FLIGHT_LEVEL_CHANGE", "AUTOPILOT FLIGHT LEVEL CHANGE", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("AUTOPILOT_AIRSPEED_HOLD_VAR", "AUTOPILOT AIRSPEED HOLD VAR", "knots", SIMCONNECT_DATATYPE.FLOAT64),
                ("AUTOPILOT_WING_LEVELER", "AUTOPILOT WING LEVELER", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("AUTOPILOT_THROTTLE_ARM", "AUTOPILOT THROTTLE ARM", "bool", SIMCONNECT_DATATYPE.FLOAT64),

                ("ELECTRICAL_MASTER_BATTERY", "ELECTRICAL MASTER BATTERY", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("GENERAL_ENG_MASTER_ALTERNATOR_1", "GENERAL ENG MASTER ALTERNATOR:1", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("AVIONICS_MASTER_SWITCH", "AVIONICS MASTER SWITCH", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("GENERAL_ENG_FUEL_PUMP_SWITCH", "GENERAL ENG FUEL PUMP SWITCH:1", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("STRUCTURAL_DEICE_SWITCH", "STRUCTURAL DEICE SWITCH", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("PITOT_HEAT", "PITOT HEAT", "bool", SIMCONNECT_DATATYPE.FLOAT64),

                ("LIGHT_LANDING", "LIGHT LANDING", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("LIGHT_TAXI", "LIGHT TAXI", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("LIGHT_NAV", "LIGHT NAV", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("LIGHT_BEACON", "LIGHT BEACON", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("LIGHT_STROBE", "LIGHT STROBE", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("LIGHT_PANEL", "LIGHT PANEL", "bool", SIMCONNECT_DATATYPE.FLOAT64),

                ("PROP_RPM_1", "PROP RPM:1", "rpm", SIMCONNECT_DATATYPE.FLOAT64),
                ("TURB_ENG_CORRECTED_N1_1", "TURB ENG CORRECTED N1:1", "percent", SIMCONNECT_DATATYPE.FLOAT64),

                ("GENERAL_ENG_RPM_1", "GENERAL ENG RPM:1", "rpm", SIMCONNECT_DATATYPE.FLOAT64),
                ("GENERAL_ENG_THROTTLE_LEVER_POSITION_1", "GENERAL ENG THROTTLE LEVER POSITION:1", "percent", SIMCONNECT_DATATYPE.FLOAT64),
                ("GENERAL_ENG_MIXTURE_LEVER_POSITION_1", "GENERAL ENG MIXTURE LEVER POSITION:1", "percent", SIMCONNECT_DATATYPE.FLOAT64),

                ("NAV_ACTIVE_FREQUENCY_1", "NAV ACTIVE FREQUENCY:1", "mhz", SIMCONNECT_DATATYPE.FLOAT64),
                ("NAV_STANDBY_FREQUENCY_1", "NAV STANDBY FREQUENCY:1", "mhz", SIMCONNECT_DATATYPE.FLOAT64),
                ("NAV_ACTIVE_FREQUENCY_2", "NAV ACTIVE FREQUENCY:2", "mhz", SIMCONNECT_DATATYPE.FLOAT64),
                ("NAV_STANDBY_FREQUENCY_2", "NAV STANDBY FREQUENCY:2", "mhz", SIMCONNECT_DATATYPE.FLOAT64),

                ("ADF_ACTIVE_FREQUENCY_1", "ADF ACTIVE FREQUENCY:1", "Frequency ADF BCD32", SIMCONNECT_DATATYPE.FLOAT64),
                ("ADF_STANDBY_FREQUENCY_1", "ADF STANDBY FREQUENCY:1", "Frequency ADF BCD32", SIMCONNECT_DATATYPE.FLOAT64),
                ("ADF_CARD", "ADF CARD", "Degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("NAV_OBS_1", "NAV OBS:1", "Degrees", SIMCONNECT_DATATYPE.FLOAT64),
                ("NAV_OBS_2", "NAV OBS:2", "Degrees", SIMCONNECT_DATATYPE.FLOAT64),

                ("COM_ACTIVE_FREQUENCY_1", "COM ACTIVE FREQUENCY:1", "mhz", SIMCONNECT_DATATYPE.FLOAT64),
                ("COM_STANDBY_FREQUENCY_1", "COM STANDBY FREQUENCY:1", "mhz", SIMCONNECT_DATATYPE.FLOAT64),
                ("COM_ACTIVE_FREQUENCY_2", "COM ACTIVE FREQUENCY:2", "mhz", SIMCONNECT_DATATYPE.FLOAT64),
                ("COM_STANDBY_FREQUENCY_2", "COM STANDBY FREQUENCY:2", "mhz", SIMCONNECT_DATATYPE.FLOAT64),

                ("COM_TRANSMIT_1", "COM TRANSMIT:1", "bool", SIMCONNECT_DATATYPE.FLOAT64),
                ("COM_TRANSMIT_2", "COM TRANSMIT:2", "bool", SIMCONNECT_DATATYPE.FLOAT64),

                ("SIMULATION_RATE", "SIMULATION RATE", "number", SIMCONNECT_DATATYPE.FLOAT64),

                ("GPS_WP_PREV_LAT", "GPS WP PREV LAT", "radians", SIMCONNECT_DATATYPE.FLOAT64),
                ("GPS_WP_PREV_LON", "GPS WP PREV LON", "radians", SIMCONNECT_DATATYPE.FLOAT64),
                ("GPS_WP_NEXT_LAT", "GPS WP NEXT LAT", "radians", SIMCONNECT_DATATYPE.FLOAT64),
                ("GPS_WP_NEXT_LON", "GPS WP NEXT LON", "radians", SIMCONNECT_DATATYPE.FLOAT64),
                ("GPS_POSITION_LAT", "GPS POSITION LAT", "radians", SIMCONNECT_DATATYPE.FLOAT64),
                ("GPS_POSITION_LON", "GPS POSITION LON", "radians", SIMCONNECT_DATATYPE.FLOAT64),
                ("GPS_GROUND_SPEED", "GPS GROUND SPEED", "meters per second", SIMCONNECT_DATATYPE.FLOAT64)

             */
        }
    }
}

