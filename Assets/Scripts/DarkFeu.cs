using UnityEngine;


public class DarkFeu : MonoBehaviour
{
    [HideInInspector] public bool On = false;

    [SerializeField] private Button LightWorldButton;
    [SerializeField] private Button DarkWorldButton;


    void Start()
    {
        LightWorldButton.Toggled += Toggle;
        DarkWorldButton.Toggled += Toggle;
    }

    void OnDestroy()
    {
        LightWorldButton.Toggled -= Toggle;
        DarkWorldButton.Toggled -= Toggle;
    }

    private void Toggle()
    {
        On = !On;
    }
}
