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

    void Update()
    {   
        /*
            animation events are not used here so the game 
            can run in the background
        */

        // check if main scene should be loaded after transition
        if (isLoadMain)
        {  
            transitionTimer -= Time.deltaTime;

            // check if the high score text should be transparent
            if (transitionTimer <= 0.4)
            {   
                highScoreText.alpha = 0;
            }

            // check if the circle transition is done
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

    public void onPlayClick()
    {   
        // make sure the play button can't be clicked repeatedly
        playButton.enabled = false;
        animator.SetBool("isFadeIn", true);
        transitionTimer = .9f;
        isLoadMain = true;

    }

    public bool updateHighScore(int score)
    {   
        if (score > highScore)
        {
            highScore = score;
            return true;
        }
        return false;

    }

}
