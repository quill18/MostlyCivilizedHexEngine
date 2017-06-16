using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// The Hex class defines the grid position, world space position, size, 
/// neighbours, etc... of a Hex Tile. However, it does NOT interact with
/// Unity directly in any way.
/// </summary>
public class Hex {

    public Hex(HexMap hexMap, int q, int r)
    {
        this.HexMap = hexMap;

        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }

    // Q + R + S = 0
    // S = -(Q + R)

    public readonly int Q;  // Column
    public readonly int R;  // Row
    public readonly int S;

    // Data for map generation and maybe in-game effects
    public float Elevation;
    public float Moisture;

    public readonly HexMap HexMap;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    float radius = 1f;

    HashSet<Unit> units;

    /// <summary>
    /// Returns the world-space position of this hex
    /// </summary>
    public Vector3 Position()
    {
        return new Vector3(
            HexHorizontalSpacing() * (this.Q + this.R/2f),
            0,
            HexVerticalSpacing() * this.R
        );
    }

    public float HexHeight()
    {
        return radius * 2;
    }

    public float HexWidth()
    {
        return WIDTH_MULTIPLIER * HexHeight();
    }

    public float HexVerticalSpacing()
    {
        return HexHeight() * 0.75f;
    }

    public float HexHorizontalSpacing()
    {
        return HexWidth();
    }

    public Vector3 PositionFromCamera()
    {
        return HexMap.GetHexPosition(this);
    }

    public Vector3 PositionFromCamera( Vector3 cameraPosition, float numRows, float numColumns )
    {
        float mapHeight = numRows * HexVerticalSpacing();
        float mapWidth  = numColumns * HexHorizontalSpacing();

        Vector3 position = Position();

        if(HexMap.AllowWrapEastWest)
        {
            float howManyWidthsFromCamera = Mathf.Round((position.x - cameraPosition.x) / mapWidth);
            int howManyWidthToFix = (int)howManyWidthsFromCamera;

            position.x -= howManyWidthToFix * mapWidth;
        }

        if(HexMap.AllowWrapNorthSouth)
        {
            float howManyHeightsFromCamera = Mathf.Round((position.z - cameraPosition.z) / mapHeight);
            int howManyHeightsToFix = (int)howManyHeightsFromCamera;

            position.z -= howManyHeightsToFix * mapHeight;
        }


        return position;
    }

    public static float Distance(Hex a, Hex b)
    {
        // WARNING: Probably Wrong for wrapping
        int dQ = Mathf.Abs(a.Q - b.Q);
        if(a.HexMap.AllowWrapEastWest)
        {
            if(dQ > a.HexMap.NumColumns / 2)
                dQ = a.HexMap.NumColumns - dQ;
        }

        int dR = Mathf.Abs(a.R - b.R);
        if(a.HexMap.AllowWrapNorthSouth)
        {
            if(dR > a.HexMap.NumRows / 2)
                dR = a.HexMap.NumRows - dR;
        }

        return 
            Mathf.Max( 
                dQ,
                dR,
                Mathf.Abs(a.S - b.S)
            );
    }

    public void AddUnit( Unit unit )
    {
        if(units == null)
        {
            units = new HashSet<Unit>();
        }

        units.Add(unit);
    }

    public void RemoveUnit( Unit unit )
    {
        if(units != null)
        {
            units.Remove(unit);
        }
    }

    public Unit[] Units()
    {
        return units.ToArray();
    }

}
