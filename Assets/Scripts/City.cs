using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : MapObject
{
    public City()
    {
        Name = "Brussels";

        EXAMPLE();
    }

    BuildingJob buildingJob;

    float productionPerTurn = 9001;

    override public void SetHex( Hex newHex )
    {
        if(Hex != null)
        {
            // Will a city ever LEAVE a hex and enter a new one?
            Hex.RemoveCity(this);
        }

        base.SetHex( newHex );

        Hex.AddCity( this );
    }

    public void DoTurn()
    {
        if(buildingJob != null)
        {
            float workLeft = buildingJob.DoWork( productionPerTurn );
            if(workLeft <= 0)
            {
                // Job is complete
                buildingJob = null;
                // TODO: Save overflow
            }
        }
    }

    void EXAMPLE()
    {
        buildingJob = new BuildingJob(
            null, 
            "Dwarf Warrior",
            100,
            0,
            () => {
                this.Hex.HexMap.SpawnUnitAt(
                    new Unit(),
                    this.Hex.HexMap.UnitDwarfPrefab,
                    this.Hex.Q,
                    this.Hex.R
                );
            },
            null
        );
    }

}

