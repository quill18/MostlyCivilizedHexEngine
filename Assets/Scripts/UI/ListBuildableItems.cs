using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListBuildableItems : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // TODO: Get list of buildings specific to this CITY

        BuildingBlueprint[] blueprintsForThisCity = /*thisCity.GetPossibleBuildings()*/
            BuildingDatabase.GetListOfBuilding();

        PopulateBuildables( blueprintsForThisCity );
	}

    public GameObject BuildableItemPrefab;

    public void PopulateBuildables(  BuildingBlueprint[] buildings )
    {
        // We are going to be passed a list of valid buildables
        // for this city (based on technology, completed wonders,
        // available resource, and stuff the city doesn't have yet.)


        for (int i = 0; i < buildings.Length; i++)
        {
            GameObject go = (GameObject)Instantiate( BuildableItemPrefab, this.transform );

            go.GetComponentInChildren<Text>().text = buildings[i].Name;


            // IF you get weird scaling, do this after instantiating:
            // theSpawnedGameObject.transform.localScale = Vector3.one
        }

    }
}
