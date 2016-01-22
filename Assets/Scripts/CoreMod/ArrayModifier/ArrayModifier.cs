using UnityEngine;
using System.Collections;
using Demiurg.Core;

namespace CoreMod
{
	public abstract class ArrayModifier<T> : Demiurg.Core.Avatar
	{
		[AOutput ("main")]
		[AInput ("array")]
		T[,] Array;

		public sealed override void Work ()
		{
			Prepare ();
			for (int i = 0; i < Array.GetLength (0); i++)
				for (int j = 0; j < Array.GetLength (1); j++)
				{
					Array [i, j] = Modify (i, j, Array [i, j]);
				}
			FinishWork ();
		}

		protected virtual void Prepare ()
		{

		}

		protected abstract T Modify (int x, int y, T value);
	}

	public abstract class ArrayModifierByArray<T> : Demiurg.Core.Avatar
	{
		[AOutput ("main")]
		[AInput ("array")]
		T[,] Array;
		[AInput ("mod_array")]
		T[,] ModArray;

		public sealed override void Work ()
		{
			Prepare ();
			for (int i = 0; i < Array.GetLength (0); i++)
				for (int j = 0; j < Array.GetLength (1); j++)
				{
					Array [i, j] = Modify (i, j, Array [i, j], ModArray [i, j]);
				}
			FinishWork ();
		}

		protected virtual void Prepare ()
		{

		}

		protected abstract T Modify (int x, int y, T value, T modValue);
	}
}



