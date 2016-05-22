using UnityEngine;
using System.Collections;
using UIO;
using System.Collections.Generic;
using System.Text;
using System;
using System.Reflection;
using System.Linq.Expressions;

namespace CoreMod
{
	[RootDependencies (typeof(ModsManager), typeof(TagsRoot), typeof(ObjectsCreator))]
	public class ObjectsRoot : ModRoot
	{
		Scribe scribe;
		Dictionary<string, CreationNamespace> namespaces = new Dictionary<string, CreationNamespace> ();


		ModsManager modsManager;

		protected override void CustomSetup ()
		{
			modsManager = Find.Root<ModsManager> ();

			var creator = Find.Root<ObjectsCreator> ();
			Transform mainFolder = this.transform;
			ITable table = modsManager.GetTable ("replacers");
			foreach (var replacerNamespaceName in table.GetKeys())
			{
				
				if (modsManager.IsTechnical (table, replacerNamespaceName))
					continue;
				try
				{
					CreationNamespace repNamespace = new CreationNamespace (replacerNamespaceName as string);
					namespaces.Add (replacerNamespaceName as string, repNamespace);


					ITable namespaceTable = table.GetTable (replacerNamespaceName);
					Transform replacersFolder = new GameObject (replacerNamespaceName as string).transform;
					replacersFolder.SetParent (mainFolder);
					foreach (var replacerName in namespaceTable.GetKeys())
					{

						ITable repTable = namespaceTable.GetTable (replacerName);
						GameObject prototypeGO = creator.CreateObject (replacerName as string, repTable);
						prototypeGO.transform.SetParent (replacersFolder);
						ObjectCreationHandle handle = null;
						ITable creationTable = repTable.GetTable ("creation", null);
						if (creationTable == null)
						{
							handle = new ObjectCreationHandle (prototypeGO);
						} else
						{
							ITable availabilityTable = creationTable.GetTable ("availability");
							ITable similarityTable = creationTable.GetTable ("similarity");
							//ITable fixedSpaceTable = creationTable.GetTable ("structure", null);
							var structure = prototypeGO.GetComponent<Structure> ();

							int size = -1;
							ObjectCreationHandle.PlotType plot = ObjectCreationHandle.PlotType.Nothing;
							if (structure != null)
							{
								size = structure.Size;
								plot = structure.PlotType;
							}
							handle = new ObjectCreationHandle (prototypeGO, 
							                                   GetAvailableTags (availabilityTable),
							                                   GetSimilarity (similarityTable),
							                                   GetModifiers (repTable), 
							                                   size, 
							                                   plot);
						}


						prototypeGO.SetActive (false);
						repNamespace.AddProrotype (replacerName as string, handle);
					}
				} catch (ITableMissingID e)
				{
					scribe.Log (e.ToString ());
					continue;
				}
			}
			Fulfill.Dispatch ();
		}

		IEnumerable<Tag> GetAvailableTags (ITable availabilityTable)
		{
			var tagsRoot = Find.Root<TagsRoot> ();
			List<Tag> tags = new List<Tag> ();
			foreach (var tagsNamespaceName in availabilityTable.GetKeys())
			{
				
				var namespaceTags = tagsRoot.GetTags (tagsNamespaceName as string);
				ITable tagsNamespaceTable = availabilityTable.GetTable (tagsNamespaceName);
				foreach (var tagKey in tagsNamespaceTable.GetKeys())
				{
					Tag tag = tagsRoot.GetTag (tagsNamespaceTable.GetString (tagKey), namespaceTags);
					if (tag != null)
						tags.Add (tag);
					else
						scribe.LogFormatError ("Can't find tag {0}", tagKey);
				}
			}
			return tags;
		}

		Dictionary<Tag, int> GetSimilarity (ITable similarityTable)
		{
			var tagsRoot = Find.Root<TagsRoot> ();
			Dictionary<Tag, int> tags = new Dictionary<Tag, int> ();
			foreach (var tagsNamespace in similarityTable.GetKeys())
			{

				var namespaceTags = tagsRoot.GetTags (tagsNamespace as string);
				var namespaceTable = similarityTable.GetTable (tagsNamespace);
				foreach (var tagKey in namespaceTable.GetKeys())
				{
					Tag tag = tagsRoot.GetTag (tagKey as string, namespaceTags);
					int value = namespaceTable.GetInt (tagKey);
					if (tag != null)
						tags.Add (tag, value);
					else
						scribe.LogFormatError ("Can't find tag {0}", tagKey);
				}
			}
			return tags;
		}

		Dictionary<Type, List<CreationModifier>> GetModifiers (ITable repTable)
		{
			ObjectsCreator creator = Find.Root<ObjectsCreator> ();
			TagsRoot tagsRoot = Find.Root<TagsRoot> ();
			Dictionary<Type, List<CreationModifier>> modifiers = new Dictionary<Type, List<CreationModifier>> ();
			foreach (var cmpName in repTable.GetKeys())
			{
				if (cmpName.Equals ("creation"))
					continue;
				Type cmpType = creator.GetRegisteredType (cmpName as string);
				if (cmpType == null)
					continue;
				
				try
				{
					
					ITable namespaces = repTable.GetTable (cmpName).GetTable ("modifiers");
					List<CreationModifier> modifiersList = new List<CreationModifier> ();
					foreach (var namespaceKey in namespaces.GetKeys())
					{
						var tags = tagsRoot.GetTags (namespaceKey as string);
						ITable modifiersTable = namespaces.GetTable (namespaceKey);
						foreach (var tagKey in modifiersTable.GetKeys())
						{
							ITable modifierTable = modifiersTable.GetTable (tagKey);
							Tag tag = tagsRoot.GetTag (tagKey as string, tags);
							CreationModifier mod = new CreationModifier (modifierTable, tag);
							modifiersList.Add (mod);
						}

					}
					modifiers.Add (cmpType, modifiersList);
				} catch (ITableMissingID e)
				{
					scribe.LogWarning (e.ToString ());
					continue;
				}
			}
			return modifiers;
		}




		protected override void PreSetup ()
		{

			scribe = Scribes.Find ("Objects root");
		}

		public CreationNamespace GetNamespace (string name)
		{
			if (namespaces.ContainsKey (name))
				return namespaces [name];
			else
			{
				scribe.LogFormatError ("Can't find objects namespace {0}", name);
				return new CreationNamespace (name);
			}
		}


		public IEnumerable<CreationNamespace> GetAllNamespaces ()
		{
			List<CreationNamespace> objectsList = new List<CreationNamespace> ();
			foreach (var names in namespaces)
				objectsList.Add (names.Value);
			return objectsList;
		}

	}




}
