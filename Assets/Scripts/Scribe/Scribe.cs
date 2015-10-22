
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class Scribe
{
	StringBuilder builder = new StringBuilder();
	string logPath;
	public Scribe (string logPath)
	{
		this.logPath = logPath;
	}
	
	public void Save()
	{
		if (File.Exists(logPath))
			File.AppendAllText(logPath, builder.ToString(), Encoding.UTF8);
		else
			File.WriteAllText(logPath, builder.ToString(), Encoding.UTF8);
		builder.Length = 0;
		
	}
	
	public void Log(string record)
	{
		#if UNITY_EDITOR
		Debug.Log(record);
		#endif
		builder.AppendLine(record);
	}
	
	public void LogFormat(string format, params object[] objects)
	{
		#if UNITY_EDITOR
		Debug.LogFormat(format, objects);
		#endif
		builder.AppendLine(string.Format(format, objects));
	}
}




