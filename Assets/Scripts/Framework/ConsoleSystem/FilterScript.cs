using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class FilterScript : MonoBehaviour
{

    [SerializeField]
    Toggle toggle;
    [SerializeField]
    Text text;
    public void Set(string name, Action<bool> callback)
    {
        text.text = name;
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(x => callback(x));
    }
    
}
