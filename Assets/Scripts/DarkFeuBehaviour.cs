using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFeuBehaviour : MonoBehaviour
{
    private ButtonBehaviour Button;
    bool darkWorld;

    private void Start()
    {
        Button = GetComponent<ButtonBehaviour>();
        darkWorld = GameManager.instance.darkWorld;
    }

    void Update()
    {
        if (Button.on && GameManager.instance.darkWorld == false)
        {
            GameManager.instance.ScreenFade();
        }
    }
}
