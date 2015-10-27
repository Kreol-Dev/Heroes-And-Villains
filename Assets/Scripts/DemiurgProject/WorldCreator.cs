using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;
using System;
using System.Reflection;


namespace Demiurg
{
	public class WorldCreator
	{
		Dictionary<string, CreationNode> nodes;
		public void InitWiring(Table wiring, Dictionary<string, Type> nodeTypes)
		{
			Dictionary<string, Entry> entries = wiring.GetNamedEntries();
			nodes = CreateNodes(entries, nodeTypes);
			InitNodes(nodes, entries);
			WireNodes(nodes, entries);
		}

		Dictionary<string, CreationNode> CreateNodes (Dictionary<string, Entry> entries, Dictionary<string, Type> nodeTypes)
		{
			Dictionary<string, CreationNode> nodes = new Dictionary<string, CreationNode>();
			foreach ( var element in entries)
			{
				Table elementTable = element.Value as Table;
				if (elementTable == null)
					continue;
				string moduleName = elementTable.Get<LuaString>("module_type");
				if (moduleName == null)
					continue;
				Type moduleType = null;
				nodeTypes.TryGetValue(moduleName, out moduleType);
				if (moduleType == null)
					continue;
				CreationNode node = Activator.CreateInstance(moduleType) as CreationNode;
				nodes.Add(element.Key, node);
			}
			return nodes;
		}

		void InitNodes (Dictionary<string, CreationNode> nodes, Dictionary<string, Entry> entries)
		{
			foreach ( var element in entries)
			{
				Table elementTable = element.Value as Table;
				CreationNode node = nodes[element.Key];
				node.Init(element.Key, elementTable.Get<Table>("params"));
			}
		}

		void WireNodes (Dictionary<string, CreationNode> nodes, Dictionary<string, Entry> entries)
		{

		}
	}
}
