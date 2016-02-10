using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class MessagePool : MonoBehaviour {

    //public GameObject messageGo = Resources.Load("ConsoleMessege") as GameObject;//Prefab of Console message
    public GameObject messageGo;
    public int DefaultSize = 5;
    int consolePoolSize = 0;// size of pool
    public int slider = 0;
    public Transform parent;//parent of messeges
    public Sprite custom_sprite;// sprite of messeges
    int newID=1;
    bool is_new;
    ConsoleMessege mes;
    public List<InternalMessage> messages = new List<InternalMessage>();// All messeges
    List<ConsoleMessege> messagesPool = new List<ConsoleMessege>();


    public List<Category> Filtr = new List<Category>();
    public List<Category> ActiveFilter = new List<Category>();
    List<InternalMessage> ShownMessages = new List<InternalMessage>();
    int startOffset = 0;
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
           
            ConsoleMessege messege = GameObject.Instantiate<GameObject>(messageGo).GetComponent<ConsoleMessege>();
            
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
    public void ShowCategory(Category cat)
    {
        activeCategories[cat.ID] = true;
        ShownMessages.Clear();
        foreach ( var message in messages)
        {
            if (activeCategories[message.Category.ID] == true)
                ShownMessages.Add(message);
        }
        ShowMessagePool();
    }

    public void HideCategory(Category cat)
    {
        activeCategories[cat.ID] = false;
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
        for (int i = slider, conMessage = 0; i < slider + consolePoolSize; i++, conMessage++)
        {
            ShownMessages[i].ShowTo(messagesPool[conMessage]);
        }

    }
    public void RegisterMessage(Category cat, string log,string type)
    {
        var mes = new InternalMessage(cat, log, type);
        if (activeCategories[cat.ID])
            ShownMessages.Add(mes);
        messages.Add(mes);
        
    }
    
    public Category RegisterCategory(string cat)
    {
        Category c = new Category(activeCategories.Count, cat);
        activeCategories.Add(true);
          
        Filtr.Add(c);

        return c;
    }
    bool Is_in_activeFiltr(Category cat,List<Category> fil)
    {
        for(int i=0;i<fil.Count;i++)
        {
            if (cat.ID == fil[i].ID)
            {
                return true;
            }
        }
        return false;
    }

    internal void ShowFrom(float value)
    {
        slider = (int)((float)ShownMessages.Count * value);
        if (slider > ShownMessages.Count - consolePoolSize)
            slider = ShownMessages.Count - consolePoolSize;
        ShowMessagePool();
    }
}
