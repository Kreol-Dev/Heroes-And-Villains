using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class ConsoleRoot : Root
{
    MessagePool console;
    protected override void PreSetup()
    {
        console = GameObject.Find("MessagePool").GetComponent<MessagePool>();
        Scribes.RegisterSelf(console);
        StartCoroutine(FiltersStartCoroutine());
    }

    IEnumerator FiltersStartCoroutine()
    {
        yield return null;
        yield return null;
        GameObject.Find("FilterPool").GetComponent<FilterPool>().awaking();
        console.ShowMessagePool();
    }
    protected override void CustomSetup()
    {
        Fulfill.Dispatch();
        //
       // GameObject.Find("StartButton").GetComponent<Button>();
        //GameObject.Find("MessagePool").GetComponent<MessagePool>().ShowMessagePool();
    }
}