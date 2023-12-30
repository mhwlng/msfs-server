using System;

namespace msfs_server.msfs
{
    // thanks to https://github.com/Dragonlaird/SimConnectHelper

    public class SimVarDefinition(
        string name,
        string description,
        string defaultUnit,
        Type unitType,
        bool readOnly,
        bool multiPlayer)
    {
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public string DefaultUnit { get; set; } = defaultUnit;
        public Type UnitType { get; set; } = unitType;
        public bool ReadOnly { get; set; } = readOnly;
        public bool MultiPlayer { get; set; } = multiPlayer;
    }

}
