using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit {

    public string Name = "Dwarf";
    public int HitPoints = 100;
    public int Strenth = 8;
    public int Movement = 2;
    public int MovementRemaining = 2;

    public Hex Hex  { get; protected set; }

    public delegate void UnitMovedDelegate ( Hex oldHex, Hex newHex );
    public event UnitMovedDelegate OnUnitMoved;

    public void SetHex( Hex newHex )
    {
        Hex oldHex = Hex;
        if(Hex != null)
        {
            Hex.RemoveUnit(this);
        }

        Hex = newHex;

        Hex.AddUnit(this);

        if(OnUnitMoved != null)
        {
            OnUnitMoved(oldHex, newHex);
        }
    }

    public void DoTurn()
    {
        Debug.Log("DoTurn");
        // Do queued move?

        // TESTING:  Move us one tile to the right

        Hex oldHex = Hex;
        Hex newHex = oldHex.HexMap.GetHexAt(oldHex.Q + 1, oldHex.R);

        SetHex( newHex );
    }

}
