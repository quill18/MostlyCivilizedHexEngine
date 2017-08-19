using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObjectNamePlate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if(TheCamera == null)
            TheCamera = Camera.main;

        rectTransform = GetComponent<RectTransform>();
	}

    public GameObject MyTarget;
    public Vector3 WorldPositionOffset = new Vector3(0, 1, 0);
    public Vector3 ScreenPositionOffset = new Vector3(0, 30, 0);

    public Camera TheCamera;

    RectTransform rectTransform;
	
	// Update is called once per frame
	void LateUpdate () {
        if(MyTarget == null)
        {
            // The object we're supposed to track has been removed, so let's destroy ourselves.
            Destroy(gameObject);
            return;
        }
		
        // Find out the screen position of our object and set ourselves to that, plus offset
        Vector3 screenPos = TheCamera.WorldToScreenPoint( MyTarget.transform.position + WorldPositionOffset );

        rectTransform.anchoredPosition = screenPos + ScreenPositionOffset;
	}
}
