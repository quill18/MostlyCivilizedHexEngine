using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FitGridToChildren : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        grid = GetComponent<GridLayoutGroup>();
	}

    GridLayoutGroup grid;
	
	// Update is called once per frame
	void Update () 
    {
        // TODO: Instead of every frame, check for changes in size
        // or be triggered by something?
        ResizeMe();
	}

    void ResizeMe()
    {
        int numChildren = this.transform.childCount;

        if(numChildren == 0)
        {
            SetHeight(0);
            return;
        }

        float childHeight = 
            this.transform.GetChild(0).GetComponent<RectTransform>().rect.height;

        // We need to add the vertical spacing to the child height

        childHeight += grid.spacing.y;

        float totalChildHeight = childHeight * numChildren;

        // We need to account for padding!

        totalChildHeight += grid.padding.top + grid.padding.bottom;

        // Remove the extra grid spacing
        totalChildHeight -= grid.spacing.y;

        SetHeight( totalChildHeight );
    }

    void SetHeight( float h )
    {
        Vector2 v = GetComponent<RectTransform>().sizeDelta;

        v.y = h;

        GetComponent<RectTransform>().sizeDelta = v;
    }
}
