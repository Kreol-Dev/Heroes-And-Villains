using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    // Use this for initialization
    Transform trans;
    Camera camera;
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
            trans.position += Vector3.up;
        }
        if (Input.GetKey (KeyCode.A))
        {
            trans.position += Vector3.left;
        }
        if (Input.GetKey (KeyCode.S))
        {
            trans.position += Vector3.down;
        }
        if (Input.GetKey (KeyCode.D))
        {
            trans.position += Vector3.right;
        }
        if (Input.GetKey (KeyCode.Q))
        {
            camera.orthographicSize += 1;
        }
        if (Input.GetKey (KeyCode.E))
        {
            camera.orthographicSize -= 1;
        }
        if (Input.GetKey (KeyCode.Escape))
            Application.Quit ();
    }
}
