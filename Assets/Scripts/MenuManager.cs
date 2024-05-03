using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.PlayerLoop;

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
    private bool isLoadMain;
    private float transitionTimer;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "" + highScore;
        Screen.SetResolution(936, 527, false);

    }

    void Update()
    {
        if (isLoadMain)
        {
            transitionTimer -= Time.deltaTime;

            if (transitionTimer <= 0)
            {
                SceneManager.LoadScene("MainScene");
            }
        }

    }

    public void onPlayClick()
    {
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
