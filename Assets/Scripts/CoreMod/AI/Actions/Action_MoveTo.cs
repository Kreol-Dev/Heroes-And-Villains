using UnityEngine;
using System.Collections;
using AI;

namespace CoreMod
{
	//	public class Action_MoveTo : Action
	//	{
	//		PositionState positionState;
	//
	//		public override void AttachTo (GameObject go)
	//		{
	//			positionState = Find.Root<StatesRoot> ().GetState<PositionState> ();
	//
	//		}
	//
	//		public override float Potential {
	//			get
	//			{
	//				throw new System.NotImplementedException ();
	//			}
	//			internal set
	//			{
	//				throw new System.NotImplementedException ();
	//			}
	//		}
	//
	//		public override float Risk {
	//			get
	//			{
	//				throw new System.NotImplementedException ();
	//			}
	//			internal set
	//			{
	//				throw new System.NotImplementedException ();
	//			}
	//		}
	//
	//		public override float Gradient {
	//			get
	//			{
	//				throw new System.NotImplementedException ();
	//			}
	//			internal set
	//			{
	//				throw new System.NotImplementedException ();
	//			}
	//		}
	//
	//		public override float Cost {
	//			get
	//			{
	//				throw new System.NotImplementedException ();
	//			}
	//			internal set
	//			{
	//				throw new System.NotImplementedException ();
	//			}
	//		}
	//
	//
	//
	//	}

	public class PositionState : EntityState<Vector2, Transform>
	{
		public override Vector2 Get (Transform cmp)
		{
			return cmp.position;
		}

		public override void Set (Transform cmp, Vector2 value)
		{
			cmp.position = value;
		}

		public void Move (Transform cmp, Vector2 value)
		{
			cmp.position += (Vector3)value;
		}
	}

	public class MovableState : EntityState<bool, Transform>
	{
		public override bool Get (Transform cmp)
		{
			return cmp.gameObject.isStatic;
		}

		public override void Set (Transform cmp, bool value)
		{
			cmp.gameObject.isStatic = value;
		}
	}


}
