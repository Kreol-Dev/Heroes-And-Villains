using System;
using Demiurg;

namespace CoreMod
{
	public class NoiseModule : CreationNode
	{
		NodeOutput<float[,]> main; 
		IntParam sizeX;
		IntParam sizeY;

		public NoiseModule()
		{
			main = Output<float[,]>("main");
			sizeX = Config<IntParam>("planet_width");
			sizeY = Config<IntParam>("planet_height");
		}
		protected override void Work ()
		{
			//Тут код шумогенератора
		}
	}
}

