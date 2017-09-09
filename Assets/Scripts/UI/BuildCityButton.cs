using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCityButton : MonoBehaviour {

    public void BuildCity()
    {
        City city = new City();

        HexMap map = GameObject.FindObjectOfType<HexMap>();
        SelectionController sc = GameObject.FindObjectOfType<SelectionController>();

        map.SpawnCityAt(
            city,
            map.CityPrefab,
            sc.SelectedUnit.Hex.Q,
            sc.SelectedUnit.Hex.R
        );
    }
	
}
