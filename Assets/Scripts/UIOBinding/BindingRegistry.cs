using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using System.Reflection;
using MoonSharp.Interpreter.Interop;
using UIO;

namespace UIOBinding
{
	public class BindingRegistry : ITabledRegistry
	{
		
		public void Register (System.Type type)
		{
			//UserData.RegisterAssembly (Assembly.GetExecutingAssembly ());
			var desc = (StandardUserDataDescriptor)UserData.RegisterType (type);
			//desc.
		}



	}

}