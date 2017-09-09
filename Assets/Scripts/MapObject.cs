using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;
using System.Linq;

public class MapObject
{
    public MapObject()
    {
    }

    public string Name;
    public int HitPoints = 100;
    public bool CanBeAttacked = true;
    public int FactionID = 0;

    public bool IsDestroyed { get; private set; }

    public Hex Hex  { get; protected set; }

    public delegate void ObjectMovedDelegate ( Hex oldHex, Hex newHex );
    public event ObjectMovedDelegate OnObjectMoved;

    public delegate void ObjectDestroyedDelegate ( MapObject mo );
    public event ObjectDestroyedDelegate OnObjectDestroyed;

    /// <summary>
    /// This object is being removed from the map/game
    /// </summary>
    virtual public void Destroy()
    {
        IsDestroyed = true;

        if(OnObjectDestroyed != null)
        {
            OnObjectDestroyed( this );
        }
    }

    virtual public void SetHex( Hex newHex )
    {
        Hex oldHex = Hex;

        Hex = newHex;

        if(OnObjectMoved != null)
        {
            OnObjectMoved(oldHex, newHex);
        }
    }

}


