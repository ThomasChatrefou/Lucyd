using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int nbrTorchLighted = 0;

    public GameObject portal;

    [SerializeField] private float resetLevelCooldown = 60f;
    private float resetLevelTimer;
    public SceneFader sceneFader;
    [SerializeField] private string menuName;


    private void Awake()
    {
        //singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //reset level
        resetLevelTimer = resetLevelCooldown;
    }

    private void Update()
    {
        // camera slider
        if (Input.GetKeyDown("c"))
        {
            StartCoroutine(GetComponent<CameraManager>().CamSlide());
        }

        // level reset
        if (Input.anyKey)
        {
            resetLevelTimer = resetLevelCooldown;
        }

        if (resetLevelTimer < 0)
        {
            sceneFader.FadeTo(menuName);
        }

        resetLevelTimer -= Time.deltaTime;
    }

    //Light torch
    public void CountTorch()
    {
        nbrTorchLighted += 1;
        StartCoroutine(GetComponent<CameraManager>().CamSlide());
        if(nbrTorchLighted == 3)
        {
            Instantiate(portal, new Vector3(0,1,7),new Quaternion(0,90,90,90));
            print("a portal has open");
        }
    }
}
