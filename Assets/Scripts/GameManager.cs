using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Level Looping")]

    public bool IsGameLooping;

    private int LevelCounter;

    public List<GameObject> LevelObjects;
    [HideInInspector]
    public GameObject Currentlevel;

    [Header("UI")]

    public GameObject EndPanel;

    public GameObject NextButton;

    public GameObject RetryButton;

    public TextMeshProUGUI EndText;

    public Image EmojiImage;

    public List<string> SuccessStrings;

    public List<Sprite> SuccessEmojis;

    public List<string> FailStrings;

    public List<Sprite> FailEmojis;

    public List<GameObject> DeactivatingUIElements;

    private int GameLevel;

    public TextMeshProUGUI LevelText;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (IsGameLooping)
        {
            if (PlayerPrefs.HasKey("LevelCounter"))
            {
                LevelCounter = PlayerPrefs.GetInt("LevelCounter");
                GameLevel = PlayerPrefs.GetInt("GameLevel");
            }
            else
            {
                PlayerPrefs.SetInt("LevelCounter", 0);
                LevelCounter = 0;
                PlayerPrefs.SetInt("GameLevel", 0);
                GameLevel = 0;
            }

            StartLevelLoad();
        }

    }

    private void StartLevelLoad()
    {
        GameObject level = Instantiate(LevelObjects[LevelCounter], Vector3.zero, Quaternion.identity);

        Currentlevel = level;

        LevelText.text = "LEVEL\n" + (GameLevel + 1).ToString();
        LevelText.gameObject.SetActive(true);

    }

    public void LevelSucceded()
    {
        Invoke("LevelSuccededLate", 2f);
    }

    private void LevelSuccededLate()
    {
        EndPanel.SetActive(true);
        NextButton.SetActive(true);

        EmojiImage.sprite = SuccessEmojis[Random.Range(0, SuccessEmojis.Count)];

        EndText.text = SuccessStrings[Random.Range(0, SuccessStrings.Count)];
        EndText.color = Color.green;
    }

    public void LevelFailed()
    {
        Invoke("LevelFailedLate", 2f);
    }

    private void LevelFailedLate()
    {
        EndPanel.SetActive(true);
        RetryButton.SetActive(true);

        EmojiImage.sprite = FailEmojis[Random.Range(0, FailEmojis.Count)];

        EndText.text = FailStrings[Random.Range(0, FailStrings.Count)];
        EndText.color = Color.red;
    }

    public void NextLevelLoad()
    {
        if (Currentlevel != null)
        {
            Destroy(Currentlevel);
        }

        LevelCounter++;
        if (LevelCounter >= LevelObjects.Count)
        {
            LevelCounter = 0;
        }

        PlayerPrefs.SetInt("LevelCounter", LevelCounter);

        GameLevel++;

        PlayerPrefs.SetInt("GameLevel", GameLevel);

        LevelText.text = "LEVEL\n" + (GameLevel + 1).ToString();

        LevelText.gameObject.SetActive(true);

        ResetScene();

        GameObject level = Instantiate(LevelObjects[LevelCounter], Vector3.zero, Quaternion.identity);

        Currentlevel = level;

    }

    public void RestartScene()
    {
        if (Currentlevel != null)
        {
            Destroy(Currentlevel);
        }

        ResetScene();

        GameObject level = Instantiate(LevelObjects[LevelCounter], Vector3.zero, Quaternion.identity);

        Currentlevel = level;

        LevelText.gameObject.SetActive(true);


    }
    
    private void ResetScene()
    {
        
    }
}
