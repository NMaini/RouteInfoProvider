using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RouteInfoProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            string localConfigFile = @".\config\SourceMap.xml";

            try
            {
                if (System.IO.File.Exists(localConfigFile))
                {
                    System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                    xmlDocument.Load(localConfigFile);

                    System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("SourceMap/Map");

                    if (xmlNode.Attributes[0].Value != null)
                    {
                        //New object can be provided through a Dependency inject module.
                        ICreateGraph createGraph = new CreateGraph();
                        if (createGraph.CreateGraph(xmlNode.Attributes[0].Value))
                        {
                            DefineRoutes app = new DefineRoutes();
                            app.Start(createGraph);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Should have valid config. for Source Map");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading the config File, Exception: ", ex.Message);
            }
            Console.WriteLine("To exit the application press Any Key");
            Console.ReadKey();
        }
    }
}
