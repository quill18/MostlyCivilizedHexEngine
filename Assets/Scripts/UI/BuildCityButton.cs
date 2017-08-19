using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCityButton : MonoBehaviour {

    public void BuildCity()
    {
        City city = new City();

        HexMap map = GameObject.FindObjectOfType<HexMap>();
        MouseController mc = GameObject.FindObjectOfType<MouseController>();

        map.SpawnCityAt(
            city,
            map.CityPrefab,
            mc.SelectedUnit.Hex.Q,
            mc.SelectedUnit.Hex.R
        );
    }
	
}
