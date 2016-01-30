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
				if (key == "global")
					continue;
				ITable namespaceTable = tagsTable.GetTable (key);
				if (namespaceTable == null || namespaceTable.Contains ("global"))
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
				ITable tagTable = table.GetTable (key) as ITable;
				if (tagTable == null)
					continue;
				Tag tag = new Tag (key as string, id++, tagTable.GetCallback ("expression"), tagTable.GetTable ("criteria"));
				tags.Add (tag);

			}

			return tags;
		}
	}


}

