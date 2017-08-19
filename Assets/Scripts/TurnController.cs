using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        hexMap = GameObject.FindObjectOfType<HexMap>();
	}

    HexMap hexMap;

    public void EndTurnButton()
    {
        Debug.Log("EndTurn");
        Unit[] units = hexMap.Units;
        City[] cities = hexMap.Cities;
        // First check to see if there are any units that have enqueued moves.
        // Do those moves
        // Now are any units waiting for orders? If so, halt EndTurn()

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


    }
}
