using System;
using System.Collections.Generic;

namespace QPath
{

    /// <summary>
    /// The graph's job is to keep a list of all neighbours leaving a tile
    /// </summary>

    public class QPath_TileGraph
    {

        Dictionary<IQPathTile, IQPathTile[]> neighbours;

        public QPath_TileGraph( IQPathWorld world )
        {
        }


    }
}

