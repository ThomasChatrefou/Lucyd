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
    private List<Button> buttons = new List<Button>();
    private List<PressPlate> presses = new List<PressPlate>();
    private List<LeverBehaviour> levers = new List<LeverBehaviour>();
    private List<LogicGate> gates = new List<LogicGate>();

    void Start()
    {
        foreach (GameObject obj in inputs)
        {
            if (obj != null)
            {
                if (obj.CompareTag("Button"))
                    buttons.Add(obj.GetComponent<Button>());

                if (obj.CompareTag("Press"))
                    presses.Add(obj.GetComponent<PressPlate>());

                if (obj.CompareTag("Lever"))
                    levers.Add(obj.GetComponent<LeverBehaviour>());

                if (obj.CompareTag("LogicGate"))
                    gates.Add(obj.GetComponent<LogicGate>());
            }
        }
    }

    void Update()
    {
        andButtons = true;
        andPresses = true;
        leversSum = 0;

        if (buttons.Count > 0)
        {
            foreach (Button button in buttons)
                andButtons = andButtons && button.On;
        }
        if (presses.Count > 0)
        {
            foreach (PressPlate press in presses)
                andPresses = andPresses && press.On;
        }

        if(andButtons && andPresses)
        {
            output = 1;

            if (levers.Count + gates.Count > 0)
            {
                foreach (LeverBehaviour lever in levers)
                    leversSum += lever.Percent;

                foreach (LogicGate gate in gates)
                    leversSum += gate.output;

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
