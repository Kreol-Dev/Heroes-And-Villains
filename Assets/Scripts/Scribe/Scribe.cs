
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
    // Category cat = null;
    Category cat;
    MessagePool console;
    public Scribe (string logName)
    {

        this.logName = logName;
        logPath = "Logs\\" + logName + ".txt";
       // MessagePool console = GameObject.Find("MessagePool").GetComponent<MessagePool>();
       // cat= console.RegisterCategory(logPath);
        //Debug.LogError("QWERTY");


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
        //console.messages.Add(new InternalMessage(new Category(0,"LOL"), log, "Error"));
        //console.RegisterMessage(log, "Error");
       
        console.RegisterMessage(cat, log, type);

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
        SaveLog(string.Format(format, objects),"LOL");
        
       
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
       // SaveLog(string.Format(format, objects));

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
       // SaveLog(string.Format(format, objects));
    }
}




public class Category
{
    public int ID;
    public string Name;
    public Category(int _id,string name)
    {
        ID = _id;
        Name = name;
    }
}