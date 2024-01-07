using System;

namespace msfs_server.msfs
{
    // thanks to https://github.com/Dragonlaird/SimConnectHelper

    public class SimVarDefinition
    {
        public SimVarDefinition(string name, string description, string defaultUnit, Type unitType, bool readOnly, bool multiPlayer)
        {
            Name = name;
            Description = description;
            DefaultUnit = defaultUnit;
            UnitType = unitType;
            ReadOnly = readOnly;
            MultiPlayer = multiPlayer;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultUnit { get; set; }
        public Type UnitType { get; set; }
        public bool ReadOnly { get; set; }
        public bool MultiPlayer { get; set; }
    }

}
