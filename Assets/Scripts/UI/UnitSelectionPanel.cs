using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        selectionController = GameObject.FindObjectOfType<SelectionController>();
	}

    public Text Title;
    public Text Movement;
    public Text HexPath;

    public GameObject CityBuildButton;

    SelectionController selectionController;
    	
	// Update is called once per frame
	void Update () {
		
        if(selectionController.SelectedUnit != null)
        {

            Title.text    = selectionController.SelectedUnit.Name;

            Movement.text = string.Format(
                "{0}/{1}", 
                selectionController.SelectedUnit.MovementRemaining, 
                selectionController.SelectedUnit.Movement
            );

            Hex[] hexPath = selectionController.SelectedUnit.GetHexPath();
            HexPath.text  = hexPath == null ? "0" : hexPath.Length.ToString();


            if( selectionController.SelectedUnit.CanBuildCities && selectionController.SelectedUnit.Hex.City == null)
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
