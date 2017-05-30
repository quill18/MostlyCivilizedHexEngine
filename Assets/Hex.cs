using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Hex class defines the grid position, world space position, size, 
/// neighbours, etc... of a Hex Tile. However, it does NOT interact with
/// Unity directly in any way.
/// </summary>
public class Hex {

    public Hex(int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }

    // Q + R + S = 0
    // S = -(Q + R)

    public readonly int Q;  // Column
    public readonly int R;  // Row
    public readonly int S;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    float radius = 1f;
    bool allowWrapEastWest = true;
    bool allowWrapNorthSouth = false;

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

    public Vector3 PositionFromCamera( Vector3 cameraPosition, float numRows, float numColumns)
    {
        float mapHeight = numRows * HexVerticalSpacing();
        float mapWidth  = numColumns * HexHorizontalSpacing();

        Vector3 position = Position();

        if(allowWrapEastWest)
        {
            float howManyWidthsFromCamera = Mathf.round((position.x - cameraPosition.x) / mapWidth);
            int howManyWidthToFix = (int)howManyWidthsFromCamera;

            position.x -= howManyWidthToFix * mapWidth;
        }

        if(allowWrapNorthSouth)
        {
            float howManyHeightsFromCamera = Mathf.round((position.z - cameraPosition.z) / mapHeight);
            int howManyHeightsToFix = (int)howManyHeightsFromCamera;

            position.z -= howManyHeightsToFix * mapHeight;
        }


        return position;
    }
}
