using UnityEngine;
using System.Collections;
using UIO;

namespace CoreMod
{
	[AShared]
	[ECompName ("structure")]
	public class Structure : EntityComponent
	{
		[Defined ("size")]
		public int Size;
		[Defined ("plot")]
		public ObjectCreationHandle.PlotType PlotType;

		public bool[,] Mask { get; internal set; }

		public override EntityComponent CopyTo (GameObject go)
		{
			Structure str = go.AddComponent<Structure> ();
			str.Size = Size;
			str.PlotType = PlotType;
			return str;
		}

		public override void PostCreate ()
		{
		}

		protected override void PostDestroy ()
		{
		}


		public override void LoadFromTable (ITable table)
		{
			this.Size = table.GetInt ("size");
			this.PlotType = table.GetString ("form") == "Rect" ? ObjectCreationHandle.PlotType.Rect : ObjectCreationHandle.PlotType.Circle;
			//Find.Root<ModsManager> ().Defs.LoadObject (this, table);
		}

		void InitMask ()
		{
			Mask = new bool[Size, Size];
			float distSqr = Size * Size / 4f;
			float halfSize = Size / 2f;
			for (int i = 0; i < Size; i++)
				for (int j = 0; j < Size; j++)
				{
					float distX = halfSize - i;
					float distY = halfSize - j;
					Mask [i, j] = distX * distX + distY * distY < distSqr;
				}
		}

	}

}


