using UnityEngine;
using System.Collections;
using UIO;

namespace CoreMod
{
	//Нечто размещаемое на карте
	public class Unit : EntityComponent
	{
		//Если неопределено - объект нельзя нигде разместить
		[Defined ("surface")]
		public int Surface = -1;

		//Если неопределено ( aka = 0) - объект нельзя сгенерировать с помощью стоимостного агента-генератора
		[Defined ("creation_cost")]
		public int CreationCost = 0;

		public override EntityComponent CopyTo (GameObject go)
		{
			Unit unit = go.AddComponent<Unit> ();
			unit.Surface = Surface;
			unit.CreationCost = CreationCost;
			return unit;
		}

		public override void PostCreate ()
		{
		}

		protected override void PostDestroy ()
		{
		}

		public override void LoadFromTable (ITable table)
		{
			Find.Root<ModsManager> ().Defs.LoadObject (this, table);
		}
	}

}

