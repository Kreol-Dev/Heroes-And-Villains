using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using MoonSharp.Interpreter;
using System.Reflection;
using MoonSharp.Interpreter.Interop;

namespace DemiurgBinding
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