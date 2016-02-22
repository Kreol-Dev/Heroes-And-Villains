using UnityEngine;
using System.Collections;

namespace AI
{
	public abstract class Condition
	{

		public delegate void BoolDelegate (bool value);

		public event BoolDelegate FulfillmentChanged;

		private bool fulfilled;

		protected bool Fulfilled {
			get
			{
				fulfilled = IsFulfilled ();
				return fulfilled;
			}
			private set
			{
				if (fulfilled != value)
				{
					fulfilled = value;
					if (FulfillmentChanged != null)
						FulfillmentChanged (fulfilled);
				}
			}
		}

		protected GameObject Host { get; private set; }

		public abstract bool IsValidFor (GameObject go);

		public void AssignTo (GameObject go)
		{
			Host = go;
			Setup ();
		}

		protected abstract void Setup ();

		protected abstract bool IsFulfilled ();
	}
}


