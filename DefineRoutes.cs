using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteInfoProvider
{
    class DefineRoutes
    {
        public void Start(ICreateGraph createGraph)
        {
            if (createGraph !=null && createGraph.Graph != null)
            {
                //Finds the distance across a route.
                CalculateDistance(createGraph);

                //Finds the total routes between C and C
                TripsFromCtoC(createGraph);

                //Finds the total routes with A and C
                TripsFromAtoC(createGraph);

                //Finds the shortest path between A and C
                ShortestPath(createGraph, "A", "C");

                //Finds the shortest path between D and D
                ShortestPath(createGraph, "D", "D");
            }
        }

        private void ShortestPath(ICreateGraph graph, string start, string finish)
        {
            Dictionary<string, int> weightTable = new Dictionary<string, int>();

            Dictionary<string, int> startNode = new Dictionary<string, int>();

            if (graph.Graph.TryGetValue(start, out startNode))
            {
                foreach (var node in startNode)
                {
                    weightTable.Add(node.Key, node.Value);
                }
            }

            foreach (var node in graph.Graph)
            {
                if (!weightTable.ContainsKey(node.Key))
                {
                    weightTable.Add(node.Key, int.MaxValue);
                }
            }

            weightTable = weightTable.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            var list = weightTable.ToList();

            KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>();
            for (int index = 0; index < list.Count; index++)
            {
                keyValuePair = list[index];
                int parentWeight = keyValuePair.Value;

                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                if (graph.Graph.TryGetValue(keyValuePair.Key, out dictionary))
                {
                    foreach (var item in dictionary)
                    {
                        int weight = 0;
                        if (weightTable.TryGetValue(item.Key, out weight))
                        {
                            if (item.Value != int.MaxValue && parentWeight != int.MaxValue)
                            {
                                if (item.Value + parentWeight < weight)
                                {
                                    weightTable.Remove(item.Key);
                                    weightTable.Add(item.Key, item.Value + parentWeight);
                                    weightTable = weightTable.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                                    list = weightTable.ToList();
                                }
                            }
                        }
                    }
                }
            }

            int finalDistance = 0;
            if (weightTable.TryGetValue(finish, out finalDistance))
                Console.WriteLine("Path from " + start + " to " + finish + " is: " + finalDistance);
            else
                Console.WriteLine("NO SUCH ROUTE");
        }
        private void TripsFromAtoC(ICreateGraph graph)
        {
            int numberOfRoutes = 0;
            Dictionary<string, int> keyValues = new Dictionary<string, int>();
            Queue<string> queue = new Queue<string>();

            if (graph.Graph.TryGetValue("A", out keyValues))
            {
                foreach (var keyValue in keyValues)
                {
                    queue.Enqueue(keyValue.Key);
                }
            }

            int depthCounter = queue.Count;
            int depth = 0;
            while(queue.Count != 0)
            {
                var item = queue.Dequeue();
                if (depth != 3) 
                {
                    depthCounter -= 1;
                    if (depthCounter == 0)
                    {
                        depth += 1;
                    }
                    
                    if (graph.Graph.TryGetValue(item, out keyValues))
                    {
                        foreach (var keyValue in keyValues)
                        {
                            queue.Enqueue(keyValue.Key);
                        }
                    }
                    if (depthCounter == 0)
                        depthCounter = queue.Count;
                } 
                else
                {
                    if(item == "C")
                    {
                        numberOfRoutes += 1;
                    }    
                }
            }

            Console.WriteLine("Number of Routes from A to C are: " + numberOfRoutes);
        }
        private void TripsFromCtoC(ICreateGraph graph)
        {
            int numberOfRoutes = 0;
            Dictionary<string, int> keyValues = new Dictionary<string, int>();
            Stack<string> stack = new Stack<string>();

            if (graph.Graph.TryGetValue("C", out keyValues))
            {
                foreach(var keyValue in keyValues)
                {
                    stack.Push(keyValue.Key);
                }
            }

            int depth = 3;
            while(stack.Count != 0)
            {
                var item = stack.Pop();
                if (item == "C")
                {
                    depth = 3;
                    numberOfRoutes += 1;
                }
                else
                {
                    depth -= 1;
                    if (depth != 0)
                    {
                        if (graph.Graph.TryGetValue(item, out keyValues))
                        {
                            foreach (var keyValue in keyValues)
                            {
                                stack.Push(keyValue.Key);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Number of Routes from C to C are: " + numberOfRoutes);
        }
        private void CalculateDistance(ICreateGraph createGraph)
        {
            int routeDistance = CalculateRouteDistance("A-B-C", createGraph.Graph);
            if (routeDistance == 0)
            {
                Console.WriteLine("NO SUCH ROUTE");
            }
            else
            {
                Console.WriteLine("Distance from A-B-C is: " + routeDistance);
            }

            routeDistance = 0;
            routeDistance = CalculateRouteDistance("A-D", createGraph.Graph);
            if (routeDistance == 0)
            {
                Console.WriteLine("NO SUCH ROUTE");
            }
            else
            {
                Console.WriteLine("Distance from A-D is: " + routeDistance);
            }

            routeDistance = 0;
            routeDistance = CalculateRouteDistance("A-D-C", createGraph.Graph);
            if (routeDistance == 0)
            {
                Console.WriteLine("NO SUCH ROUTE");
            }
            else
            {
                Console.WriteLine("Distance from A-D-C is: " + routeDistance);
            }

            routeDistance = 0;
            routeDistance = CalculateRouteDistance("A-E-B-C-D", createGraph.Graph);
            if (routeDistance == 0)
            {
                Console.WriteLine("NO SUCH ROUTE");
            }
            else
            {
                Console.WriteLine("Distance from A-E-B-C-D is: " + routeDistance);
            }

            routeDistance = 0;
            routeDistance = CalculateRouteDistance("A-E-D", createGraph.Graph);
            if (routeDistance == 0)
            {
                Console.WriteLine("NO SUCH ROUTE");
            }
            else
            {
                Console.WriteLine("Distance from A-E-D is: " + routeDistance);
            }
        }
        private int CalculateRouteDistance(string route, Dictionary<string, Dictionary<string, int>> graph)
        {
            int routeDistance = 0;

            if (!String.IsNullOrEmpty(route))
            {
                string[] nodes = route.Trim().Split('-');

                if (nodes.Length > 1 && graph.Count > 0)
                {
                    // Loop until last but-one node.
                    for(int index = 0; index < nodes.Length - 1; index++)
                    {
                        var node = nodes[index];
                        if (graph.ContainsKey(node))
                        {
                            Dictionary<string, int> value = new Dictionary<string, int>();
                            if(graph.TryGetValue(node, out value))
                            {
                                if(value.ContainsKey(nodes[index + 1]))
                                {
                                    int distance = 0;
                                    value.TryGetValue(nodes[index + 1], out distance);
                                    routeDistance = routeDistance + distance;
                                }
                                else
                                {
                                    routeDistance = 0;
                                }
                            }
                            else
                            {
                                routeDistance = 0;
                            }
                        }
                    }
                }
            }

            return routeDistance;
        }
    }
}
