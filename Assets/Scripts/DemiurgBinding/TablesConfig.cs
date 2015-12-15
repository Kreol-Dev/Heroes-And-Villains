using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;
using MoonSharp.Interpreter;
using Signals;

namespace DemiurgBinding
{
    public class TablesConfig
    {
        Dictionary<string, ConfigEntry> entries = new Dictionary<string, ConfigEntry> ();

        class ConfigEntry
        {
            public string TableName { get; internal set; }

            public Signal FinishedLoading { get; internal set; }

            public List<string> Dependencies { get; internal set; }

            public List<string> Paths { get; internal set; }

            public ConfigEntry (string tableName)
            {
                TableName = tableName;
                FinishedLoading = new Signal ();
                Paths = new List<string> ();
                Dependencies = new List<string> ();
            }

            public delegate void VoidDelegate (string tableName, List<string> paths);

            public event VoidDelegate LoadSelf;

            int deps = 0;

            public void SatisfiedDependency ()
            {
                deps++;
                if (deps == Dependencies.Count)
                    LoadSelf (TableName, Paths);
            }
        }

        Metatable metatable = new Metatable ();
        Script script;

        public TablesConfig (Script script)
        {
            this.script = script;
        }

        public BindingTable ConfigureTable (string tableName, params string[] dependencies)
        {
            ITable table = metatable.Get (tableName);
            ConfigEntry entry = null;
            if (table == null)
            {
                Debug.LogWarningFormat ("Create new table {0}", tableName);
                script.Globals [tableName] = new Table (script);
                table = new BindingTable (script.Globals [tableName] as Table);
                table.Name = tableName;
                metatable.Add (table);
                entry = new ConfigEntry (tableName);
                entries.Add (tableName, entry);
                entry.LoadSelf += LoadTable;
            }

            entry = entries [tableName];
            foreach (var dep in dependencies)
            {
                entry.Dependencies.Add (dep);
                metatable.Provide (dep, tableName);
            }
            return table as BindingTable;
        }

        public void AddPathsToTable (string tableName, params string[] paths)
        {
            ConfigEntry entry = null;
            entries.TryGetValue (tableName, out entry);
            if (entry == null)
                return;
            entry.Paths.AddRange (paths);
        }

        public void Load ()
        {
            DetermineOrder ();
        }

        void LoadTable (string tableName, List<string> paths)
        {
            BindingTable table = metatable.Get (tableName) as BindingTable;
            foreach (var path in paths)
            {
                script.DoFile (path, table.Table);
            }
        }

        List<ConfigEntry> DetermineOrder ()
        {
            List<ConfigEntry> firstEntries = new List<ConfigEntry> ();
            foreach (var entryNode in entries)
            {
                if (entryNode.Value.Dependencies.Count == 0)
                {
                    firstEntries.Add (entryNode.Value);
                    continue;
                }
                else
                {
                    foreach (var dep in entryNode.Value.Dependencies)
                    {
                        ConfigEntry dependency = null;
                        entries.TryGetValue (dep, out dependency);
                        if (dependency == null)
                            continue;
                        dependency.FinishedLoading.AddOnce (entryNode.Value.SatisfiedDependency);
                    }
                }
            }

            return firstEntries;
        }

        public BindingTable GetTable (string table)
        {
            return metatable.Get (table) as BindingTable;
        }
        
        
        
    }
}


