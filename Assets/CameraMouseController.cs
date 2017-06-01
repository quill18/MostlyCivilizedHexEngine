using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseController : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        
    }

    bool isDraggingCamera = false;
    Vector3 lastMousePosition;
    Vector3 cameraTargetOffset;
    
    // Update is called once per frame
    void Update ()
    {
        
        // Right now, all we need are camera controls

        Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        // What is the point at which the mouse ray intersects Y=0
        if (mouseRay.direction.y >= 0) {
            //Debug.LogError("Why is mouse pointing up?");
            return;
        }
        float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
        Vector3 hitPos = mouseRay.origin - (mouseRay.direction * rayLength);

        if (Input.GetMouseButtonDown (0)) {
            // Mouse button just went down -- start a drag.
            isDraggingCamera = true;

            lastMousePosition = hitPos;
        } else if (Input.GetMouseButtonUp (0)) {
            // Mouse button went up, stop drag
            isDraggingCamera = false;
        }

        if (isDraggingCamera) {
            Vector3 diff = lastMousePosition - hitPos;
            Camera.main.transform.Translate (diff, Space.World);
            mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
            // What is the point at which the mouse ray intersects Y=0
            if (mouseRay.direction.y >= 0) {
                Debug.LogError ("Why is mouse pointing up?");
                return;
            }
            rayLength = (mouseRay.origin.y / mouseRay.direction.y);
            lastMousePosition = hitPos = mouseRay.origin - (mouseRay.direction * rayLength);
        }

        // Zoom to scrollwheel
        float scrollAmount = Input.GetAxis ("Mouse ScrollWheel");
        float minHeight = 2;
        float maxHeight = 20;
        // Move camera towards hitPos
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
        float lowZoom = minHeight + 3;
        float highZoom = maxHeight - 10;

        Camera.main.transform.rotation = Quaternion.Euler (
            Mathf.Lerp (30, 75, Camera.main.transform.position.y / maxHeight),
            Camera.main.transform.rotation.eulerAngles.y,
            Camera.main.transform.rotation.eulerAngles.z
        );

            

    }

}
