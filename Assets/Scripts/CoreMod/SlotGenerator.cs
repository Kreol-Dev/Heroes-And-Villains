using UnityEngine;
using System.Collections;
using Demiurg.Core;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CoreMod
{
   /* public class SlotsControl : MonoBehaviour
    {
        public GameObject chief;
        public List<GameObject> subordinates;
        SlotsControl()
        {
            chief = null;
            subordinates = null;
        }
    }*/

    public class SlotGenerator :Demiurg.Core.Avatar

    {
        [AInput("slots")]
        [AOutput("slots")]
        List<GameObject> slots;
        [AConfig("density")]
        int density;
        [AConfig("name")]
        string name;
       // [AConfig("spatial")]
       // bool spatial;
        [AOutput("controllers")]
        List<GameObject> controllers;

        public override void Work()
        {
            Debug.LogError("Work");         
            
            foreach(var i in slots)
            {
                i.AddComponent<SlotsControl>();
            }
            controllers = new List<GameObject>();
            Stack<GameObject> freeslots = new Stack<GameObject>(slots);
            int count = slots.Count / density + 1;
            if (slots.Count == 0) count = 0;
            for(int i=0;i<count;i++)
            {
                GameObject contr = new GameObject(name);
                SlotsControl cmp= contr.AddComponent<SlotsControl>();
                controllers.Add(contr);
                for(int j=0; j<density&&freeslots.Count>0;j++)
                {
                    GameObject slot = freeslots.Pop();
                    slot.GetComponent<SlotsControl>().Chief = contr;
                    cmp.Subordinates.Add(slot);
                }
            }
            
            
                
                
            

            FinishWork();
        }
    }
}