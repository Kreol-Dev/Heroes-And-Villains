using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class InternalMessage
{
    public Category Category { get; internal set; }
    public string Message { get; internal set; }
    public string Type { get; internal set; }

    public InternalMessage(Category cat, string mes, string type)
    {
        Category = cat;
        Message = mes;
        Type = type;
    }

    public void ShowTo(ConsoleMessege messege)
    {
        messege.Init(Category.Name, Message, Type);
    }
}
public class ConsoleMessege : MonoBehaviour
{
    Text catText;
    Text mesText;
    Text typeText;
    void Awake()
    {
        catText = transform.FindChild("Category").gameObject.GetComponent<Text>();
        mesText = transform.FindChild("Messege").gameObject.GetComponent<Text>();
        typeText = transform.FindChild("Type").gameObject.GetComponent<Text>();
    }
    
    public void Init(string cat, string mes, string type)

    {
        catText.text = cat;
        mesText.text = mes;
        typeText.text = type;
    }
}
