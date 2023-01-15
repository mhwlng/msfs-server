using Microsoft.FlightSimulator.SimConnect;
using System;

namespace msfs_server.msfs
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DataDefinition : System.Attribute
    {
        private string _datumName;
        private string _unitsName;
        private SIMCONNECT_DATATYPE _datumType;
        private float _fEpsilon;

        public DataDefinition(
            string datumName,
            string unitsName,
            SIMCONNECT_DATATYPE datumType,
            float epsilon)
        {
            this._datumName = datumName;
            this._unitsName = unitsName;
            this._datumType = datumType;
            this._fEpsilon = epsilon;

        }

        public string DatumName
        {
            get => _datumName;
            set => _datumName = value;
        }

        public string UnitsName
        {
            get => _unitsName;
            set => _unitsName = value;
        }

        public SIMCONNECT_DATATYPE DatumType
        {
            get => _datumType;
            set => _datumType = value;
        }

        public float fEpsilon
        {
            get => _fEpsilon;
            set => _fEpsilon = value;
        }



    }
}
