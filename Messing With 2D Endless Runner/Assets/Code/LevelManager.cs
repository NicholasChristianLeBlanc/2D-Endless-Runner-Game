using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject highScoreText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuText;
    [SerializeField] private Button play;
    [SerializeField] private Button quit;

    private int currentScore;
    private int deathCounter = 0;
    private TextMeshProUGUI scoretxt;
    private TextMeshProUGUI highScoretxt;
    private GameObject[] killBlocks;

    // Start is called before the first frame update
    void Start()
    {
        currentScore = playerCharacter.GetComponent<PlayerMovement>().GetScore();
        scoretxt = scoreText.GetComponent<TextMeshProUGUI>();
        highScoretxt = highScoreText.GetComponent<TextMeshProUGUI>();

        play.onClick.AddListener(Play);
        quit.onClick.AddListener(Exit);

        Time.timeScale = 0;
        menu.SetActive(true);
        play.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCharacter.GetComponent<PlayerMovement>().GetLives() <= 0) 
        {
            deathCounter++;

            playerCharacter.GetComponent<PlayerMovement>().Disable();

            killBlocks = GameObject.FindGameObjectsWithTag("KillBlock");

            foreach (GameObject block in killBlocks)
            {
                Destroy(block);
            }

            if (deathCounter >= 50) 
            { 
                Time.timeScale = 0;

                playerCharacter.GetComponent<PlayerMovement>().Reset();
                menuText.GetComponent<TextMeshProUGUI>().SetText("Game Over");

                menu.SetActive(true); 
                play.gameObject.SetActive(true);
                quit.gameObject.SetActive(true);
            }
        }
        else
        {
            currentScore = playerCharacter.GetComponent<PlayerMovement>().GetScore();

            scoretxt.SetText("Score: " + currentScore.ToString());
            highScoretxt.SetText("Hi-Score: " + PlayerPrefs.GetInt("highScore"));
        }
    }

    void Play()
    {
        Time.timeScale = 3;

        menu.SetActive(false);
        play.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);

        playerCharacter.GetComponent<PlayerMovement>().Enable();
    }

    void Exit()
    {
        Application.Quit();
    }
}
