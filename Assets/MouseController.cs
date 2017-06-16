using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        Update_CurrentFunc = Update_DetectModeStart;
    }

    // Generic bookkeeping variables
    Vector3 lastMousePosition;  // From Input.mousePosition

    // Camera Dragging bookkeeping variables
    int mouseDragThreshold = 1; // Threshold of mouse movement to start a drag
    Vector3 lastMouseGroundPlanePosition;
    Vector3 cameraTargetOffset;

    // Unit movement
    Unit selectedUnit = null;

    delegate void UpdateFunc();
    UpdateFunc Update_CurrentFunc;

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape) )
        {
            CancelUpdateFunc();
        }

        Update_CurrentFunc();

        // Always do camera zooms (check for being over a scroll UI later)
        Update_ScrollZoom();

        lastMousePosition = Input.mousePosition;
    }

    void CancelUpdateFunc()
    {
        Update_CurrentFunc = Update_DetectModeStart;

        // Also do cleanup of any UI stuff associated with modes.
    }

    void Update_DetectModeStart()
    {

        if (Input.GetMouseButtonDown (0)) 
        {
            // Left mouse button just went down.
            // This doesn't do anything by itself, really.
            Debug.Log("MOUSE DOWN");
        }
        else if( Input.GetMouseButtonUp(0) )
        {
            Debug.Log("MOUSE UP -- click!");

            // TODO: Are we clicking on a hex with a unit?
            //          If so, select it
        }
        else if( Input.GetMouseButton(0) && 
            Vector3.Distance( Input.mousePosition, lastMousePosition) > mouseDragThreshold )
        {
            // Left button is being held down AND the mouse moved? That's a camera drag!
            Update_CurrentFunc = Update_CameraDrag;
            lastMouseGroundPlanePosition = MouseToGroundPlane(Input.mousePosition);
            Update_CurrentFunc();
        }
        else if( selectedUnit != null && Input.GetMouseButton(1) )
        {
            // We have a selected unit, and we are holding down the mouse
            // button.  We are in unit movement mode -- show a path from
            // unit to mouse position via the pathfinding system.
        }

    }

    Vector3 MouseToGroundPlane(Vector3 mousePos)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay (mousePos);
        // What is the point at which the mouse ray intersects Y=0
        if (mouseRay.direction.y >= 0) {
            //Debug.LogError("Why is mouse pointing up?");
            return Vector3.zero;
        }
        float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
        return mouseRay.origin - (mouseRay.direction * rayLength);
    }

    void Update_UnitMovement ()
    {
        if( Input.GetMouseButtonUp(1) )
        {
            Debug.Log("Complete unit movement.");

            // TODO: copy pathfinding path to unit's movement queue

            CancelUpdateFunc();
            return;

        }
    }
    
    void Update_CameraDrag ()
    {
        if( Input.GetMouseButtonUp(0) )
        {
            Debug.Log("Cancelling camera drag.");
            CancelUpdateFunc();
            return;
        }
        
        // Right now, all we need are camera controls

        Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);

        Vector3 diff = lastMouseGroundPlanePosition - hitPos;
        Camera.main.transform.Translate (diff, Space.World);

        lastMouseGroundPlanePosition = hitPos = MouseToGroundPlane(Input.mousePosition);

            

    }

    void Update_ScrollZoom()
    {
        // Zoom to scrollwheel
        float scrollAmount = Input.GetAxis ("Mouse ScrollWheel");
        float minHeight = 2;
        float maxHeight = 20;
        // Move camera towards hitPos
        Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);
        Vector3 dir = hitPos - Camera.main.transform.position;

        Vector3 p = Camera.main.transform.position;

        // Stop zooming out at a certain distance.
        // TODO: Maybe you should still slide around at 20 zoom?
        if (scrollAmount > 0 || p.y < (maxHeight - 0.1f)) {
            cameraTargetOffset += dir * scrollAmount;
        }
        Vector3 lastCameraPosition = Camera.main.transform.position;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + cameraTargetOffset, Time.deltaTime * 5f);
        cameraTargetOffset -= Camera.main.transform.position - lastCameraPosition;


        p = Camera.main.transform.position;
        if (p.y < minHeight) {
            p.y = minHeight;
        }
        if (p.y > maxHeight) {
            p.y = maxHeight;
        }
        Camera.main.transform.position = p;

        // Change camera angle
        Camera.main.transform.rotation = Quaternion.Euler (
            Mathf.Lerp (30, 75, Camera.main.transform.position.y / maxHeight),
            Camera.main.transform.rotation.eulerAngles.y,
            Camera.main.transform.rotation.eulerAngles.z
        );


    }

}
