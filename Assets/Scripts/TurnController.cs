using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        hexMap = GameObject.FindObjectOfType<HexMap>();
        selectionController = GameObject.FindObjectOfType<SelectionController>();
	}

    HexMap hexMap;
    SelectionController selectionController;

    public GameObject EndTurnButton;
    public GameObject NextUnitButton;

    public void Update()
    {
        // Is the current player an AI?
        // If so, instruct AI to do its move.
        if(hexMap.CurrentPlayer.Type == Player.PlayerType.AI)
        {
            // Call AI logic function whatever here.
            hexMap.AdvanceToNextPlayer();
            return;
        }

        // Which button should be visible in the bottom-right?
        // Are any units waiting for commands?
        Unit[] units = hexMap.CurrentPlayer.Units;
        EndTurnButton.SetActive(true);
        NextUnitButton.SetActive(false);
        foreach(Unit u in units)
        {
            if(u.UnitWaitingForOrders())
            {
                EndTurnButton.SetActive(false);
                NextUnitButton.SetActive(true);
                break;
            }
        }

        // Is a city waiting with an empty production queue?
    }

    public void EndTurn()
    {
        Debug.Log("EndTurn");
        Unit[] units = hexMap.CurrentPlayer.Units;
        City[] cities = hexMap.CurrentPlayer.Cities;

        // First check to see if there are any units that have enqueued moves.
        foreach(Unit u in units)
        {
            // Do those moves
            while( u.DoMove() )
            {
                // TODO: WAIT FOR ANIMATION TO COMPLETE!
            }
        }

        // Now are any units waiting for orders? If so, halt EndTurn()
        foreach(Unit u in units)
        {
            if(u.UnitWaitingForOrders())
            {
                // Select the unit
                selectionController.SelectedUnit = u;

                // Stop processing the end turn
                return;
            }
        }

        // Heal units that are resting

        // Reset unit movement
        foreach(Unit u in units)
        {
            u.RefreshMovement();
        }

        // If we get to this point, no units are waiting for orders, so process cities

        foreach(City c in cities)
        {
            c.DoTurn();
        }


        // Go to next player
        selectionController.SelectedUnit = null;
        selectionController.SelectedCity = null;
        hexMap.AdvanceToNextPlayer();

    }
}
