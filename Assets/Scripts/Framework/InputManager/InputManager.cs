using UnityEngine;
using System.Collections;
using Signals;

public class InputManager : Root
{
	//public EventsManager Events { get; internal set; }

	/*Signal_Hover hover;
    Signal_LeftClick leftClick;
    Signal_RightClick rightClick;
    Signal_Drag drag;
    Signal_DragEnd dragEnd;
    Signal_DragStop dragStop;
    Signal_DoubleClick doubleClick;
    Signal_WheelScroll wheelScroll;*/
	public delegate void PointDelegate (Vector2 point);

	public delegate void TwoPointDelegate (Vector2 startPoint, Vector2 endPoint);

	public delegate void WheelDirDelegate (WheelScrollDir dir);

	public event PointDelegate Hover;

	public event PointDelegate LeftClick;

	public event PointDelegate RightClick;

	public event TwoPointDelegate Drag;

	public event TwoPointDelegate DragEnd;

	public event TwoPointDelegate DragStop;

	public event PointDelegate DoubleClick;

	public event WheelDirDelegate WheelScroll;


	protected override void CustomSetup ()
	{
		
		//lineRenderer = gameObject.AddComponent<LineRenderer> ();
		//lineRenderer.SetWidth (0.3f, 0.1f);
		//lineRenderer.SetVertexCount (2);
		/*Events = new EventsManager (
            typeof(Signal_Hover), 
            typeof(Signal_Drag), 
            typeof(Signal_WheelScroll),
            typeof(Signal_LeftClick), 
            typeof(Signal_RightClick), 
            typeof(Signal_DragEnd), 
            typeof(Signal_DragStop), 
            typeof(Signal_DoubleClick)
        );
        hover = Events.GetEvent<Signal_Hover> ();
        wheelScroll = Events.GetEvent<Signal_WheelScroll> ();
        leftClick = Events.GetEvent<Signal_LeftClick> ();
        rightClick = Events.GetEvent<Signal_RightClick> ();
        drag = Events.GetEvent<Signal_Drag> ();
        dragEnd = Events.GetEvent<Signal_DragEnd> ();
        dragStop = Events.GetEvent<Signal_DragStop> ();
        doubleClick = Events.GetEvent<Signal_DoubleClick> ();*/
		
		Fulfill.Dispatch ();
	}

	protected override void PreSetup ()
	{
		#region eventsInit
		Hover += x =>
		{
		};
		LeftClick += x =>
		{
		};
		Drag += (x, y) =>
		{
		};
		DragEnd += (x, y) =>
		{
		};
		DragStop += (x, y) =>
		{
		};
		RightClick += x =>
		{
		};
		WheelScroll += x =>
		{
		};
		#endregion
	}

	void Update ()
	{
		Hover ((Vector2)Input.mousePosition);
		if (actualDrag)
		{

			clickEnd = (Vector2)Input.mousePosition;
			Drag (clickStart, clickEnd);
			if (Input.GetMouseButtonUp (0))
			{
				dragTime = 0f;
				DragEnd (clickStart, clickEnd);
				actualDrag = false;
			}
			if (Input.GetMouseButtonUp (1))
			{
				dragTime = 0f;
				DragStop (clickStart, clickEnd);
				actualDrag = false;
			}
		} else if (possibleDrag)
		{
			clickEnd = (Vector2)Input.mousePosition;
			float distance = (clickEnd - clickStart).magnitude;
			dragTime += Time.deltaTime;
			if (dragTime > dragTreshold && distance > dragTresholdDistance)
			{
				actualDrag = true;
				possibleDrag = false;
			}
			if (Input.GetMouseButtonUp (0))
			{
				LeftClick (clickEnd);
				possibleDrag = false;
			}
			if (Input.GetMouseButtonUp (1))
			{
				possibleDrag = false;
			}
		} else if (Input.GetMouseButtonDown (0))
		{
			clickStart = (Vector2)Input.mousePosition;
			possibleDrag = true;
		} else if (Input.GetMouseButtonUp (1))
		{
			RightClick ((Vector2)Input.mousePosition);
		}

		if (Input.mouseScrollDelta.y > Mathf.Epsilon)
		{
			WheelScroll (WheelScrollDir.Up);
		}
		if (Input.mouseScrollDelta.y < -Mathf.Epsilon)
		{
			WheelScroll (WheelScrollDir.Down);
		}
	}

	[SerializeField]
	bool possibleDrag = false;
	[SerializeField]
	bool actualDrag = false;
	[SerializeField]
	float dragTime = 0f;
	[SerializeField]
	float dragTreshold = 0.1f;
	[SerializeField]
	Vector2 clickStart;
	[SerializeField]
	Vector2 clickEnd;
	[SerializeField]
	float dragTresholdDistance = 5f;

	Vector3 LinePoint (Vector2 screenPoint)
	{
		return Camera.main.ScreenToWorldPoint (screenPoint) + Vector3.forward;
	}



  
}

public enum WheelScrollDir
{
	Up,
	Down

}