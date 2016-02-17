using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConsoleScript : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Awake()
    {
        

    }
    void StartConsole()
    {
    //    f.awaking();
    //    m.ShowMessagePool();
    }

    public void ConsoleAppear()
    {
        this.transform.position.Set(0, 0, 0);
    }
    public void ConsoleDisappear()
    {
        this.gameObject.transform.position.Set(-300, 0, 0);
        Debug.LogError("LOL");
    }

}
