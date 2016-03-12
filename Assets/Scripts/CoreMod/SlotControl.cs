using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace CoreMod
{
    public class SlotsControl : SlotComponent
    {
        public GameObject Chief;
        public List<GameObject> Subordinates;
        
        void Awake()
        {
            Subordinates = new List<GameObject>();
        }
    }
}