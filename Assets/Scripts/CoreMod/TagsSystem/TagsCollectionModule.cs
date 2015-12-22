using UnityEngine;
using System.Collections;
using Demiurg.Core;
using System.Collections.Generic;
using Demiurg.Core.Extensions;

namespace CoreMod
{
    public class TagsCollectionModule : Demiurg.Core.Avatar
    {
        [AConfig ("tags_table")]
        string tagsTableName;
        [AOutput ("tags")]
        Dictionary<string, List<Tag>> tags;

        int id = 0;

        public override void Work ()
        {
            ITable tagsTable = Find.Root<ModsManager> ().GetTable (tagsTableName);
            tags = new Dictionary<string, List<Tag>> ();
            foreach (var key in tagsTable.GetKeys())
            {
                ITable namespaceTable = tagsTable.Get (key) as ITable;
                if (namespaceTable == null || namespaceTable.Get ("global") != null)
                    continue;
                string strKey = key as string;
                if (strKey == null)
                    continue;
                tags.Add (strKey, GetTags (namespaceTable));

            }
            FinishWork ();
        }

        List<Tag> GetTags (ITable table)
        {
            List<Tag> tags = new List<Tag> ();
            foreach (var key in table.GetKeys())
            {
                ITable tagTable = table.Get (key) as ITable;
                if (tagTable == null)
                    continue;
                Tag tag = new Tag (key as string, id++, tagTable.Get ("expression") as ICallback, tagTable.Get ("criteria") as ITable);
                tags.Add (tag);

            }

            return tags;
        }
    }


}

