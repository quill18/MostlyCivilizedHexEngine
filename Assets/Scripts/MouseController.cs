using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        Update_CurrentFunc = Update_DetectModeStart;

        hexMap = GameObject.FindObjectOfType<HexMap>();

        lineRenderer = transform.GetComponentInChildren<LineRenderer>();
    }

    public GameObject UnitSelectionPanel;

    // Generic bookkeeping variables
    HexMap hexMap;
    Hex hexUnderMouse;
    Hex hexLastUnderMouse;
    Vector3 lastMousePosition;  // From Input.mousePosition

    // Camera Dragging bookkeeping variables
    int mouseDragThreshold = 1; // Threshold of mouse movement to start a drag
    Vector3 lastMouseGroundPlanePosition;
    Vector3 cameraTargetOffset;

    // Unit movement
    Unit __selectedUnit = null;
    public Unit SelectedUnit {
        get { return __selectedUnit; }   
        set {
            __selectedUnit = value;
            UnitSelectionPanel.SetActive( __selectedUnit != null );
        }
    }


    Hex[] hexPath;
    LineRenderer lineRenderer;

    delegate void UpdateFunc();
    UpdateFunc Update_CurrentFunc;

    public LayerMask LayerIDForHexTiles;

    void Update()
    {
        hexUnderMouse = MouseToHex();

        if( Input.GetKeyDown(KeyCode.Escape) )
        {
            SelectedUnit = null;
            CancelUpdateFunc();
        }

        Update_CurrentFunc();

        // Always do camera zooms (check for being over a scroll UI later)
        Update_ScrollZoom();

        lastMousePosition = Input.mousePosition;
        hexLastUnderMouse = hexUnderMouse;

        if(SelectedUnit != null)
        {
            DrawPath( ( hexPath != null ) ? hexPath : SelectedUnit.GetHexPath() );
        }
        else
        {
            DrawPath( null );   // Clear the path display
        }            
    }

    void DrawPath(Hex[] hexPath)
    {
        if(hexPath == null || hexPath.Length == 0)
        {
            lineRenderer.enabled = false;
            return;
        }
        lineRenderer.enabled = true;

        Vector3[] ps = new Vector3[ hexPath.Length ];

        for (int i = 0; i < hexPath.Length; i++)
        {
            GameObject hexGO = hexMap.GetHexGO(hexPath[i]);
            ps[i] = hexGO.transform.position + (Vector3.up*0.1f);
        }

        lineRenderer.positionCount = ps.Length;
        lineRenderer.SetPositions(ps);
    }
        

    void CancelUpdateFunc()
    {
        Update_CurrentFunc = Update_DetectModeStart;

        // Also do cleanup of any UI stuff associated with modes.
        hexPath = null;
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

            Unit[] us = hexUnderMouse.Units();

            // TODO: Implement cycling through multiple units in the same tile

            if(us.Length > 0)
            {
                SelectedUnit = us[0];

                // NOTE: Selecting a unit does NOT change our mouse mode

                //Update_CurrentFunc = Update_UnitMovement;
            }

        }
        else if ( SelectedUnit != null && Input.GetMouseButtonDown(1) )
        {
            // We have a selected unit, and we've pushed down the right
            // mouse button, so enter unit movement mode.
            Update_CurrentFunc = Update_UnitMovement;

        }
        else if( Input.GetMouseButton(0) && 
            Vector3.Distance( Input.mousePosition, lastMousePosition) > mouseDragThreshold )
        {
            // Left button is being held down AND the mouse moved? That's a camera drag!
            Update_CurrentFunc = Update_CameraDrag;
            lastMouseGroundPlanePosition = MouseToGroundPlane(Input.mousePosition);
            Update_CurrentFunc();
        }
        else if( SelectedUnit != null && Input.GetMouseButton(1) )
        {
            // We have a selected unit, and we are holding down the mouse
            // button.  We are in unit movement mode -- show a path from
            // unit to mouse position via the pathfinding system.
        }

    }

    Hex MouseToHex()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hitInfo;

        int layerMask = LayerIDForHexTiles.value;

        if( Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, layerMask) )
        {
            // Something got hit
            //Debug.Log( hitInfo.collider.name );

            // The collider is a child of the "correct" game object that we want.
            GameObject hexGO = hitInfo.rigidbody.gameObject;

            return hexMap.GetHexFromGameObject(hexGO);
        }

        Debug.Log("Found nothing.");
        return null;
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
        if( Input.GetMouseButtonUp(1) || SelectedUnit == null )
        {
            Debug.Log("Complete unit movement.");

            if(SelectedUnit != null)
            {
                SelectedUnit.SetHexPath(hexPath);
            }

            CancelUpdateFunc();
            return;
        }

        // We have a selected unit

        // Look at the hex under our mouse

        // Is this a different hex than before (or we don't already have a path)
        if( hexPath == null || hexUnderMouse != hexLastUnderMouse )
        {
            // Do a pathfinding search to that hex
            hexPath = QPath.QPath.FindPath<Hex>( hexMap, SelectedUnit, SelectedUnit.Hex, hexUnderMouse, Hex.CostEstimate );
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
