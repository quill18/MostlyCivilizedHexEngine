using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityNameplateController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.FindObjectOfType<HexMap>().OnCityCreated += CreateCityNameplate;
	}

	// Update is called once per frame
	void Update () {
		
	}

    public GameObject CityNameplatePrefab;

    public void CreateCityNameplate( City city, GameObject cityGO )
    {
        GameObject nameGO = (GameObject)Instantiate(CityNameplatePrefab, this.transform);
        nameGO.GetComponent<MapObjectNamePlate>().MyTarget = cityGO;
        nameGO.GetComponentInChildren<CityNameplate>().MyCity = city;

    }
}
