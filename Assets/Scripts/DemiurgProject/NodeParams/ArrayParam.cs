
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using MoonSharp.Interpreter;

namespace Demiurg
{
    public class ArrayParam<T> : GlobalArrayParam<T> where T : class, new()
    {
        public ArrayParam (string name):base(name)
        {
        }
        public override void GetItself (Table table)
        {
            Debug.Log (Name + " param");
            base.GetItself (table.Get (Name).Table);
        } 
        public override void GetItselfFrom (object o)
        {
            base.GetItself (o as Table);
        }

    }

    public class GlobalArrayParam<T> : NodeParam<T[]> where T : class, new()
    {
        static List<FieldInfo> nodeParams = new List<FieldInfo> ();
        static GlobalArrayParam ()
        {
            Type t = typeof(T);
            Debug.Log ("static contructor: " + typeof(T));
            Type nodeType = typeof(NodeParam);
            FieldInfo[] infos = t.GetFields ();
            Debug.Log (infos.Length);
            foreach (var info in infos)
            {
                if (info.FieldType.IsSubclassOf (nodeType))
                    nodeParams.Add (info);
            }
        }
        public GlobalArrayParam (string name):base(name)
        {
        }
        public override void GetItself (Table table)
        {
            Debug.Log ("Get array param " + Name);
            var list = new List<DynValue> (table.Values);
            List<T> contentList = new List<T> ();
            Debug.Log (list.Count);
            for (int i = 0; i < list.Count; i++)
            {

                var element = list [i];
                if (element.Type != DataType.Table)
                    continue;
                T entry = new T ();
                Debug.Log (nodeParams.Count);
                foreach (var param in nodeParams)
                {
                    Debug.Log (((NodeParam)param.GetValue (entry)).Name);
                    ((NodeParam)param.GetValue (entry)).GetItself (element.Table);
                }
                contentList.Add (entry);
                    
            }
            Content = contentList.ToArray ();


        }
        public override void GetItselfFrom (object o)
        {
            GetItself (o as Table);
        }
    }
}





