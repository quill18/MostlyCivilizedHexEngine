using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap {



    override public void GenerateMap()
    {
        // First, call the base version to make all the hexes we need
        base.GenerateMap();

        int numContinents = 3;
        int continentSpacing = NumColumns / numContinents;

        // Uncomment this to generate the same "random" terrain every time.
        Random.InitState(0);


        for (int c = 0; c < numContinents; c++)
        {
            // Make some kind of raised area
            int numSplats = Random.Range(4, 8);
            for (int i = 0; i < numSplats; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, NumRows - range);
                int x = Random.Range(0, 10) - y/2 + (c * continentSpacing);

                ElevateArea(x, y, range);
            }

        }

        // Add lumpiness Perlin Noise?
        float noiseResolution = 0.01f;
        Vector2 noiseOffset = new Vector2( Random.Range(0f, 1f), Random.Range(0f, 1f) ); 

        float noiseScale = 2f;  // Larger values makes more islands (and lakes, I guess)


        for (int column = 0; column < NumColumns; column++)
        {
            for (int row = 0; row < NumRows; row++)
            {
                Hex h = GetHexAt(column, row);
                float n = 
                    Mathf.PerlinNoise( ((float)column/Mathf.Max(NumColumns,NumRows) / noiseResolution) + noiseOffset.x, 
                        ((float)row/Mathf.Max(NumColumns,NumRows) / noiseResolution) + noiseOffset.y )
                    - 0.5f;
                h.Elevation += n * noiseScale;
            }
        }

        // Simulate rainfall/moisture (probably just Perlin it for now) and set plains/grasslands + forest 
        noiseResolution = 0.05f;
        noiseOffset = new Vector2( Random.Range(0f, 1f), Random.Range(0f, 1f) ); 

        noiseScale = 2f;  // Larger values makes more islands (and lakes, I guess)


        for (int column = 0; column < NumColumns; column++)
        {
            for (int row = 0; row < NumRows; row++)
            {
                Hex h = GetHexAt(column, row);
                float n = 
                    Mathf.PerlinNoise( ((float)column/Mathf.Max(NumColumns,NumRows) / noiseResolution) + noiseOffset.x, 
                        ((float)row/Mathf.Max(NumColumns,NumRows) / noiseResolution) + noiseOffset.y )
                    - 0.5f;
                h.Moisture = n * noiseScale;
            }
        }


        // Now make sure all the hex visuals are updated to match the data.

        UpdateHexVisuals();

        Unit unit = new Unit();

        // For development, turn on CanBuildCities on this unit
        unit.CanBuildCities = true;

        SpawnUnitAt(unit, UnitDwarfPrefab, 36, 15);

        City city = new City();
        SpawnCityAt(city, CityPrefab, 35, 15);
    }

    void ElevateArea(int q, int r, int range, float centerHeight = .8f)
    {
        Hex centerHex = GetHexAt(q, r);

        Hex[] areaHexes = GetHexesWithinRangeOf(centerHex, range);

        foreach(Hex h in areaHexes)
        {
            //if(h.Elevation < 0)
                //h.Elevation = 0;
            
            h.Elevation = centerHeight * Mathf.Lerp( 1f, 0.25f, Mathf.Pow(Hex.Distance(centerHex, h) / range,2f) );
        }
    }

}
