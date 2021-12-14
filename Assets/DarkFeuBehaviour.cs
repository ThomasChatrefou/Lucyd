using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFeuBehaviour : MonoBehaviour
{
    private ButtonBehaviour Button;

    private void Start()
    {
        Button = GetComponent<ButtonBehaviour>();
    }

    void Update()
    {
        if (Button.on && GameManager.instance.darkWorld == false)
        {
            GameManager.instance.ScreenFade();
        }
        else if (Button.on == false && GameManager.instance.darkWorld)
        {
            GameManager.instance.ScreenFade();
        }
    }
}
