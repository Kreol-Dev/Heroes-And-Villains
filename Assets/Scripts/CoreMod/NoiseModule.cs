using System;
using Demiurg;

namespace CoreMod
{
	public class NoiseModule : CreationNode
	{
		NodeOutput<float[,]> main; 
		public NoiseModule()
		{
			main = Output<float[,]>("main");
		}
		protected override void Work ()
		{
			//Тут код шумогенератора
		}
	}
}

