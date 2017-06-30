using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPath
{
    /// <summary>
    /// 
    ///   Tile[] ourPath = QPath.FindPath( ourWorld, theUnit, startTile, endTile );
    /// 
    ///   theUnit is a object that is the thing actually trying to path between
    ///   tiles.  It might have special logic based on its movement type and the
    ///   type of tiles being moved through
    /// 
    ///   Our tiles need to be able to return the following information:
    ///     1)  List of neighbours
    ///     2)  The aggregate cost to enter this tile from another tile
    /// 
    /// 
    /// 
    /// 
    /// </summary>


    public static class QPath {

        public static T[] FindPath<T>( 
            IQPathWorld world, 
            IQPathUnit unit, 
            T startTile, 
            T endTile,
            CostEstimateDelegate costEstimateFunc
            ) where T : IQPathTile
        {
            Debug.Log("QPath::FindPath");
            if( world == null || unit == null || startTile == null || endTile == null )
            {
                Debug.LogError("null values passed to QPath::FindPath");
                return null;
            }

            // Call on our actual path solver


            QPath_AStar<T> resolver = new QPath_AStar<T>( world, unit, startTile, endTile, costEstimateFunc );

            resolver.DoWork();

            return resolver.GetList();
        }
    }

    public delegate float CostEstimateDelegate(IQPathTile a, IQPathTile b);
}