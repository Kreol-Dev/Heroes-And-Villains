using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class MessagePool : MonoBehaviour {

    
    public GameObject messageGo;
    public int DefaultSize = 6;
    int consolePoolSize = 0; // size of pool
    public int slider = 0; //start of shown messages
    public Transform parent;//parent of messeges
    public Sprite custom_sprite;// sprite of messeges  
    public List<InternalMessage> messages = new List<InternalMessage>();// All messeges
    List<ConsoleMessage> messagesPool = new List<ConsoleMessage>();//references to ConsoleMessage
    public List<Category> Filtr = new List<Category>();// List of all Categories
    public List<TypeMes> Filtr2 = new List<TypeMes>();// List of all Types
    public List<InternalMessage> ShownMessages = new List<InternalMessage>();//
   // int startOffset = 0;
    // Use this for initialization
    void Start () {
        //IncreasePool(consolePoolSize);
       
    }
	
	// Update is called once per frame
	void Update () {
        //ShowMessagePool();
	
	}
    void IncreasePool(int delta)
    {
        for (int i = 0; i < delta; i++)
        {
           
            ConsoleMessage messege = GameObject.Instantiate<GameObject>(messageGo).GetComponent<ConsoleMessage>();
            
            messagesPool.Add(messege);
            messege.gameObject.transform.SetParent(parent,false);
            messege.gameObject.name = "ConsoleMessage"+(i+1).ToString();         
           
            messege.gameObject.transform.localPosition = new Vector3(0,63-28*i,0);
            messege.gameObject.GetComponent<Image>().sprite=custom_sprite;
            
        }
        consolePoolSize += delta;
        
    }
    void Awake()
    {
        IncreasePool(DefaultSize);

    }
    List<bool> activeCategories = new List<bool>();
    public List<bool> activeTypes = new List<bool>();
    public void ShowCategory(Category cat)
    {
        activeCategories[cat.ID] = true;
        
        ShownMessages.Clear();
        foreach (var message in messages)
        {
            if (activeCategories[message.Category.ID] == true && activeTypes[message.Type.ID]==true)
                ShownMessages.Add(message);
        }
        ShowMessagePool();
        
    }

    public void HideCategory(Category cat)
    {
        activeCategories[cat.ID] = false;
        //slider = 0;
        ShownMessages.Clear();
        foreach (var message in messages)
        {
            if (activeCategories[message.Category.ID] == true && activeTypes[message.Type.ID] == true)
                ShownMessages.Add(message);
        }
        ShowMessagePool();
    }
    public void FilterByType()
    {
        ShownMessages.Clear();
        foreach (var message in messages)
        {
            if (activeCategories[message.Category.ID] == true && activeTypes[message.Type.ID] == true)
                ShownMessages.Add(message);
        }
        ShowMessagePool();
    }
    public void ShowMessagePool()
    {
           
            for (int i = slider, conMessage = 0; i < slider + consolePoolSize; i++, conMessage++)
            {
                ShownMessages[i].ShowTo(messagesPool[conMessage]);
            }
       
       

    }
    public void SetActiveMessage()
    {
        if (consolePoolSize <= ShownMessages.Count)
        {
            for (int i = 0; i <consolePoolSize; i++)
            {
                GameObject.Find("ConsoleMessage" + (i+1).ToString()).SetActive(true);
            }
        }
       if(consolePoolSize>ShownMessages.Count)
        {
            for (int i = consolePoolSize; i > ShownMessages.Count; i--)
            {
                GameObject.Find("ConsoleMessage" + (i).ToString()).SetActive(false);
            }

        }
    }
    public void RegisterMessage(Category cat, string log,TypeMes type)
    {
        var mes = new InternalMessage(cat, log, type);
        if (activeCategories[cat.ID])
            ShownMessages.Add(mes);        
        messages.Add(mes);
        
    }
    int co = 0;
    public Category RegisterCategory(string cat)
    {
        Category c = new Category(activeCategories.Count, cat, new Color(20*co++,150-20*co,0));
       // Debug.LogError(c.Name+c.color.ToString());
        activeCategories.Add(true);
        Filtr.Add(c);

        return c;
    }


    internal void ShowFrom(float value)
    {
        slider = (int)((float)ShownMessages.Count * value);
        if (slider > ShownMessages.Count - consolePoolSize)
            slider = ShownMessages.Count - consolePoolSize;
        ShowMessagePool();
    }
    public int RegisterType(string type)
    {
       // TypeMes t = new TypeMes(activeTypes.Count, type);
        for(int i=0;i<messages.Count;i++)
        {
            if (type.Contains(messages[i].Type.Name))
            {
                return messages[i].Type.ID;
            }
        }
        activeTypes.Add(true);
        Filtr2.Add(new TypeMes(activeTypes.Count-1,type));
        return activeTypes.Count-1;
    }
}
