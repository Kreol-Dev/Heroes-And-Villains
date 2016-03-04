using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public class Form
	{
		public delegate void FormDelegate (Form form);

		public event FormDelegate FormUpdated;

		protected void OnFormUpdated ()
		{
			if (FormUpdated != null)
				FormUpdated (this);
		}
	}

	public class CircleForm : Form
	{
		public Vector2 Center {
			get { return center; }
			set
			{
				center = value;
				OnFormUpdated ();
			}
		}

		public float Radius {
			get { return radius; }
			set
			{
				radius = value;
				OnFormUpdated ();
			}
		}

		Vector2 center;
		float radius;

		public CircleForm (Vector2 center, float radius)
		{
			this.center = center;
			this.radius = radius;
		}
	}

	public class RectForm : Form
	{
		public Vector2 Center {
			get { return center; }
			set
			{
				center = value;
				OnFormUpdated ();
			}
		}

		public Vector2 Size {
			get { return size; }
			set
			{
				size = value;
				OnFormUpdated ();
			}
		}

		Vector2 center;
		Vector2 size;

		public RectForm (Vector2 center, Vector2 size)
		{
			this.center = center;
			this.size = size;
		}
	}

	public class PolygonForm : Form
	{
		Vector2[] corners;

		public PolygonForm (Vector2[] corners)
		{
			this.corners = corners;
		}

		public IEnumerable<Vector2> GetCorners ()
		{
			return corners;
		}

		public void SetCorners (Vector2[] corners)
		{
			this.corners = corners;
			OnFormUpdated ();
		}
	}

	public class DotsForm : Form
	{
		Vector2[] dots;

		public DotsForm (Vector2[] dots)
		{
			this.dots = dots;
		}

		public IEnumerable<Vector2> GetDots ()
		{
			return dots;
		}

		public void SetDots (Vector2[] dots)
		{
			this.dots = dots;
			OnFormUpdated ();
		}
	}
}


