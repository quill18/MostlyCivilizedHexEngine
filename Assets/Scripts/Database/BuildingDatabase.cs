using System;
using System.Linq;
using System.Collections.Generic;

public class BuildingBlueprint
{
    public BuildingBlueprint( int id, string name )
    {
        MyID = id;
        Name = name;
    }
    
    public readonly int MyID;
    public string Name;
    public int ProductionCost = 0;
    public string IconFilename = null;
    public int TechID = -1;  // The technology that unlocks this building
    public int ObsoleteTechId = -1; 

    public enum BUILDING_TYPE { BUILDING, GLOBAL_WONDER, NATIONAL_WONDER };
    public BUILDING_TYPE isGlobalWonder = BUILDING_TYPE.BUILDING;
}

static public class BuildingDatabase
{
    static BuildingDatabase()
    {
        buildings = new Dictionary<int, BuildingBlueprint>();
        LoadBuildingBlueprints();
    }

    static Dictionary<int, BuildingBlueprint> buildings;
    // Consider also keeping a list or maybe even just already
    // have an array version

    static public BuildingBlueprint[] GetListOfBuilding()
    {
        return buildings.Values.ToArray();
    }

    static public BuildingBlueprint GetBuildingById( int id )
    {
        // TODO: Check for exceptions?
        return buildings[id];
    }

    static void LoadBuildingBlueprints()
    {
        // Maybe from an XML file?

        BuildingBlueprint bb;
        int id = 0;

        bb = new BuildingBlueprint(
            id,
            "Barracks"
        );
        buildings.Add(id++, bb);

        bb = new BuildingBlueprint(
            id,
            "Granary"
        );
        buildings.Add(id++, bb);

        bb = new BuildingBlueprint(
            id,
            "Monument"
        );
        buildings.Add(id++, bb);

        bb = new BuildingBlueprint(
            id,
            "Library"
        );
        buildings.Add(id++, bb);

    }
}


