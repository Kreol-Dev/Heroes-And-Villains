using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ConsoleRoot : Root
{
    protected override void PreSetup()
    {

        Scribes.RegisterSelf(GameObject.Find("MessagePool").GetComponent<MessagePool>());
    }
    protected override void CustomSetup()
    {
        Fulfill.Dispatch();
    }
}