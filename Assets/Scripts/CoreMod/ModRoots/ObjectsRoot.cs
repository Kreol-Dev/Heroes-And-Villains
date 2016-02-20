﻿using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
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


						ITable creationTable = repTable.GetTable ("creation");
						ITable availabilityTable = creationTable.GetTable ("availability");
						ITable similarityTable = creationTable.GetTable ("similarity");

						ObjectCreationHandle handle = new ObjectCreationHandle (prototypeGO, 
						                                                        GetAvailableTags (availabilityTable),
						                                                        GetSimilarity (similarityTable),
						                                                        GetModifiers (repTable));
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

		Dictionary<Type, List<Modifier>> GetModifiers (ITable repTable)
		{
			ObjectsCreator creator = Find.Root<ObjectsCreator> ();
			TagsRoot tagsRoot = Find.Root<TagsRoot> ();
			Dictionary<Type, List<Modifier>> modifiers = new Dictionary<Type, List<Modifier>> ();
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
					List<Modifier> modifiersList = new List<Modifier> ();
					foreach (var namespaceKey in namespaces.GetKeys())
					{
						var tags = tagsRoot.GetTags (namespaceKey as string);
						ITable modifiersTable = namespaces.GetTable (namespaceKey);
						foreach (var tagKey in modifiersTable.GetKeys())
						{
							ITable modifierTable = modifiersTable.GetTable (tagKey);
							Tag tag = tagsRoot.GetTag (tagKey as string, tags);
							Modifier mod = new Modifier (modifierTable, tag);
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
