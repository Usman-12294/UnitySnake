using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Obi;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager _Instance;
    public ObiSolver solver;
    public ObiRope rope;
    private float _snakeCurrentLength;
    public float snakeStartingLength;

    public GameObject SnakePlayer;
    private bool isCheckpoint = false;
    private Vector3 CheckpointPosition = Vector3.zero;
    public static bool FirstTime = true;
    public GameObject LoadingPanel;

    public float snakeCurrentLength {
        get {
            return
                rope.CalculateLength();
        }
    }

    private void Awake() {
        Application.targetFrameRate = 60;
        if (_Instance == null) {
            _Instance = this;
        }

        PopulateLevels();

        

        if(FirstTime){ 
		StartCoroutine(RestartScene(1.0f));
		FirstTime = false;
	 }else{
		LoadingPanel.SetActive(false);
	
	}
    }

    private void Start() {
        
        isCheckpoint = false;
        CheckpointPosition = Vector3.zero;
        SumPooler = 0;
        CollectiblesCounter = 0;
        InitializeLevel();
        snakeStartingLength = rope.restLength;
    }

    [Header("Collectibles")] public Collectibles[] gameCollectibles;
    public GameObject[] SumUp;
    public Text CollectiblesCount;
    private int SumPooler;
    public int CollectiblesCounter;
    public GameObject ExplodedParticles;


    [Header("Level Stats")] [SerializeField]
    private GameObject StartLevel;

    [SerializeField] private GameObject EndLevel;


    [Header("Level Complete")] [SerializeField]
    private GameObject WinPanel;

    [SerializeField] private GameObject FailPanel;

    [SerializeField] private Image[] Fruits;
    [SerializeField] private Text[] FruitsCount;

    #region Level Manager

    [Header("\nLevel manager\n")] public int TotalLevels;
    [SerializeField] private GameObject LevelsContainer;
    [SerializeField] private GameObject[] Levels;
    [SerializeField] private GameObject[] Hurdles;
    [SerializeField] private GameObject[] Boundaries;
    [SerializeField] private int DesiredLevel;
    [SerializeField] private GameObject[] Controls;
    public bool vibrationHaptic = true;


    public void ShowSumUp(Vector3 position) {
        SumUp[SumPooler].transform.position = position;
        SumUp[SumPooler].SetActive(true);
        SumPooler++;
        if (SumPooler >= 4)
            SumPooler = 0;
    }

    public void UpdateCounter() {
        SnakePlayer.GetComponent<HeadControllerV2>().animator.SetTrigger("eat");
        CollectiblesCounter++;
        CollectiblesCount.text = CollectiblesCounter.ToString();
    }

    public void IncreaseLength() {
        //rope.GetComponent<RopeExtender>().IncreaseLength();
        StartCoroutine(SnakePlayer.GetComponent<HeadControllerV2>().EmulateBodyBump());
    }


    public void ExplodeDoor(Vector3 ExplosionPosition) {
        ExplodedParticles.transform.position = ExplosionPosition;
        ExplodedParticles.SetActive(true);
        ExplodedParticles.GetComponent<poolObject>().explode();
    }

    void PopulateLevels() {
        TotalLevels = LevelsContainer.transform.childCount;
        Levels = new GameObject[TotalLevels];

        for (int i = 0; i < TotalLevels; i++) {
            Levels[i] = LevelsContainer.transform.GetChild(i).gameObject;
        }
    }


    void InitializeLevel() {
        Time.timeScale = 1.0f;
        CollectiblesCount.text = CollectiblesCounter.ToString();
        if (DesiredLevel > 0) {
            StartLevel.GetComponentInChildren<Text>().text = "LEVEL  " + DesiredLevel.ToString();
//            EndLevel.GetComponentInChildren<Text>().text = (DesiredLevel + 1).ToString();
            Levels[DesiredLevel - 1].SetActive(true);
            int TempHurdlesCount = Levels[DesiredLevel - 1].transform.GetChild(0).transform.childCount;
            Hurdles = new GameObject[TempHurdlesCount];
            for (int i = 0; i < TempHurdlesCount; i++) {
                Hurdles[i] = Levels[DesiredLevel - 1].transform.GetChild(0).transform.GetChild(i).gameObject;
            }
        }
        else {
            if (PlayerPrefs.GetInt("CurrentLevel") >= TotalLevels) {
                PlayerPrefs.SetInt("CurrentLevel", 0);
            }
            StartLevel.GetComponentInChildren<Text>().text =
                "LEVEL  " + (PlayerPrefs.GetInt("CurrentLevel", 0) + 1).ToString();
//            EndLevel.GetComponentInChildren<Text>().text = (PlayerPrefs.GetInt("CurrentLevel", 0) + 2).ToString();
            Levels[PlayerPrefs.GetInt("CurrentLevel", 0)].SetActive(true);
            int TempHurdlesCount = Levels[PlayerPrefs.GetInt("CurrentLevel", 0)].transform.GetChild(0).transform
                .childCount;
            Hurdles = new GameObject[TempHurdlesCount];
            for (int i = 0; i < TempHurdlesCount; i++) {
                Hurdles[i] = Levels[PlayerPrefs.GetInt("CurrentLevel", 0)].transform.GetChild(0).transform.GetChild(i)
                    .gameObject;
            }
        }
    }

    public void StartControls() {
        for (int i = 0; i < Controls.Length; i++) {
            Controls[i].SetActive(true);
        }
    }

    public void SaveCheckpoint(Vector3 NewSpawnpoint) {
        isCheckpoint = true;
        CheckpointPosition = NewSpawnpoint;
    }


    public void LevelComplete() {
        WinPanel.SetActive(true);
//..        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, PlayerPrefs.GetInt("CurrentLevel").ToString());
        if (DesiredLevel > 0) {
            EndLevel.GetComponentInChildren<Text>().text = "LEVEL  " + DesiredLevel.ToString();
        }
        else {
            EndLevel.GetComponentInChildren<Text>().text = "LEVEL  " + (PlayerPrefs.GetInt("CurrentLevel", 0) + 1);
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
        }

//        SnakePlayer.GetComponent<HeadController>().AutoMovement = 1;
        SnakePlayer.GetComponent<HeadControllerV2>().AutoMovement = 1;
        for (int i = 0; i < Controls.Length; i++) {
            Controls[i].GetComponent<Image>().enabled = false;
        }

        for (int i = 0; i < gameCollectibles.Length; i++) {
            Fruits[i].sprite = gameCollectibles[i].picture;
            FruitsCount[i].text = gameCollectibles[i].count.ToString();
        }

        StartCoroutine(GameOver(2.0f));
    }


    public void RunGameOver() {
        StartCoroutine(GameOver(2.0f));
    }

    IEnumerator GameOver(float timer) {
        yield return new WaitForSeconds(timer);
        Time.timeScale = 0.0f;
    }


    public void NextLevel() {
        SceneManager.LoadScene(0);
    }

    public IEnumerator RestartLevelAfterTime(float time) {
        yield return new WaitForSeconds(time);
        RestartLevel();
    }

    public void RestartLevel() {
        FailPanel.SetActive(true);
//        FailPanel.GetComponent<DOTweenAnimation>().DORestartById("0");
        if (isCheckpoint) {
            foreach (var hurdles in Hurdles) {
//                hurdles.SetActive(false);
                hurdles.transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
            }

            foreach (var boundary in Boundaries) {
                boundary.SetActive(false);
            }

            StartCoroutine(GetBack(2.0f));
        }
        else {
            Debug.Log("Loading Scene");
            StartCoroutine(RestartScene(1.0f));
        }
    }

    IEnumerator RestartScene(float ReturnTime) {
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene("Gameplay");
    }

    IEnumerator GetBack(float ReturnTime) {
        yield return new WaitForSeconds(1.0f);
        rope.GetComponent<ObiParticleAttachment>().attachmentType = ObiParticleAttachment.AttachmentType.Static;
        rope.RebuildConstraintsFromElements();
        SnakePlayer.GetComponent<Rigidbody>().isKinematic = true;
        SnakePlayer.transform.position = CheckpointPosition;
        yield return new WaitForSeconds(ReturnTime);
//        FailPanel.GetComponent<DOTweenAnimation>().DORestartById("1");
        SnakePlayer.GetComponent<Rigidbody>().isKinematic = false;
        SnakePlayer.GetComponent<HeadControllerV2>().enabled = true;
        rope.GetComponent<ObiParticleAttachment>().attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
        rope.RebuildConstraintsFromElements();
        FailPanel.SetActive(false);
        foreach (var hurdles in Hurdles) {
//            hurdles.SetActive(true);
            hurdles.transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
        }

        foreach (var boundary in Boundaries) {
            boundary.SetActive(true);
        }
    }

    #endregion

    private void InitCallback() {
        //if (FB.IsInitialized) {
            // Signal an app activation App Event
          //  FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
       // }
       // else {
         //   Debug.Log("Failed to Initialize the Facebook SDK");
        //}
    }

    private void OnHideUnity(bool isGameShown) {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
}


[Serializable]
public class Collectibles {
    public String name;
    public int count;
    public Sprite picture;
}