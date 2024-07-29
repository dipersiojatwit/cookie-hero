using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{   
    // singlton
    private static MenuManager _instance = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
    }

    public static MenuManager instance()
    {
        return _instance;

    }

    public TMP_Text highScoreText;
    public static int highScore = 0;
    public Animator animator;
    public Button playButton;
    private bool isLoadMain;
    private float transitionTimer;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "" + highScore;
        highScoreText.alpha = 0;
        transitionTimer = 0.3f;
        playButton.enabled = false;
        Screen.SetResolution(936, 527, false);

    }

    // Update is called once per frame
    void Update()
    {   
        // Check if main scene should be loaded after transition
        if (isLoadMain)
        {  
            transitionTimer -= Time.deltaTime;

            // Check if the high score text should be transparent
            if (transitionTimer <= 0.4)
            {   
                highScoreText.alpha = 0;

            }

            // Check if the circle transition is done
            if (transitionTimer <= 0)
            {
                SceneManager.LoadScene("MainScene");

            }
        }
        else
        {   
            transitionTimer -= Time.deltaTime;

            if (transitionTimer <= 0)
            {
                highScoreText.alpha = 255;
                playButton.enabled = true;

            }

        }

    }

    /// <summary>
    /// Triggers the main scene load
    /// </summary>
    public void onPlayClick()
    {   
        // Make sure the play button can't be clicked repeatedly
        playButton.enabled = false;
        animator.SetBool("isFadeIn", true);
        transitionTimer = .9f;
        isLoadMain = true;

    }

    /// <summary>
    /// Updates the high score display in the menu
    /// </summary>
    /// <param name="score">The score from the main scene</param>
    /// <returns>A bool representing if the score was the high score</returns>
    public bool updateHighScore(int score)
    {   
        // Check if the score is a new high score
        if (score > highScore)
        {
            highScore = score;
            return true;

        }
        return false;

    }

}
