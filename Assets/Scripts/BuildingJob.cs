using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingJob
{
    public BuildingJob( Image BuildingJobIcon, 
        string BuildingJobName, 
        float totalProductionNeeded,
        float overflowedProduction,
        ProductionCompleteDelegate OnProductionComplete,
        ProductionBonusDelegate ProductionBonusFunc
    )
    {
        if(OnProductionComplete == null)
            throw new UnityException();

        this.BuildingJobIcon = BuildingJobIcon;
        this.BuildingJobName = BuildingJobName;
        this.totalProductionNeeded = totalProductionNeeded;
        productionDone = overflowedProduction;
        this.OnProductionComplete = OnProductionComplete;
        this.ProductionBonusFunc = ProductionBonusFunc;
    }

    public float totalProductionNeeded;
    public float productionDone;

    public Image BuildingJobIcon;   // Ex: Image for the Petra
    public string BuildingJobName;  // Ex:  "Petra"

    public delegate void ProductionCompleteDelegate(  );
    public event ProductionCompleteDelegate OnProductionComplete;

    public delegate float ProductionBonusDelegate();
    public  ProductionBonusDelegate ProductionBonusFunc;

    /// <summary>
    /// Dos the work.
    /// </summary>
    /// <returns>Number of hammers remaining, or negative is complete/overflow</returns>
    /// <param name="rawProduction">Raw production.</param>
    public float DoWork( float rawProduction )
    {
        if(ProductionBonusFunc != null)
        {
            rawProduction *= ProductionBonusFunc();
        }

        productionDone += rawProduction;

        if(productionDone >= totalProductionNeeded)
        {
            OnProductionComplete();
        }

        return totalProductionNeeded - productionDone;
    }

}

