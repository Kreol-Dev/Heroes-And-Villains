
using System.Collections;
using System.Collections.Generic;
using Demiurg;


namespace CoreMod
{
	public class DistinctorModule : CreationNode
	{
		class LevelValue
		{
			public FloatParam Level = new FloatParam("level");
			public IntParam Value = new IntParam("val");
		}
		NodeOutput<int[,]> mainO; 
		NodeInput<float[,]> mainI;
		GlobalArrayParam<LevelValue> levels;
		public DistinctorModule()
		{
			mainO = Output<int[,]>("main");
			mainI = Input<float[,]>("main");
			levels = Config<GlobalArrayParam<LevelValue>>("levels");
		}
		protected override void Work ()
		{


		}
	}
}



