using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using System.Text;
using UIO;

namespace CoreMod
{
	[System.Serializable]
	public class Tag
	{
		static SlotComponentsProvider provider;

		[SerializeField]
		string
			name;

		public string Name { get { return name; } }

		public int ID { get; internal set; }

		ITable criteria;
		ICallback checkFunction;

		public Tag (string name, int id, ICallback checkFunction, ITable criteria)
		{
			if (provider == null)
				provider = new SlotComponentsProvider ();
			this.name = name;
			ID = id;
			this.checkFunction = checkFunction;
			this.criteria = criteria;
		}

		public bool CheckSlot (GameObject go)
		{
			provider.GO = go;
			if (checkFunction == null || criteria == null)
				return false;
			return (bool)checkFunction.Call (provider, criteria);
		}

		public override int GetHashCode ()
		{
			return ID;
		}
	}

	public class TagsComparer : IEqualityComparer<Tag>
	{
		public bool Equals (Tag x, Tag y)
		{
			return x.ID == y.ID;
		}

		public int GetHashCode (Tag obj)
		{
			return obj.ID;
		}
		
	}

	[System.Serializable]
	public class TagsCollection
	{
		static TagsComparer comparer = new TagsComparer ();

		public delegate void TagDelegate (Tag tag);

		public event TagDelegate TagAdded;
		public event TagDelegate TagRemoved;

		HashSet<Tag>
			assignedTags;


		public TagsCollection ()
		{
			assignedTags = new HashSet<Tag> (comparer);
			TagAdded += x => {
			};
			TagRemoved += x => {
			};
		}

		public void AddTag (Tag tag)
		{
			if (assignedTags.Add (tag))
				TagAdded (tag);
		}

		public void RemoveTag (Tag tag)
		{
			if (assignedTags.Remove (tag))
				TagRemoved (tag);
		}

		public bool Contains (Tag tag)
		{
			return assignedTags.Contains (tag);
		}

		public bool Contains (TagsCollection tags)
		{
			return Contains (tags.assignedTags);
		}

		public bool Contains (HashSet<Tag> tags)
		{
			foreach (var tag in tags)
				if (!assignedTags.Contains (tag))
				{
					return false;
				}
			return true;

		}

		public IEnumerable<Tag> Tags ()
		{
			return assignedTags;
		}

		public int TagsCount ()
		{
			return assignedTags.Count;
		}

		public int ComputeSimilarity (TagsCollection collection)
		{
			HashSet<Tag> otherTags = collection.assignedTags;
			int similarity = 0;
			foreach (var otherTag in otherTags)
				if (assignedTags.Contains (otherTag))
					similarity++;
			return similarity;
		}

		public int ComputeSimilarity (TagsCollection collection, Dictionary<Tag, int> weights)
		{
			HashSet<Tag> otherTags = collection.assignedTags;
			int similarity = 0;
			foreach (var otherTag in otherTags)
				if (assignedTags.Contains (otherTag))
				{
					int weight = 0;
					if (weights.TryGetValue (otherTag, out weight))
						similarity += weight;
				}
			return similarity;
		}

		class Comparator : Comparer<Tag>
		{
			public static Comparator Instance = new Comparator ();

			public override int Compare (Tag x, Tag y)
			{
				return x.ID.CompareTo (y.ID);
			}


		}
	}


	public class TagsConverter : IConverter<TagsCollection>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			TagsCollection collection = new TagsCollection ();
			TagsRoot tagsRoot = Find.Root<TagsRoot> ();
			ITable tagsTable = table.GetTable (key);
			foreach (var namespaceKey in tagsTable.GetKeys())
			{
				ITable namespaceTable = tagsTable.GetTable (namespaceKey);
				var tags = tagsRoot.GetTags (namespaceKey as string);
				if (tags == null)
					tags = tagsRoot.CreateNamespace (namespaceKey as string);
				foreach (var tagKey in namespaceTable.GetKeys())
				{
					Tag tag = null;
					string tagName = namespaceTable.GetString (tagKey);
					if (tags.ContainsKey (tagName))
					{
						tag = tagsRoot.GetTag (tagName, tags);
					} else
					{
						tag = tagsRoot.CreateNewTag (namespaceKey as string, tagName);
					}
					collection.AddTag (tag);
				}
			}
			return collection;
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new System.NotImplementedException ();
		}
	}
}


