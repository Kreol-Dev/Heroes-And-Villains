using UnityEngine;
using System.Collections;
using Demiurg.Core;
using System.Collections.Generic;
using Demiurg.Core.Extensions;

namespace CoreMod
{
    public class ReplacersCollectionModule : Demiurg.Core.Avatar
    {
        [AConfig ("replacers_table")]
        string replacersTableName;
        [AOutput ("replacers")]
        Dictionary<string, List<GameObject>> replacers;

        int id = 0;
        Transform replacersFolder;
        ObjectsCreator creator;

        public override void Work ()
        {
            creator = Find.Root<ObjectsCreator> ();
            replacersFolder = (new GameObject ("Replacers")).transform;
            ITable replacersTable = Find.Root<ModsManager> ().GetTable (replacersTableName);
            replacers = new Dictionary<string, List<GameObject>> ();
            foreach (var key in replacersTable.GetKeys())
            {
                ITable namespaceTable = replacersTable.Get (key) as ITable;
                if (namespaceTable == null || namespaceTable.Get ("global") != null)
                    continue;
                string strKey = key as string;
                if (strKey == null)
                    continue;
                replacers.Add (strKey, GetGOs (strKey, namespaceTable));

            }
            FinishWork ();
        }

        List<GameObject> GetGOs (string namespaceName, ITable table)
        {
            List<GameObject> replacerGOs = new List<GameObject> ();
            Transform namespaceFolder = (new GameObject (namespaceName)).transform;
            namespaceFolder.SetParent (replacersFolder);
            foreach (var repKey in table.GetKeys())
            {
                
                ITable repTable = table.Get (repKey) as ITable;
                GameObject go = creator.CreateObject (repKey as string, repTable);
                go.transform.SetParent (namespaceFolder);
                go.SetActive (false);

                replacerGOs.Add (go);
                Debug.Log (go.name);
            }
            return replacerGOs;
        }


    }
}


