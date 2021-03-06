using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const string TAG_PLAYER = "Player";
    public const string TAG_FEUMAN = "Follower";
    public const string TAG_MOVABLE = "MovableObject";
    public const string TAG_GROUND = "Ground";
    public const string TAG_DOOR = "Door";

    public static GameManager instance = null;

    public int nbrTorchLighted = 0;

    public GameObject portal;

    [SerializeField] private float resetLevelCooldown = 60f;
    private float resetLevelTimer;
    public SceneFader sceneFader;
    [SerializeField] private string menuName;

    LevelEnding _lvlEnd;


    private void Awake()
    {
        //singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //reset level
        resetLevelTimer = resetLevelCooldown;

        //level ending
        GameObject lvlEnd = GameObject.Find("LevelEndingCanvas");
        if (lvlEnd)
            _lvlEnd = lvlEnd.GetComponent<LevelEnding>();
    }

    private void Update()
    {
        // camera slider
        if (Input.GetKeyDown("c"))
            StartCoroutine(GetComponent<CameraManager>().CamSlide());

        // level reset
        if (Input.anyKey)
            resetLevelTimer = resetLevelCooldown;

        if (sceneFader && resetLevelTimer < 0)
            sceneFader.FadeTo(menuName);

        resetLevelTimer -= Time.deltaTime;

        // level ending test
        if (Input.GetKeyDown(KeyCode.E))
            SpawnEndingPortal();
    }

    //Light torch
    public void CountTorch()
    {
        nbrTorchLighted += 1;
        StartCoroutine(GetComponent<CameraManager>().CamSlide());

        if (nbrTorchLighted == 3)
            SpawnEndingPortal();
    }

    public void SpawnEndingPortal()
    {
        Instantiate(portal, new Vector3(0, 1, 7), new Quaternion(0, 90, 90, 90));
    }

    public void EndLevel()
    {
        if (_lvlEnd)
            _lvlEnd.Display();
    }
}
