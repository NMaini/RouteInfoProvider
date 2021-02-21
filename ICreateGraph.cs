using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteInfoProvider
{
    interface ICreateGraph
    {
        bool CreateGraph(string sourceMap);

        Dictionary<string, Dictionary<string, int>> Graph
        {
            get;
        }
    }
}
