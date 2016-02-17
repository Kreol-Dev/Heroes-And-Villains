
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;


public class Scribe
{
    StringBuilder builder = new StringBuilder ();
    string logPath;
    string logName;
    Category cat;
    MessagePool console;
    int n = 1;
    public Scribe (string logName)
    {

        this.logName = logName;
        logPath = "Logs\\" + logName + ".txt";
      


    }
    
	public void RegisterSelf(MessagePool console)
    {
        this.console = console;        
        cat = console.RegisterCategory(logName);
        
    }
    public void Save ()
    {
        if (File.Exists (logPath))
            File.AppendAllText (logPath, builder.ToString (), Encoding.UTF8);
        else
            File.WriteAllText (logPath, builder.ToString (), Encoding.UTF8);
        builder.Length = 0;
		
    }
	
    public void SaveLog(string log, string type)
    {
       
       
        console.RegisterMessage(cat, log,new TypeMes( console.RegisterType(type),type));

    }
  
    
    public void Log (string record)
    {
        //console.WriteMessage(cat, record, type);
        #if UNITY_EDITOR
        Debug.Log (record);       
        #endif
        builder.AppendLine (record);
      //  Debug.LogError("LOL" + record);



    }
	
    public void LogFormat (string format, params object[] objects)
    {
        #if UNITY_EDITOR
        Debug.LogFormat (format, objects);
        #endif
        builder.AppendLine (string.Format (format, objects));
        if (n == 1)
        {
            SaveLog(string.Format(format, objects), "LOL" + n.ToString());
            n++;
        }
        if (n == 2)
        {
            SaveLog(string.Format(format, objects), "LOL" + n.ToString());
            n--;
        }


    }
    public void LogWarning (string record)
    {
        #if UNITY_EDITOR
        Debug.LogWarning (record);
        #endif
        builder.AppendLine ("[WARNING] " + record);
       

    }
    
    public void LogFormatWarning (string format, params object[] objects)
    {
        #if UNITY_EDITOR
        Debug.LogWarningFormat (format, objects);
        #endif
        builder.AppendLine ("[WARNING] " + string.Format (format, objects));
        SaveLog(string.Format(format, objects),"Warning");

    }
    public void LogError (string record)
    {
        #if UNITY_EDITOR
        Debug.LogError (record);
        #endif
        builder.AppendLine ("[ERROR] " + record);
       
    }
    
    public void LogFormatError (string format, params object[] objects)
    {
        #if UNITY_EDITOR
        Debug.LogErrorFormat (format, objects);
        #endif
        builder.AppendLine ("[ERROR] " + string.Format (format, objects));
        SaveLog(string.Format(format, objects),"Error");
    }
}




public class Category
{
    public int ID;
    public string Name;
    public Color color;
    public Category(int _id,string name, Color col)
    {
        ID = _id;
        Name = name;
        color = col;
    }
}

public class TypeMes
{
    public string Name;
    public int ID;
    

    public TypeMes(int _id, string name)
    {
        ID = _id;
        Name = name;
    }
}