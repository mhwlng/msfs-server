using Microsoft.JSInterop;
using System.Windows;
using msfs_server.Services;
using Microsoft.Extensions.Logging;
using Microsoft.FlightSimulator.SimConnect;

namespace msfs_server.JsInterop
{
    public static class MsFsInterop
    {
        /*
        [JSInvokable]
        public static void JsMouseUp()
        {
          
        }

        [JSInvokable]
        public static void JsClick()
        {
         

        } */


        [JSInvokable]
        public static void AutoPilot(string type)
        {

            if (Worker.Simconnect == null)
            {
                return;
            }

            switch (type)
            {
                case "autopilotMaster":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_AP_MASTER, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
                case "autopilotAltitudeLock":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_AP_ALT_HOLD, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
                case "autopilotHeadingLock":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_AP_HDG_HOLD, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
                case "autopilotNav1Lock":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_AP_NAV1_HOLD, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
                case "autopilotFlightDirectorActive":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_TOGGLE_FLIGHT_DIRECTOR, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
                case "autopilotBackcourseHold":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_AP_BC_HOLD, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
                case "autopilotVerticalHold":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_AP_VS_HOLD, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
                case "autopilotYawDamper":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_YAW_DAMPER_TOGGLE, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
                case "autopilotApproachHold":
                    Worker.Simconnect.TransmitClientEvent(1, Worker.EVENTS.KEY_AP_APR_HOLD, 0, Worker.GROUP_ID.GROUP0, SIMCONNECT_EVENT_FLAG.DEFAULT);
                    break;
            }

        }
    }
}
