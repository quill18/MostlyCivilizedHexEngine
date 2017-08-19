using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mouseController = GameObject.FindObjectOfType<MouseController>();
	}

    public Text Title;
    public Text Movement;
    public Text HexPath;

    public GameObject CityBuildButton;

    MouseController mouseController;
	
	// Update is called once per frame
	void Update () {
		
        if(mouseController.SelectedUnit != null)
        {

            Title.text    = mouseController.SelectedUnit.Name;

            Movement.text = string.Format(
                "{0}/{1}", 
                mouseController.SelectedUnit.MovementRemaining, 
                mouseController.SelectedUnit.Movement
            );

            Hex[] hexPath = mouseController.SelectedUnit.GetHexPath();
            HexPath.text  = hexPath == null ? "0" : hexPath.Length.ToString();


            if( mouseController.SelectedUnit.CanBuildCities && mouseController.SelectedUnit.Hex.City == null)
            {
                CityBuildButton.SetActive( true );
            }
            else
            {
                CityBuildButton.SetActive( false );
            }

        }

	}
}
