using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Demiurg.Core.Extensions
{
    public class Metatable
    {
        Dictionary<string, ITable> tables = new Dictionary<string, ITable> ();

        public Metatable ()
        {

        }

        public Metatable (params ITable[] tables)
        {
            foreach (var table in tables)
            {
                if (this.tables.ContainsKey (table.Name))
                    continue;
                table.Set ("external", true);
                this.tables.Add (table.Name, table);
            }
                
        }

        public Metatable (List<ITable> tables)
        {
            foreach (var table in tables)
            {
                if (this.tables.ContainsKey (table.Name))
                    continue;
                table.Set ("external", true);
                this.tables.Add (table.Name, table);
            }
                
        }

        public void Add (ITable table)
        {
            if (this.tables.ContainsKey (table.Name))
                return;
            table.Set ("external", true);
            this.tables.Add (table.Name, table);
        }

        public ITable Get (string tableName)
        {
            ITable table = null;
            tables.TryGetValue (tableName, out table);
            return table;
        }

        public void Provide (string what, string to)
        {
            ITable whatTable = null;
            ITable toTable = null;
            tables.TryGetValue (what, out whatTable);
            tables.TryGetValue (to, out toTable);
            if (whatTable == null || toTable == null)
                return;
            toTable.Set (whatTable.Name, whatTable);
        }
    }
}


