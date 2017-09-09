using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnNumberText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        hexMap = GameObject.FindObjectOfType<HexMap>();
        text = GetComponent<Text>();
	}

    HexMap hexMap;
    Text text;
	
	// Update is called once per frame
	void Update () {
        text.text = "Turn: " + hexMap.TurnNumber;
	}
}
