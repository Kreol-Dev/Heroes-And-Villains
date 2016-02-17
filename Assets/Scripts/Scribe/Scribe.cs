
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class Scribe
{
    StringBuilder builder = new StringBuilder ();
    string name;
    string logPath;
    ConsoleScript console;
    object category;
    public Scribe (string name)
    {
        logPath = "Logs//" + name + ".txt";
        this.name = name;
        console = Object.FindObjectOfType<ConsoleScript>();
        category = console.RegisterCategory(name);

    }
	
    public void Save ()
    {
        if (File.Exists (name))
            File.AppendAllText (logPath, builder.ToString (), Encoding.UTF8);
        else
            File.WriteAllText (logPath, builder.ToString (), Encoding.UTF8);
        builder.Length = 0;
		
    }
	
    public void Log (string record)
    {
        console.Log(record, category, ConsoleScript.MessageType.Notification);
        builder.AppendLine (record);
    }
	
    public void LogFormat (string format, params object[] objects)
    {
        string record = string.Format(format, objects);
        console.Log(record, category, ConsoleScript.MessageType.Notification);
        builder.AppendLine (record);
    }
    public void LogWarning (string record)
    {
        console.Log(record, category, ConsoleScript.MessageType.Warning);
        builder.AppendLine ("[WARNING] " + record);
    }
    
    public void LogFormatWarning (string format, params object[] objects)
    {
        string record = string.Format(format, objects);
        console.Log(record, category, ConsoleScript.MessageType.Warning);
        builder.AppendLine ("[WARNING] " + record);
    }
    public void LogError (string record)
    {
        console.Log(record, category, ConsoleScript.MessageType.Error);
        builder.AppendLine ("[ERROR] " + record);
    }
    
    public void LogFormatError (string format, params object[] objects)
    {
        string record = string.Format(format, objects);
        console.Log(record, category, ConsoleScript.MessageType.Error);
        builder.AppendLine ("[ERROR] " + record);
    }
}




