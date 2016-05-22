using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UAI
{
	public class Modifier
	{
		List<object> aspects = new List<object> ();

		public void ApplyTo (GameObject go)
		{
			for (int i = 0; i < aspects.Count; i++)
			{
				IEffectAspect aspect = aspects [i] as IEffectAspect;
				aspect.ApplyTo (go);
			}
		}

		public void Reverse (GameObject go)
		{
			for (int i = 0; i < aspects.Count; i++)
			{
				IModifierAspect aspect = aspects [i] as IModifierAspect;
				aspect.Reverse (go);
			}
		}
	}

	public abstract class Effect
	{
		List<object> aspects = new List<object> ();

		public abstract void ApplyTo (GameObject go);
	}

	public class SimpleEffect : Effect
	{
		List<object> aspects = new List<object> ();

		public override void ApplyTo (GameObject go)
		{
			for (int i = 0; i < aspects.Count; i++)
			{
				IEffectAspect aspect = aspects [i] as IEffectAspect;
				aspect.ApplyTo (go);
			}
		}
	}

	public interface IEffectAspect
	{
		void ApplyTo (GameObject go);
	}

	public interface IModifierAspect
	{
		void Reverse (GameObject go);
	}

	public abstract class ModifierAspect<C, T, P> : EffectAspect<C, T, P>, IModifierAspect where C : Component where P : Promise
	{
		public void Reverse (GameObject go)
		{
			C cmp = null;
			cmp = go.GetComponent<C> ();
			if (cmp != null)
				Reverse (cmp);
		}

		protected abstract void Reverse (C cmp);
	}

	public abstract class EffectAspect<C, T, P> : IEffectAspect where C : Component where P : Promise
	{
		public void ApplyTo (GameObject go)
		{
			C cmp = null;
			cmp = go.GetComponent<C> ();
			if (cmp != null)
				ApplyTo (cmp);
		}

		protected abstract void ApplyTo (C cmp);

		protected T AspectValue;

	}
}
