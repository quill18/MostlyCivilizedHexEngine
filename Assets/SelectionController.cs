using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mouseController = GameObject.FindObjectOfType<MouseController>();
        hexMap = GameObject.FindObjectOfType<HexMap>();
	}

    public GameObject UnitSelectionPanel;
    public GameObject CitySelectionPanel;
    public GameObject SelectionIndicator;
    HexMap hexMap;

    // Unit selection
    Unit __selectedUnit = null;
    public Unit SelectedUnit {
        get { return __selectedUnit; }   
        set {
            __selectedUnit = null;
            if(__selectedCity != null)
                SelectedCity = null;

            __selectedUnit = value;
            UnitSelectionPanel.SetActive( __selectedUnit != null );
            UpdateSelectionIndicator();
        }
    }

    City __selectedCity = null;
    public City SelectedCity {
        get { return __selectedCity; }   
        set {
            if(__selectedCity != null)
            {
                // We already have a city selected, make sure we cancel the old mouse mode
                mouseController.CancelUpdateFunc();

            }

            __selectedCity = null;
            if(__selectedUnit != null)
                SelectedUnit = null;

            __selectedCity = value;
            CitySelectionPanel.SetActive( __selectedCity != null );
            if(__selectedCity != null)
            {
                mouseController.StartCityView();
            }
        }
    }

    MouseController mouseController;

	
	// Update is called once per frame
	void Update () {

        // De-select things have have been destroyed
        if(SelectedUnit != null && SelectedUnit.IsDestroyed == true)
        {
            // We are pointing to a destroyed unit.
            SelectedUnit = null;
        }

        if(SelectedCity != null && SelectedCity.IsDestroyed == true)
        {
            // We are pointing to a destroyed unit.
            SelectedCity = null;
        }

        UpdateSelectionIndicator();

	}

    void UpdateSelectionIndicator()
    {
        if(SelectedUnit == null)
        {
            SelectionIndicator.SetActive(false);
            return;
        }

        GameObject uGO = hexMap.GetUnitGO( SelectedUnit );
        if(uGO == null)
        {
            SelectedUnit = null;
            return;
        }

        SelectionIndicator.SetActive(true);
        SelectionIndicator.transform.position = uGO.transform.position;
    }

    public void SelectNextUnit( bool skipDoneUnits )
    {
        Player player = hexMap.CurrentPlayer;

        Unit[] units = player.Units;

        int currentIndex = 0;

        if(SelectedUnit != null)
        {
            for (int i = 0; i < units.Length; i++)
            {
                if(SelectedUnit == units[i])
                {
                    currentIndex = i;
                    break;
                }
            }
        }

        for (int i = 0; i < units.Length; i++)
        {
            int tryIndex = (currentIndex + i + 1) % units.Length;

            if ( skipDoneUnits == true && units[tryIndex].UnitWaitingForOrders() == false )
            {
                // Skip this unit
                continue;
            }

            // We only get here if we're on a valid pick
            SelectedUnit = units[ tryIndex ];
            return;
        }

        // if we got here, selection did not change
        // If the pre-existing unit is done, and we're suppposed to skip that,
        // then clear the selection.
        if(SelectedUnit.UnitWaitingForOrders() == false && skipDoneUnits == true)
        {
            SelectedUnit = null;
        }

    }
}
