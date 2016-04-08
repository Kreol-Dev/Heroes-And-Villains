using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UIO;

namespace AI
{
	//	public class Conditions : MonoBehaviour
	//	{
	//		Dictionary<Type, Condition> conditions = new Dictionary<Type, Condition> ();
	//
	//		public T GetCondition<T> () where T : Condition
	//		{
	//			Type t = typeof(T);
	//			Condition condition = null;
	//			conditions.TryGetValue (t, out condition);
	//			return condition as T;
	//		}
	//
	//		//		public void AssignCondition (Type conditionType)
	//		//		{
	//		//			conditions.Add(conditionType,
	//		//		}
	//	}

	public abstract class Condition
	{
		public abstract bool Satisfied { get; }

		public abstract Type StateType { get; }

		protected static Scribe Scribe { get; private set; }

		public Action PlannedAction { get; internal set; }

		public abstract PlanResult Plan (Planner planner);

		public abstract void LoadFromTable (object key, ITable table);

		public void DePlan ()
		{
			if (PlannedAction != null)
			{
				PlannedAction.DePlan ();
				PlannedAction = null;
			}
		}
	}

	public abstract class Condition<TCondition, C, S, T> : Condition where TCondition : Condition<TCondition, C, S, T> where C : Component where S : EntityState<T, C>
	{
		public C Component { get; internal set; }

		public static S State { get; private set; }

		public override Type StateType {
			get
			{
				return typeof(S);
			}
		}

		[Defined (1)]
		public T TargetValue { get; set; }

		public T CurValue { get { return State.Get (Component); } set { State.Set (Component, value); } }



		//		StringBuilder debugInfo = new StringBuilder (100);

		public sealed override PlanResult Plan (Planner planner)
		{

			PlannedAction = null;
			var relActions = planner.RelevantActions (typeof(TCondition));
			//var node = plan [lastNodeID];
//			debugInfo.Length = 0;
//			debugInfo.Append (this.GetType ());
//			debugInfo.Append (" ");
//			debugInfo.Append (this.Component.gameObject.name);
//			debugInfo.Append (" ");
			float minCost = float.MaxValue;
			PlanResult bestResult = new PlanResult (0, 0);
			int minIndex = -1;
			for (int i = 0, maxCount = relActions.Count; i < maxCount; i++)
			{
				var action = relActions [i];

				//debugInfo.Append (action.GetType ());
				//debugInfo.Append (" - ");
				var result = action.Plan (planner, this);
				//debugInfo.Append (String.Format ("cost = {0} effect = {1} \r\n", result.Cost, result.Effect));
				//Scribe.Log (debugInfo.ToString ());
				if (result.Effect <= 0)
				{
					continue;
				}
				float cost = (1 - planner.ContentratedOnResult) * result.Effect + planner.ContentratedOnResult * result.Cost;
				if (cost < minCost)
				{
					minCost = cost;
					bestResult = result;
					minIndex = i;
				}
			}
			if (minIndex >= 0)
				PlannedAction = relActions [minIndex];
			else
				PlannedAction = null;
			for (int i = 0, maxCount = relActions.Count; i < maxCount; i++)
			{
				if (i != minIndex)
					relActions [i].DePlan ();
			}
			planner.ReturnList (relActions);
			return bestResult;
		}

		public void Setup (GameObject go)
		{
			Component = go.GetComponent<C> ();
			if (State == null)
				State = Find.Root<StatesRoot> ().GetState<S> ();
		}

		public void Setup (C cmp)
		{
			Component = cmp;
			if (State == null)
				State = Find.Root<StatesRoot> ().GetState<S> ();
		}

		public bool CanBeApplied (GameObject go)
		{
			return go.GetComponent<C> () != null;
		}

	}

	public abstract class ClassCondition<TCondition, C, S, T> : Condition<TCondition, C, S, T> 
		where TCondition : ClassCondition<TCondition, C, S, T> 
		where C : Component where S : EntityState<T, C> where T : class
	{
		[Defined (2)]
		public ClassConditionSpec Spec;

		public sealed override bool Satisfied {
			get
			{
				switch (Spec)
				{
				case ClassConditionSpec.Equal:
					return TargetValue == CurValue;
				case ClassConditionSpec.NotEqual:
					return TargetValue != CurValue;
				}
				return false;
			}
		}
	}

	public enum ClassConditionSpec
	{
		Equal,
		NotEqual

	}

	public interface BorrowableCondition<T>
	{
		bool Borrowed { get; }

		void Borrow (T value);

		void DefaultBorrow ();

		T Release ();
	}

	public abstract class IntCondition<TCondition, C, S> : Condition<TCondition, C, S, int>, BorrowableCondition<int>
		where TCondition : IntCondition<TCondition, C, S> 
		where C : Component where S : IntState<C>
	{
		public NumericConditionSpec Spec;

		public bool Borrowed { get; internal set; }

		[Defined ("borrow")]
		int defaultBorrow = 0;
		int borrowedValue = 0;

		public void DefaultBorrow ()
		{
			Borrow (defaultBorrow);
		}

		public void Borrow (int value)
		{
			if (!Borrowed)
			{

				CurValue -= value;
				borrowedValue = value;
				Borrowed = true;
			}
		}

		public int Release ()
		{
			if (Borrowed)
			{
				Borrowed = false;
				int returnValue = borrowedValue;
				borrowedValue = 0;
				CurValue += returnValue;
				return returnValue;
			}
			return 0;
		}

		public sealed override bool Satisfied {
			get
			{
				switch (Spec)
				{
				case NumericConditionSpec.More:
					return CurValue + borrowedValue > TargetValue;
				case NumericConditionSpec.Less:
					return CurValue + borrowedValue < TargetValue;
				case NumericConditionSpec.Equal:
					return CurValue + borrowedValue == TargetValue;
				case NumericConditionSpec.MoreEqual:
					return CurValue + borrowedValue >= TargetValue;
				case NumericConditionSpec.LessEqual:
					return CurValue + borrowedValue <= TargetValue;
				case NumericConditionSpec.NotEqual:
					return CurValue + borrowedValue != TargetValue;
				}
				return false;
			}
		}
	}

	public abstract class FloatCondition<TCondition, C, S> : Condition<TCondition, C, S, float>, BorrowableCondition<float>
		where TCondition : FloatCondition<TCondition, C, S> 
		where C : Component where S : FloatState<C>
	{
		public NumericConditionSpec Spec;

		public bool Borrowed { get; internal set; }

		[Defined ("borrow")]
		float defaultBorrow = 0;
		float borrowedValue = 0;

		public void DefaultBorrow ()
		{
			Borrow (defaultBorrow);
		}

		public void Borrow (float value)
		{
			if (!Borrowed)
			{
				CurValue -= value;
				borrowedValue = value;
				Borrowed = true;
			}
		}

		public float Release ()
		{
			if (Borrowed)
			{
				Borrowed = false;
				float returnValue = borrowedValue;
				borrowedValue = 0;
				CurValue += returnValue;
				return returnValue;
			}
			return 0f;
		}

		public sealed override bool Satisfied {
			get
			{
				switch (Spec)
				{
				case NumericConditionSpec.More:
					return CurValue + borrowedValue > TargetValue;
				case NumericConditionSpec.Less:
					return CurValue + borrowedValue < TargetValue;
				case NumericConditionSpec.Equal:
					return Mathf.Approximately (CurValue + borrowedValue, TargetValue);
				case NumericConditionSpec.MoreEqual:
					return CurValue + borrowedValue >= TargetValue;
				case NumericConditionSpec.LessEqual:
					return CurValue + borrowedValue <= TargetValue;
				case NumericConditionSpec.NotEqual:
					return !Mathf.Approximately (CurValue + borrowedValue, TargetValue);
				}
				return false;
			}
		}
	}

	public enum NumericConditionSpec
	{
		Equal,
		NotEqual,
		More,
		Less,
		MoreEqual,
		LessEqual
	}
}


