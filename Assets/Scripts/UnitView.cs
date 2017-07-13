using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour {

    void Start()
    {
        newPosition = this.transform.position;
    }

    Vector3 newPosition;

    Vector3 currentVelocity;
    float smoothTime = 0.5f;

    public void OnUnitMoved( Hex oldHex, Hex newHex )
    {
        // This GameObject is supposed to be a child of the hex we are
        // standing in. This ensures that we are in the correct place
        // in the hierachy
        // Our correct position when we aren't moving, is to be at
        // 0,0 local position relative to our parent.

        this.transform.position = oldHex.PositionFromCamera();
        newPosition = newHex.PositionFromCamera();
        currentVelocity = Vector3.zero;

        if( Vector3.Distance(this.transform.position, newPosition) > 2 )
        {
            // This OnUnitMoved is considerably more than the expected move
            // between two adjacent tiles -- it's probably a map seam thing,
            // so just teleport
            this.transform.position = newPosition;
        }
    }

    void Update()
    {

        this.transform.position = Vector3.SmoothDamp( this.transform.position, newPosition, ref currentVelocity, smoothTime );

    }

}
