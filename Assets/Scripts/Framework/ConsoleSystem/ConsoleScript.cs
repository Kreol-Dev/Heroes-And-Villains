using UnityEngine;
using System.Collections;
using System;

public class ConsoleScript : MonoBehaviour
{
	[SerializeField]
	CategoriesScript categories;
	[SerializeField]
	TypesScript types;
	[SerializeField]
	MessagesScript messages;

	public void Init ()
	{
		//categories.OnCategoryHided += OnCategoryHidedHandle;
		//types.AddType(type, typeState => { if(typeState == true) вернуть в список сообщений сообщения типа type else изъять; });
		//throw new NotImplementedException();
	}

	public Category RegisterCategory (string name)
	{       
		Category c = new Category (messages.activeCategories.Count, name, messages.GetHash (name));
		messages.activeCategories.Add (true);
		messages.FilterCat.Add (c);        
		categories.AddCategory (c);
		return c;
	}

	void OnChangeShownMessages (string catName)
	{
		//пересобрать список видных сообщений
		//messages.Show(ShownMessages)
	}


	public void Log (string message, Category category, InternalMessage.MessageType type)
	{
		messages.RegisterMessage (category, message, type);
		if (messages.IsNewType (type))
		{
			messages.FilterType.Add (type);
			types.AddType (type);
			messages.ActiveType.Add (type);
		}
		messages.ShowMessagePool ();
	}
    

}
