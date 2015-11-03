using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using MoonSharp.Interpreter;

namespace Demiurg
{
    public class WorldCreator
    {
        Scribe scribe = Scribes.Find ("WorldCreator");
        Dictionary<string, CreationNode> nodes;

        public void InitWiring (Dictionary<string, Table> nodesTables, Dictionary<string, Type> nodesTypes)
        {
            nodes = CreateNodes (nodesTables, nodesTypes);
            InitNodes (nodes, nodesTables);
            WireNodes (nodes, nodesTables);
            foreach (var nodePair in nodes)
                nodePair.Value.StartIfReady ();
        }

        Dictionary<string, CreationNode> CreateNodes (Dictionary<string, Table> nodesTables, Dictionary<string, Type> nodesTypes)
        {
            Dictionary<string, CreationNode> nodes = new Dictionary<string, CreationNode> ();
            foreach (var pair in nodesTables)
            {
                string moduleTypeName = (string)pair.Value ["module_type"];
                if (moduleTypeName == null)
                    continue;
                Type moduleType = null;
                nodesTypes.TryGetValue (moduleTypeName, out moduleType);
                if (moduleType == null)
                    continue;
                CreationNode node = Activator.CreateInstance (moduleType) as CreationNode;
                nodes.Add (pair.Key, node);
            }
            return nodes;
        }

        void InitNodes (Dictionary<string, CreationNode> nodes, Dictionary<string, Table> entries)
        {
            foreach (var element in entries)
            {
                Table elementTable = element.Value as Table;
                CreationNode node = nodes [element.Key];
                node.PrepareNode ();
                node.Init (element.Key, elementTable ["params"] as Table);
            }
        }

        void WireNodes (Dictionary<string, CreationNode> nodes, Dictionary<string, Table> entries)
        {
            foreach (var element in entries)
            {
                Table inputs = element.Value ["inputs"] as Table;
                if (inputs == null)
                {
                    scribe.LogFormat ("No inputs specified for {0}", element.Key);
                    continue;
                }
                CreationNode node = nodes [element.Key];
                foreach (var input in inputs.Pairs)
                {
                    //node.GetInput(input.Key).ConnectTo()
                    NodeInput nodeInput = node.GetInput (input.Key.CastToString ());
                    if (nodeInput == null)
                    {
                        scribe.LogFormat ("Can't find input {1} in {0}", element.Key, input.Key);
                        continue;
                    }
                    CreationNode targetNode = null;
                    string targetNodeName = (string)input.Value.Table [1];
                    if (targetNodeName == null)
                    {
                        scribe.LogFormat ("Can't parse output reference as table (target node name) {1} in {0}", element.Key, input.Key);
                        continue;
                    }
                    nodes.TryGetValue (targetNodeName, out targetNode);
                    if (targetNode == null)
                    {
                        scribe.LogFormat ("Can't find node {0} to connect with {1}", targetNodeName, element.Key);
                        continue;
                    }
                    string outputName = (string)input.Value.Table [2];
                    if (outputName == null)
                    {
                        scribe.LogFormat ("Can't parse output reference as table (target output name) {1} in {0}", element.Key, input.Key);
                        continue;
                    }
                    Debug.LogFormat ("{0} {1} {2} {3}", node.Name, input.Key.CastToString (), targetNodeName, outputName);
                    nodeInput.ConnectTo (targetNode, outputName);

                }
            }
        }
    }
}
