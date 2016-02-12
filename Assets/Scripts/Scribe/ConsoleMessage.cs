using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class InternalMessage
{
    public Category Category { get; internal set; }
    public string Message { get; internal set; }
    public TypeMes Type { get; internal set; }

    public InternalMessage(Category cat, string mes, TypeMes type)
    {
        Category = cat;
        Message = mes;
        Type = type;
    }

    public void ShowTo(ConsoleMessage messege)
    {
        messege.Init(Category.Name, Message, Type.Name,Category.color);
    }
}
public class ConsoleMessage : MonoBehaviour
{
    Text catText;
    Text mesText;
    Text typeText;
    Image CatColor;
    
    void Awake()
    {
        catText = transform.FindChild("Category").gameObject.GetComponent<Text>();
        mesText = transform.FindChild("Message").gameObject.GetComponent<Text>();
        typeText = transform.FindChild("Type").gameObject.GetComponent<Text>();
        CatColor = this.GetComponent<Image>();
       
    }

    public void Init(string cat, string mes, string type,Color col)

    {
        catText.text = cat;
        mesText.text = mes;
        typeText.text = type;
        CatColor.color = col;
       // catText.color = col;

    }
}
