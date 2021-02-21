using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteInfoProvider
{
    class CreateGraph : ICreateGraph
    {
        private Dictionary<string, Dictionary<string, int>> graph;

        public CreateGraph()
        {
            this.graph = new Dictionary<string, Dictionary<string, int>>();
        }
     
        public Dictionary<string, Dictionary<string, int>> Graph
        {
            get
            {
                return this.graph;
            }
        }

        bool ICreateGraph.CreateGraph(string sourceMap)
        {
            bool graphCreated = true;

            try
            {
                if (!String.IsNullOrWhiteSpace(sourceMap))
                {
                    if (sourceMap.Trim() != String.Empty)
                    {
                        string[] routes = sourceMap.Split(',');
                        if (routes.Length >= 1)
                        {
                            foreach (var route in routes)
                            {
                                char[] array = route.Trim().ToCharArray();
                                if (Graph.ContainsKey(array[0].ToString()))
                                {
                                    Dictionary<string, int> value = new Dictionary<string, int>();
                                    if (Graph.TryGetValue(array[0].ToString(), out value))
                                    {
                                        value.Add(array[1].ToString(), int.Parse(array[2].ToString()));
                                    }
                                }
                                else
                                {
                                    Dictionary<string, int> valuePairs = new Dictionary<string, int>();
                                    valuePairs.Add(array[1].ToString(), int.Parse(array[2].ToString()));
                                    Graph.Add(array[0].ToString(), valuePairs);
                                }
                            }
                        }
                    }
                }
            } 
            catch(Exception ex)
            {
                graphCreated = false;
                Console.WriteLine("Failed Creating the Graph. Exception details: ", ex.Message);
            }

            return graphCreated;
        }
    }
}
