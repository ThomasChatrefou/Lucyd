using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFeuBehaviour : MonoBehaviour
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
        if (Button.on && GameManager.instance.darkWorld)
        {
            GameManager.instance.ScreenFade();
        }
    }
}
