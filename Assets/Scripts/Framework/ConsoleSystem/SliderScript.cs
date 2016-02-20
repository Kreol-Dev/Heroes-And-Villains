using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField]
    MessagesScript messages;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {       

    }

    public void Change_view()
    {
        messages.ShowFrom(this.GetComponent<Slider>().value);
    }

    public void scrolUP()
    {
        if (messages.slider >= 1)
        {
            messages.slider--;
            this.GetComponent<Slider>().value -= 1 / messages.ShownMessages.Count;
            messages.ShowMessagePool();
        }

    }
    public void scrolDown()
    {
        if (messages.slider < messages.ShownMessages.Count - 6)
        {
            messages.slider++;
            this.GetComponent<Slider>().value += 1 / messages.ShownMessages.Count;
            messages.ShowMessagePool();

        }

    }
}