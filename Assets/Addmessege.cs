using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Addmessege : MonoBehaviour {


    GameObject messageGo = Resources.Load("ConsoleMessage") as GameObject;

    public int DefaultSize = 5;
    int consolePoolSize = 0;
    public Transform parent;//parent of messeges
    public Sprite custom_sprite;// sprite of messeges

    ConsoleMessage mes ;
    public List<InternalMessage> messages = new List<InternalMessage>();// All messeges
    List<ConsoleMessage> messagesPool = new List<ConsoleMessage>();

    List<ConsoleMessage> ShownMessages = new List<ConsoleMessage>();
    int startOffset = 0;
    int delPool=0;// number of shown messeges
    int i =1;
	// Use this for initialization
	void Start () {
        // messege = Instantiate(Resources.Load("C:\\Users\\Вадим\\Documents\\GitHub\\Heroes-And-Villains\\Assets\\Resources\\UI\\ConsoleMessage.prefab",typeof(GameObject))) as GameObject;
        
	}
	
	// Update is called once per frame
	void Update ()
    {      
        
          //ShowListOfMesseges(MessegePool);      
        
	
	}

    void IncreasePool(int delta)
    {
        for (int i = 0; i < delta; i++)
        {
            ConsoleMessage messege = GameObject.Instantiate<GameObject>(messageGo).GetComponent<ConsoleMessage>();
            messagesPool.Add(messege);
            messege.gameObject.transform.SetParent(parent, false);
            messege.gameObject.SetActive(false);
        }
        consolePoolSize += delta;
    }
    void Awake()
    {
        IncreasePool(DefaultSize);

    }


    /*
    public void AddNote()
    {
        for (int n = 0; n < 5; n++)
        {
            GameObject newmessege = null;
            newmessege = GameObject.Instantiate(messege, new Vector3(messege.transform.position.x, messege.transform.position.y - 20 * i++, messege.transform.position.z), transform.rotation) as GameObject;
            newmessege.transform.SetParent(this.transform);
            newmessege.name = "ConsoleMessage" + (i - 1).ToString();

            newmessege.GetComponent<Image>().sprite = custom_sprite;
           // newmessege.GetComponentsInChildren<Text>()[1].text = "LOL";
            newmessege.GetComponentsInChildren<Text>()[0].text = "Category" + n.ToString();
            newmessege.GetComponentsInChildren<Text>()[1].text = "Type" + n.ToString();
            newmessege.GetComponentsInChildren<Text>()[2].text = "NewMessege" + n.ToString();


            Debug.Log(newmessege.transform.position.ToString());
        }
        
        
       
    }
    public void testFunc()
    {
        ConsoleMessage mes = new ConsoleMessage("  1", " 2 ", "3  ");
        AddMessege(mes);
    }
    public void ShowListOfMesseges()
    {
        List<ConsoleMessage> c = MessegePool;
        ClearConsoleView(delPool);
        Debug.LogError(c.Count.ToString());
       
        for(int i=1;i<=c.Count;i++)
        {
            GameObject mes = null;
            mes = GameObject.Instantiate(messege, new Vector3(messege.transform.position.x, messege.transform.position.y - 28* i, messege.transform.position.z), messege.transform.rotation) as GameObject;
            mes.transform.SetParent(this.transform);
            mes.name = "ConsoleMessage" + i.ToString();
            mes.GetComponent<Image>().sprite = custom_sprite;

            mes.GetComponentsInChildren<Text>()[0].text = c[i-1].GetCategory();
            mes.GetComponentsInChildren<Text>()[1].text = c[i-1].GetType();
            mes.GetComponentsInChildren<Text>()[2].text = c[i-1].GetMessege();


        }
        delPool = c.Count;

    }
    public void AddMessege(ConsoleMessage mes)
    {
        MessegePool.Add(mes);
    }
    void ClearConsoleView(int numb)
    {
        if (numb != 0)
        {
            for(int i=1;i<=delPool;i++)
            {
                GameObject g = GameObject.Find("ConsoleMessage" + i.ToString());
                Destroy(g);
            }

        }
    }*/
}
