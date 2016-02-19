using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class MessagesScript : MonoBehaviour
{

    public GameObject messageGo;
    public int DefaultSize = 6;
    int consolePoolSize = 0; // size of pool
    public int slider = 0; //start of shown messages
    public Transform parent;//parent of messeges
    public Sprite custom_sprite;// sprite of messeges  
    public List<InternalMessage> messages = new List<InternalMessage>();// All messeges
    List<ConsoleMessage> messagesPool = new List<ConsoleMessage>();//references to ConsoleMessage
    public List<Category> FilterCat = new List<Category>();// List of all Categories     
    public List<MessageType> FilterType = new List<MessageType>();// List of all Types
    public List<InternalMessage> ShownMessages = new List<InternalMessage>();//

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Awake()
    {
        IncreasePool(DefaultSize);

    }

    void IncreasePool(int delta)
    {
        for (int i = 0; i < delta; i++)
        {

            ConsoleMessage messege = GameObject.Instantiate<GameObject>(messageGo).GetComponent<ConsoleMessage>();

            messagesPool.Add(messege);
            messege.gameObject.transform.SetParent(parent, false);
            messege.gameObject.name = "ConsoleMessage" + (i + 1).ToString();
            messege.gameObject.transform.localPosition = new Vector3(0, 63 - 28 * i, 0);
            messege.gameObject.GetComponent<Image>().sprite = custom_sprite;
            // consolemes.Add(messege.gameObject);

        }
        consolePoolSize += delta;

    }
    public List<bool> activeCategories = new List<bool>();
    public void ShowCategory(Category cat)
    {
        activeCategories[cat.ID] = true;

        ShownMessages.Clear();
        foreach (var message in messages)
        {
            if (activeCategories[message.Category.ID] == true)
                ShownMessages.Add(message);
        }
        ShowMessagePool();

    }

    public void HideCategory(Category cat)
    {
        activeCategories[cat.ID] = false;
        slider = 0;
        ShownMessages.Clear();
        foreach (var message in messages)
        {
            if (activeCategories[message.Category.ID] == true)
                ShownMessages.Add(message);
        }
        ShowMessagePool();
    }
    public void ShowMessagePool()
    {
         //SetActiveMessage();
        for (int i = slider, conMessage = 0; (i < slider + consolePoolSize) && conMessage < ShownMessages.Count; i++, conMessage++)
        {
            ShownMessages[i].ShowTo(messagesPool[conMessage]);
        }
    }
    public void RegisterMessage(Category cat, string log, MessageType type)
    {
        var mes = new InternalMessage(cat, log, type);
        if (activeCategories[cat.ID])
            ShownMessages.Add(mes);
        messages.Add(mes);

    }
    public Color GetHash(string name)
    {
        float r = 0, g = 0, b = 0;
        for (int i = 0; i < name.Length; i++)
        {
            r += name[i];
            g += name[i] * name[i];
            b = b + name[i] + 21;

        }
        r = (r % 15) / 15;
        g = 1 / (g % 12);
        b = (b % 10) / 10;
        Color c = new Color(r, g, b);
        return c;


    }

    public void SetActiveMessage()
    {
        if (consolePoolSize <= ShownMessages.Count)
        {
            for (int i = 0; i < consolePoolSize; i++)
            {
                GameObject.Find("ConsoleMessage"+(i+1).ToString()).SetActive(true);
            }
        }
        if (consolePoolSize > ShownMessages.Count)
        {
            for (int i = consolePoolSize ; i >= ShownMessages.Count; i--)
            {
                GameObject.Find("ConsoleMessage" + (i).ToString()).SetActive(false);
            }

        }
    }
    internal void ShowFrom(float value)
    {
        slider = (int)((float)ShownMessages.Count * value);
        if (ShownMessages.Count < consolePoolSize) slider = 0;
        else
        if (slider > ShownMessages.Count - consolePoolSize)
            slider = ShownMessages.Count - consolePoolSize;
        ShowMessagePool();
    }

}
