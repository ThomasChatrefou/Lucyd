using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : MonoBehaviour
{
    public GameObject[] inputs;
    public float output = 0;

    private bool andButtons;
    private bool andPresses;
    private float leversSum;
    private List<ButtonBehaviour> buttons = new List<ButtonBehaviour>();
    private List<PressBehaviour> presses = new List<PressBehaviour>();
    private List<LeverBehaviour> levers = new List<LeverBehaviour>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in inputs)
        {
            if (obj != null)
            {
                if (obj.CompareTag("Button"))
                    buttons.Add(obj.GetComponent<ButtonBehaviour>());

                if (obj.CompareTag("Press"))
                    presses.Add(obj.GetComponent<PressBehaviour>());

                if (obj.CompareTag("Lever"))
                    levers.Add(obj.GetComponent<LeverBehaviour>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        andButtons = true;
        andPresses = true;
        leversSum = 0;

        if (buttons.Count > 0)
        {
            foreach (ButtonBehaviour button in buttons)
                andButtons = andButtons && button.on;
        }
        if (presses.Count > 0)
        {
            foreach(PressBehaviour press in presses)
                andPresses = andPresses && press.on;
        }

        if(andButtons && andPresses)
        {
            output = 1;

            if (levers.Count > 0)
            {
                foreach(LeverBehaviour lever in levers)
                    leversSum += lever.percent;

                if (leversSum > 1)
                    leversSum = 1;
                else if (leversSum < 0)
                    leversSum = 0;

                output *= leversSum;
            }
        }
        else
            output = 0;
    }
}
