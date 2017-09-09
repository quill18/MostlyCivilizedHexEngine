using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CityNameplate : MonoBehaviour, IPointerClickHandler {

    public City MyCity;

    public void OnPointerClick(PointerEventData eventData)
    {
        //MapObjectNamePlate monp = GetComponent<MapObjectNamePlate>();

        GameObject.FindObjectOfType<SelectionController>().SelectedCity = MyCity;
    }
}
