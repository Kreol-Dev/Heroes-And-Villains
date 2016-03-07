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

		Dictionary<string, ObjectCreationHandle> Prototypes;

		public string Name { get; internal set; }

		public CreationNamespace (string name)
		{
			Prototypes = new Dictionary<string, ObjectCreationHandle> ();
			Name = name;
		}

		public void AddProrotype (string name, ObjectCreationHandle prototype)
		{
			Prototypes.Add (name, prototype);
		}

		public IEnumerable<ObjectCreationHandle> FindAvailable (TagsCollection tags)
		{
			return (from prototype in Prototypes
			        where prototype.Value.IsAvailable (tags)
			        select prototype.Value) as IEnumerable<ObjectCreationHandle>;
		}

		public IEnumerable<ObjectCreationHandle> FindSimilar (TagsCollection tags, out int maxSimilarity, IEnumerable<ObjectCreationHandle> availablePrototypes)
		{
			int maximum = int.MinValue;
			List<ObjectCreationHandle> maxSimilar = new List<ObjectCreationHandle> ();
			if (availablePrototypes == null)
			{
				maxSimilarity = int.MinValue;
				return maxSimilar;
			}
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

		public IEnumerable<ObjectCreationHandle> FindSimilar (TagsCollection tags, out int maxSimilarity)
		{
			int maximum = int.MinValue;
			List<ObjectCreationHandle> maxSimilar = new List<ObjectCreationHandle> ();
			IEnumerable<ObjectCreationHandle>	availablePrototypes = (from prototype in Prototypes
			                                                         select prototype.Value) as IEnumerable<ObjectCreationHandle>;
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

		public int PlotSize { get; internal set; }

		public enum PlotType
		{
			Circle,
			Rect,
			Nothing

		}

		public PlotType Plot { get; internal set; }

		public ObjectCreationHandle (GameObject go, IEnumerable<Tag> availabilityTags, 
		                             Dictionary<Tag, int> similarity, Dictionary<Type, List<Modifier>> modifiers,
		                             int plotSize, PlotType plot)
		{
			this.PlotSize = plotSize;
			this.Plot = plot;
			availability = new TagsCollection ();
			similarTags = new TagsCollection ();

			go.AddComponent<TagsVisual> ().Setup (availability);
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
			return tags.Contains (availability);
		}

		public int HowSimilar (TagsCollection tags)
		{
			return tags.ComputeSimilarity (similarTags, weights);
		}

		public GameObject CreateObject (TagsCollection tags)
		{
			GameObject newGO = new GameObject (prototype.name);
			EntityComponent[] cmps = prototype.GetComponents<EntityComponent> ();
			foreach (var cmp in cmps)
			{
				var addedCmp = cmp.CopyTo (newGO);
				if (modifiers.ContainsKey (cmp.GetType ()))
					foreach (var mod in modifiers[cmp.GetType()])
					{
						if (tags.Contains (mod.Tag))
							mod.Apply (addedCmp);
					}
			}
			return newGO;
		}

	}
}
