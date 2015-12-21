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
        Dictionary<string, CoreMod.Tag> tags;

        public override void Work ()
        {
            int id = 0;
            ITable tagsTable = Find.Root<ModsManager> ().GetTable (tagsTableName);
            tags = new Dictionary<string, Tag> ();
            foreach (var key in tagsTable.GetKeys())
            {
                ITable tagTable = tagsTable.Get (key) as ITable;
                Tag tag = new Tag (tagTable.Get ("name") as string, id++, tagTable.Get ("expression") as ICallback, tagTable.Get ("criteria") as ITable);
                tags.Add (tag.Name, tag);
            }
            FinishWork ();
        }
    }

}

