using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexComponent : MonoBehaviour {

    void Start()
    {
        hexMap = GameObject.FindObjectOfType<HexMap>();
    }

    public Hex Hex;
    HexMap hexMap;

	void Update () {
		
        if(Hex == null)
        {
            return;
        }

        this.transform.position = Hex.PositionFromCamera(
            Camera.main.transform.position,
            hexMap.NumRows,
            hexMap.NumColumns
        );
	}
}
