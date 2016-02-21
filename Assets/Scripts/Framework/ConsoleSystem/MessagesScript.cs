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
    public List<MessageType> ActiveType = new List<MessageType>();
    List<GameObject> consolemes = new List<GameObject>();
    [SerializeField]
    SliderScript slide;
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
            consolemes.Add(messege.gameObject);

        }
        consolePoolSize += delta;

    }
    public List<bool> activeCategories = new List<bool>();
    public void ShowCategory(Category cat)
    {
        activeCategories[cat.ID] = true;
       // slider -= ShownMessages.Count;
        ShownMessages.Clear();
        foreach (var message in messages)
        {
            if (activeCategories[message.Category.ID] == true && IsActiveType(message.Type))
                ShownMessages.Add(message);
        }
      //  slider += ShownMessages.Count;
        slide.SetValue((float)slider / ShownMessages.Count);
        ShowMessagePool();

    }

    public void HideCategory(Category cat)
    {
        activeCategories[cat.ID] = false;
        //slider -= ShownMessages.Count;
        ShownMessages.Clear();
        foreach (var message in messages)
        {
            if (activeCategories[message.Category.ID] == true && IsActiveType(message.Type))
                ShownMessages.Add(message);
        }
       // slider = slider+ShownMessages.Count;
       // if (slider < 0) slider = 0;
        slide.SetValue((float)slider / ShownMessages.Count);
        ShowMessagePool();
    }
    public void ShowMessagePool()
    {
        
        SetActiveMessage();
        for (int i = slider, conMessage = 0; (i < slider + consolePoolSize) && i < ShownMessages.Count && conMessage < consolePoolSize; i++, conMessage++)
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
                consolemes[i].SetActive(true);
            }
        }
        if (consolePoolSize > ShownMessages.Count)
        {
            for (int i = consolePoolSize-1 ; i >= ShownMessages.Count; i--)
            {
               consolemes[i].SetActive(false);
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
    public bool IsNewType(MessageType type)
    {
        for(int i=0;i<FilterType.Count;i++)
        {
            if (type == FilterType[i]) return false;
        }
        return true;
    }
    public bool IsActiveType(MessageType type)
    {
        for(int i=0;i<ActiveType.Count;i++)
        {
            if (type == ActiveType[i]) return true;
        }
        return false;
    }
    public void FilterByType()
    {
        ShownMessages.Clear();
        foreach (var message in messages)
        {
            if (activeCategories[message.Category.ID] == true && IsActiveType(message.Type))
                ShownMessages.Add(message);
        }
        ShowMessagePool();
    }
    }
