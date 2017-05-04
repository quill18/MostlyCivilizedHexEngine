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
        // TODO:  Allow have the camera do some kind of reset / rollback
        //          if it gets too far from the origin.  That will help
        //          optimize this relatively poor code, plus prevent
        //          floating point issues.

        float mapHeight = numRows * HexVerticalSpacing();
        float mapWidth  = numColumns * HexHorizontalSpacing();

        // First get the "base" position 
        Vector3 position = Position();

        // Now offset based on where the camera is
        //   i.e. How far from the camera is our tile?
        position.x -= cameraPosition.x;
        position.z -= cameraPosition.z;

        // Are we more than mapWidth/2 away from the camera?
        //   FIXME: This is a bit (a lot) brute-forceish
        if(allowWrapEastWest)
        {
            while(position.x < -mapWidth/2) {
                position.x += mapWidth;
            }
            while(position.x >= mapWidth/2) {
                position.x -= mapWidth;
            }
        }
        if(allowWrapNorthSouth)
        {
            while(position.z < -mapHeight/2) {
                position.z += mapHeight;
            }
            while(position.z >= mapHeight/2) {
                position.z -= mapHeight;
            }
        }

        // De-offset the camera position
        position.x += cameraPosition.x;
        position.z += cameraPosition.z;

        return position;
    }
}