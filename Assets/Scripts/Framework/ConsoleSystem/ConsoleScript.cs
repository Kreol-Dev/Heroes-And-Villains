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
    public enum MessageType { Notification, Warning, Error }
    public void Init()
    {
        //categories.OnCategoryHided += OnCategoryHidedHandle;
        throw new NotImplementedException();
    }

    public object RegisterCategory(string name)
    {
        throw new NotImplementedException();
    }
    

    public void Log(string mesage, object category, object type)
    {
        throw new NotImplementedException();
    }
    

}
