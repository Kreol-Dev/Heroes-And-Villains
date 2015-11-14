
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class Scribe
{
    StringBuilder builder = new StringBuilder ();
    string logPath;
    public Scribe (string logPath)
    {
        this.logPath = logPath;
    }
	
    public void Save ()
    {
        if (File.Exists (logPath))
            File.AppendAllText (logPath, builder.ToString (), Encoding.UTF8);
        else
            File.WriteAllText (logPath, builder.ToString (), Encoding.UTF8);
        builder.Length = 0;
		
    }
	
    public void Log (string record)
    {
        #if UNITY_EDITOR
        Debug.Log (record);
        #endif
        builder.AppendLine (record);
    }
	
    public void LogFormat (string format, params object[] objects)
    {
        #if UNITY_EDITOR
        Debug.LogFormat (format, objects);
        #endif
        builder.AppendLine (string.Format (format, objects));
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
    }
}




