using UnityEngine;
using System.Collections;
using UAI;
using System.Collections.Generic;
using System;

namespace CoreMod
{
	[ECompName ("cluster_agent")]
	public class ClusterAgent : CreationAgent, IGenerationAgent
	{
		public override EntityComponent CopyTo (GameObject go)
		{
			var newAgent = go.AddComponent<ClusterAgent> ();

			return newAgent;
		}

		public override void PostCreate ()
		{
		}

		protected override void PostDestroy ()
		{
		}


		System.Action _onAgentFinish;
		public int Points;

		public void Generate (int points, System.Action onAgentFinish)
		{
			GetComponent<Utilities> ().AddUtility (new C_HasCreationPoints () {
				Agent = this,
				TargetValue = 0,
				Type = ValueConditionType.Less
			});
			GetComponent<Agent> ().AgentHasNoActions += OnFinishedGeneration;
			_onAgentFinish = onAgentFinish;

		}

		void OnFinishedGeneration ()
		{
			_onAgentFinish ();
			Destroy (gameObject);
		}

	}

	public class C_HasCreationPoints : IntCondition
	{
		public CreationAgent Agent;

		public override bool Satisfied {
			get
			{
				switch (this.Type)
				{
				case ValueConditionType.Less:
					return Agent.CreationPoints < TargetValue;
				case ValueConditionType.More:
					return Agent.CreationPoints > TargetValue;
				}
				return false;
			}
		}

		public override bool IsValidFor (GameObject go)
		{
			return go.GetComponent<CreationAgent> () != null;
		}

		public override void InitFor (GameObject go)
		{
			Agent = go.GetComponent<CreationAgent> ();
		}
	}

	public class P_ChangeCreationPoints : IntPromise<C_HasCreationPoints>, IInstantPromise<C_HasCreationPoints>
	{
		
		protected override float CheckCondition (C_HasCreationPoints condition)
		{
			int newValue;
			if (this.Change == PromisedChange.Increase)
				newValue = condition.Agent.CreationPoints + this.Delta;
			else
				newValue = condition.Agent.CreationPoints - this.Delta;
			return Mathf.InverseLerp (condition.Agent.CreationPoints, condition.TargetValue, newValue);
		}

		public void Perform (C_HasCreationPoints c)
		{
			if (this.Change == PromisedChange.Increase)
				c.Agent.CreationPoints += this.Delta;
			else
				c.Agent.CreationPoints -= this.Delta;
		}

		public void Reverse (C_HasCreationPoints c)
		{
			if (this.Change == PromisedChange.Increase)
				c.Agent.CreationPoints -= this.Delta;
			else
				c.Agent.CreationPoints += this.Delta;
		}

		public override System.Type ConditionType { get { return typeof(C_HasCreationPoints); } }
		
	}

	public struct PromisesAndConditions
	{
		public List<Promise> Promises;
		public List<Condition> Conditions;
	}

	public abstract class CreationAgent : EntityComponent
	{
		public int CreationPoints;


	}


	public interface IUnitAction
	{
		void Setup (Unit unit, ClusterAgent hostAgent, int iteration);
	}

	public class UnitsAgentsSubsystem : ProtoUtSystem<IUnitAction, UnitsAgentsSubsystem>
	{
		static UnitsRoot _unitsRoot;

		protected override bool CreateActions (Utilities uts, int iteration)
		{
			if (_unitsRoot == null)
				_unitsRoot = Find.Root<UnitsRoot> ();
			var hostAgent = uts.GetComponent<ClusterAgent> ();
			bool anyNewActions = false;
			foreach (var unit in _unitsRoot.Units)
			{
				foreach (var relAction in allRelevatActions)
				{
					if (usedActions.Add (new KeyValuePair<object, Type> (unit, relAction)))
					{

						UAI.Action action = Activator.CreateInstance (relAction) as UAI.Action;
						var unitAction = action as IUnitAction;
						unitAction.Setup (unit, hostAgent, iteration);
						anyNewActions = true;
						actionsList.Add (action);
					}
				}
			}
			return anyNewActions;
		}

	}

	public class A_CreateUnitAgent : UAI.Action, IUnitAction
	{
		static ObjectCreationHandle workerAgent;
		Unit unit;
		ClusterAgent hostAgent;
		int iteration;

		public void Setup (Unit unit, ClusterAgent hostAgent, int iteration)
		{
//			if (workerAgent == null)
//				workerAgent = Find.Root<ObjectsRoot> ().GetNamespace ("coremod").GetByName ("unit_agent");
			this.unit = unit;
			this.hostAgent = hostAgent;
			this.iteration = iteration;

		}

		public override float ComputeUtility (Utilities uts, out int notSatisfiedConditions, out int actionIteration)
		{
			throw new NotImplementedException ();
		}

		public override bool IsPossible ()
		{
			throw new NotImplementedException ();
		}

		public override bool IsPossibleAtAll (GameObject go)
		{
			return false;
		}

		public override void Perform (System.Action onPerformed)
		{
			throw new NotImplementedException ();
		}

		public override void Discard (Utilities uts)
		{
			throw new NotImplementedException ();
		}

		public override void Approve (Utilities uts, int iteration)
		{
			throw new NotImplementedException ();
		}
	}

	[RootDependencies (typeof(ObjectsRoot))]
	public class UnitsRoot : ModRoot
	{
		public IEnumerable<Unit> Units { get; internal set; }

		protected override void CustomSetup ()
		{
			//var mm = Find.Root<ModsManager> ();
			var objectsRoot = Find.Root<ObjectsRoot> ();
			List<Unit> units = new List<Unit> ();
			foreach (var space in objectsRoot.GetAllNamespaces())
			{
				var objects = space.GetAll ();
				foreach (var obj in objects)
				{
					
					var unit = obj.Prototype.GetComponent<Unit> ();
					if (unit == null)
						continue;
					units.Add (unit);
				}
			}
			Units = units;
			Fulfill.Dispatch ();
		}
		
	}

	public class P_ChangeFreeSpace : Promise
	{
		public override float CheckCondition (Condition condition)
		{
			return 0f;
		}

		public override System.Type ConditionType {
			get
			{
				return typeof(C_HasFreeSpace);
			}
		}

	}

	public class C_HasFreeSpace : Condition
	{
		public int Surface;
		public int Size;
		public ObjectCreationHandle.PlotType Type;
		public ReacheableSpace Space;
		public List<TileHandle> PossibleTiles;

		public override bool Satisfied {
			get
			{
				var tiles = Space.GetTiles (Surface, Size);
				return tiles.Count > 0;
			}
		}

		public override void InitFor (GameObject go)
		{

			Space = Actor.GetComponent<ReacheableSpace> ();
		}

		public override bool IsValidFor (GameObject go)
		{
			return go.GetComponent<ReacheableSpace> () != null;
		}
	}


	//	public interface IStructureCreationAction
	//	{
	//		void Setup (Structure structure);
	//	}
	//
	//	public class StructuresCreationSystem : ProtoUtSystem<IStructureCreationAction, StructuresCreationSystem>
	//	{
	//		protected override bool CreateActions (List<Condition> conditions, int iteration)
	//		{
	//
	//			return false;
	//		}
	//
	//	}
	//
		

	//
	//	public interface IAgentAction
	//	{
	//		void Setup (ObjectCreationHandle handle);
	//	}
	//
	//	public class CreateAgent : Action, IAgentAction
	//	{
	//		P_ChangeCreationPoints changePoints = new P_ChangeCreationPoints ();
	//		C_HasCreationPoints hasPoints = new C_HasCreationPoints ();
	//		CreationAgent agent;
	//
	//		public override float ComputeUtility (Utilities uts, out int notSatisfiedConditions, out int actionIteration)
	//		{
	//			throw new System.NotImplementedException ();
	//		}
	//
	//		public override bool IsPossible ()
	//		{
	//			throw new System.NotImplementedException ();
	//		}
	//
	//		public override bool IsPossibleAtAll (GameObject go)
	//		{
	//			return go.GetComponent<ClusterAgent> ();
	//		}
	//
	//		public override void Perform (System.Action onPerformed)
	//		{
	//			throw new System.NotImplementedException ();
	//		}
	//
	//		public override void Discard (Utilities uts)
	//		{
	//			Object.Destroy (agent.gameObject);
	//		}
	//
	//		public override void Approve (Utilities uts, int iteration)
	//		{
	//			throw new System.NotImplementedException ();
	//		}
	//
	//		public void Setup (ObjectCreationHandle handle)
	//		{
	//			GameObject agent = handle.CreateObject ();
	//			this.agent = agent.GetComponent<CreationAgent> ();
	//		}
	//	}
	//
	//
	//	public class CreateStructureAction : Action, IStructureCreationAction
	//	{
	//		Structure targetStructure;
	//		P_ChangeCreationPoints points = new P_ChangeCreationPoints ();
	//		C_HasCreationPoints hasPoints = new C_HasCreationPoints ();
	//		C_HasFreeSpace hasSpace = new C_HasFreeSpace ();
	//
	//		public void Setup (Structure structure)
	//		{
	//			hasSpace.Size = structure.TilesSize;
	//			hasSpace.Type = structure.PlotType;
	//			hasSpace.Surface = 0;
	//			points.Change = PromisedChange.Decrease;
	//			points.Delta = structure.Cost;
	//
	//		}
	//
	//
	//		public override float ComputeUtility (Utilities uts, out int notSatisfiedConditions, out int actionIteration)
	//		{
	//			if (hasSpace.Space == null)
	//				hasSpace.Space = uts.GetComponent<ReacheableSpace> ();
	//			actionIteration = hasSpace.Iteration;
	//			if (hasPoints.Agent == null)
	//				hasPoints.Agent = uts.GetComponent<ClusterAgent> ();
	//			if (hasSpace.Satisfied)
	//				notSatisfiedConditions = 0;
	//			else
	//				notSatisfiedConditions = 1;
	//			if (!hasPoints.Satisfied)
	//				notSatisfiedConditions += 1;
	//			return uts.GetUtility (points);
	//		}
	//
	//		public override bool IsPossible ()
	//		{
	//			return hasPoints.Satisfied && hasSpace.Satisfied;
	//		}
	//
	//		public override bool IsPossibleAtAll (GameObject go)
	//		{
	//			return go.GetComponent<ClusterAgent> () != null;
	//		}
	//
	//		public override void Perform (System.Action onPerformed)
	//		{
	//			GameObject structureAgentGO = new GameObject ("StructureAgent " + this.targetStructure.name);
	//			var structureAgent = structureAgentGO.AddComponent<StructureAgent> ();
	//			structureAgent.Structure = targetStructure;
	//			//structureAgent.Points =
	//		}
	//
	//		public override void Discard (Utilities uts)
	//		{
	//
	//		}
	//
	//		public override void Approve (Utilities uts, int iteration)
	//		{
	//			hasSpace.Iteration = iteration;
	//		}
	//
	//	}
}


