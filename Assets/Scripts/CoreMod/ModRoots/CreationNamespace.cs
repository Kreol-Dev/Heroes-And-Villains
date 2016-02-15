using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CoreMod
{
	public class CreationNamespace
	{
		Scribe scribe = Scribes.Find ("Objects root");
		Dictionary<string, ObjectCreationHandle> prototypes = new Dictionary<string, ObjectCreationHandle> ();

		public string Name { get; internal set; }

		public CreationNamespace (string name)
		{
			Name = name;
		}

		public void AddProrotype (string name, ObjectCreationHandle prototype)
		{
			prototypes.Add (name, prototype);
		}

		public IEnumerable<ObjectCreationHandle> FindAvailable (TagsCollection tags)
		{
			return (from prototype in prototypes
			        where prototype.Value.IsAvailable (tags)
			        select prototype) as IEnumerable<ObjectCreationHandle>;
		}

		public static IEnumerable<ObjectCreationHandle> FindSimilar (TagsCollection tags, out int maxSimilarity, IEnumerable<ObjectCreationHandle> availablePrototypes = null)
		{
			int maximum = int.MinValue;
			List<ObjectCreationHandle> maxSimilar = new List<ObjectCreationHandle> ();
			if (availablePrototypes == null)
				availablePrototypes = (from prototype in prototypes
				                       select prototype) as IEnumerable<ObjectCreationHandle>;
			foreach (var handle in availablePrototypes)
			{
				int similarity = handle.HowSimilar (tags);
				if (maximum < similarity)
				{
					maximum = similarity;
					maxSimilar.Clear ();
					maxSimilar.Add (handle);

				} else if (maximum == similarity)
					maxSimilar.Add (handle);
			}
			maxSimilarity = maximum;
			return maxSimilar;
		}


	}

	public class ObjectCreationHandle
	{
		GameObject prototype;

		Dictionary<Type, List<Modifier>> modifiers;

		TagsCollection availability;

		Dictionary<Tag, int> weights;
		TagsCollection similarTags;

		public ObjectCreationHandle (GameObject go, IEnumerable<Tag> availabilityTags, Dictionary<Tag, int> similarity, Dictionary<Type, List<Modifier>> modifiers)
		{
			availability = new TagsCollection ();
			similarTags = new TagsCollection ();
			foreach (var tagPair in similarity)
				similarTags.AddTag (tagPair.Key);
			foreach (var tag in availabilityTags)
				availability.AddTag (tag);
			weights = similarity;
			this.modifiers = modifiers;
			prototype = go;

		}

		public bool IsAvailable (TagsCollection tags)
		{
			return tags.CheckTags (availability);
		}

		public int HowSimilar (TagsCollection tags)
		{
			return tags.ComputeSimilarity (similarTags, weights);
		}

		public GameObject CreateObject (TagsCollection tags)
		{
			GameObject newGO = new GameObject ();
			EntityComponent[] cmps = prototype.GetComponents<EntityComponent> ();
			foreach (var cmp in cmps)
			{
				cmp.CopyTo (newGO);
				if (modifiers.ContainsKey (cmp.GetType ()))
					foreach (var mod in modifiers[cmp.GetType()])
					{
						if (tags.CheckTag (mod.Tag))
							mod.Apply (cmp);
					}
			}
			return newGO;
		}

	}
}
