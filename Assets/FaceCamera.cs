using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if(TheCamera == null)
            TheCamera = Camera.main;
	}

    public Camera TheCamera;
	
	// Update is called once per frame
	void Update () {
        transform.rotation = TheCamera.transform.rotation;
	}
}
