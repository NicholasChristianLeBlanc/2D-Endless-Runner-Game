using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject highScoreText;

    private int currentScore;
    private TextMeshProUGUI scoretxt;
    private TextMeshProUGUI highScoretxt;

    // Start is called before the first frame update
    void Start()
    {
        currentScore = playerCharacter.GetComponent<PlayerMovement>().GetScore();
        scoretxt = scoreText.GetComponent<TextMeshProUGUI>();
        highScoretxt = highScoreText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        currentScore = playerCharacter.GetComponent<PlayerMovement>().GetScore();

        scoretxt.SetText("Score: " + currentScore.ToString());
        highScoretxt.SetText("Hi-Score: " + PlayerPrefs.GetInt("highScore"));
    }
}
