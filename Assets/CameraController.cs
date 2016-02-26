using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	// Use this for initialization
	Transform trans;
	Camera camera;
	public float MoveSpeed;
	[Range (1f, 100)]
	public float ScrollSpeed;

	void Start ()
	{
		trans = transform;
		camera = gameObject.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.W))
		{
			trans.position += Vector3.up * Time.deltaTime * MoveSpeed * camera.orthographicSize;
		}
		if (Input.GetKey (KeyCode.A))
		{
			trans.position += Vector3.left * Time.deltaTime * MoveSpeed * camera.orthographicSize;
		}
		if (Input.GetKey (KeyCode.S))
		{
			trans.position += Vector3.down * Time.deltaTime * MoveSpeed * camera.orthographicSize;
		}
		if (Input.GetKey (KeyCode.D))
		{
			trans.position += Vector3.right * Time.deltaTime * MoveSpeed * camera.orthographicSize;
		}
		if (Input.GetKey (KeyCode.Q))
		{
			camera.orthographicSize *= 1f + ScrollSpeed / 100f;
		}
		if (Input.GetKey (KeyCode.E))
		{
			camera.orthographicSize /= 1f + ScrollSpeed / 100f;
		}
		if (Input.GetKey (KeyCode.Escape))
			Application.Quit ();
	}
}
