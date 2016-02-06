using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;

namespace CoreMod
{
	public abstract class GOLayerPresenter<TComponent, TLayerObject, TLayer> : BaseMapLayerPresenter<TComponent, TLayer, TiledObjectsCollectionInteractor<GameObject, GOCollection>>
		where TLayer : class, IMapLayer, ITiledMapLayer<TLayerObject, GameObject> where TComponent : EntityComponent where TLayerObject : Object
	{

		Dictionary<TComponent, ObjectPresenter<TComponent>> hovered = new Dictionary<TComponent, ObjectPresenter<TComponent>> ();
		Dictionary<TComponent, ObjectPresenter<TComponent>> selected = new Dictionary<TComponent, ObjectPresenter<TComponent>> ();
		Stack<ObjectPresenter<TComponent>> freePresenters = new Stack<ObjectPresenter<TComponent>> ();

		ObjectPresenter<TComponent> GetFreePresenter ()
		{
			ObjectPresenter<TComponent> presenter;
			if (freePresenters.Count > 0)
				presenter = freePresenters.Pop ();
			else
				presenter = NewPresenter ();


			presenter.HideObjectDesc ();
			presenter.HideObjectShortDesc ();
			return presenter;
		}

		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				Interactor.ObjectHovered += OnHover;
				Interactor.ObjectDeHovered += OnDeHover;
				Interactor.ObjectSelected += OnSelect;
				Interactor.ObjectDeSelected += OnDeSelect;
				Layer.ObjectChanged += OnObjectChanged;
				break;
			case RepresenterState.NotActive:
				break;
			}
		}

		protected abstract TComponent ComponentFromLayerObject (TLayerObject obj);

		void OnHover (GameObject go)
		{
			TComponent cmp = go.GetComponent<TComponent> ();
			if (cmp == null)
				return;
			if (hovered.ContainsKey (cmp))
				return;
			ObjectPresenter<TComponent> presenter = GetFreePresenter ();
			presenter.ShowObjectShortDesc (cmp);
			hovered.Add (cmp, presenter);
		}


		void OnDeHover (GameObject go)
		{
			TComponent cmp = go.GetComponent<TComponent> ();
			if (cmp == null)
				return;
			if (!hovered.ContainsKey (cmp))
				return;
			var presenter = hovered [cmp];
			hovered.Remove (cmp);

			presenter.HideObjectShortDesc ();
			freePresenters.Push (presenter);
			
		}

		void OnSelect (GameObject go)
		{
			TComponent cmp = go.GetComponent<TComponent> ();
			if (cmp == null)
				return;
			if (selected.ContainsKey (cmp))
				return;
			ObjectPresenter<TComponent> presenter = GetFreePresenter ();
			presenter.ShowObjectDesc (cmp);
			selected.Add (cmp, presenter);
		}

		void OnDeSelect (GameObject go)
		{
			TComponent cmp = go.GetComponent<TComponent> ();
			if (cmp == null)
				return;
			if (!selected.ContainsKey (cmp))
				return;
			var presenter = selected [cmp];
			presenter.HideObjectDesc ();
			selected.Remove (cmp);
			freePresenters.Push (presenter);
		}

		void OnObjectChanged (TLayerObject obj)
		{
			TComponent cmp = ComponentFromLayerObject (obj);
			if (cmp == null)
				return;
			if (selected.ContainsKey (cmp))
			{
				selected [cmp].Update ();
			}

			if (hovered.ContainsKey (cmp))
			{
				hovered [cmp].Update ();
			}
		}
	}

	public class ComponentLayerPresenter<TComponent, TLayer> : GOLayerPresenter<TComponent, TComponent, TLayer>
		where TLayer : class, IMapLayer, ITiledMapLayer<TComponent, GameObject> where TComponent : EntityComponent
	{
		protected override TComponent ComponentFromLayerObject (TComponent obj)
		{
			return obj;
		}
		
	}

	public class ConvertedLayerPresenter<TComponent> : GOLayerPresenter<TComponent, GameObject, DefaultGOLayer>
		where TComponent : EntityComponent
	{
		protected override TComponent ComponentFromLayerObject (GameObject obj)
		{
			return obj.GetComponent<TComponent> ();
		}
	}

	public abstract class GOLayerRenderer<TComponent, TLayer> : BaseMapLayerRenderer<TLayer, GOCollection> where TLayer : MapLayer<GOCollection>, ITiledMapLayer<TComponent, GameObject> where TComponent : EntityComponent
	{
		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				
				break;
			case RepresenterState.NotActive:
				break;
			}
		}

		protected override void Setup (Demiurg.Core.Extensions.ITable definesTable)
		{
			
		}
		
		
	}

	public class DefaultRenderer : IMapLayerRenderer
	{
		public void Setup (IMapLayer layer, RepresenterState defaultState)
		{
		}

		public void ChangeState (RepresenterState state)
		{
		}
		
	}


	public class DefaultPresenter : IMapLayerPresenter
	{
		public void Setup (IMapLayer layer, IMapCollectionInteractor interactor, System.Type objectPresenter, RepresenterState defaultState)
		{
		}

		public void ChangeState (RepresenterState state)
		{
		}
		
	}
}
