
using System.Collections;
using System.Collections.Generic;
using Demiurg;
namespace CoreMod
{
	public class ContinuousChunksModule : CreationNode
	{
		class Chunk
		{
			public IntParam Value = new IntParam("value");
		}

		NodeInput<int[,]> mainI;
		NodeOutput<List<List<int>>> mainO;

		StringParam planetConnectivity;
		ArrayParam<Chunk> chunks;
		public ContinuousChunksModule()
		{
			mainI = Input<int[,]>("main");
			mainO = Output<List<List<int>>>("main");
			planetConnectivity = Config<StringParam>("planet_connectivity");
			chunks = Config<ArrayParam<Chunk>>("chunks");
		}
		protected override void Work ()
		{
			
		}
	}
}





