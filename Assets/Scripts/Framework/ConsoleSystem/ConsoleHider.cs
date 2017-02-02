using UnityEngine;
using System.Collections;

public class ConsoleHider : MonoBehaviour
{
    public GameObject Console;
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.LeftShift))
            Console.SetActive(Console.activeSelf ? false : true);

    }
}
