using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public enum MessageType { Notification, Warning, Error };
[System.Serializable]
public class InternalMessage
{
    public Category Category { get; internal set; }
    public string Message { get; internal set; }
    
    MessageType Type;
   // Type { get; internal set; }

    public InternalMessage(Category cat, string mes, MessageType type)
    {
        Category = cat;
        Message = mes;
        Type = type;
    }

    public void ShowTo(ConsoleMessage messege)
    {
        messege.Set(Category.Name, Message, Type.ToString(), Category.color);
    }
}
public class ConsoleMessage : MonoBehaviour
{

    [SerializeField]
    Text catText;
    [SerializeField]
    Text mesText;
    [SerializeField]
    Text typeText;
    [SerializeField]
    Image panelImage;
    public void Set(string category, string message, string type, Color color)
    {
        catText.text = category;
        mesText.text = message;
        typeText.text = type;
        panelImage.color = color;
    }
   
}
public class Category
{
    public int ID;
    public string Name;
    public Color color;
    public Category(int _id, string name, Color col)
    {
        ID = _id;
        Name = name;
        color = col;
    }
}

