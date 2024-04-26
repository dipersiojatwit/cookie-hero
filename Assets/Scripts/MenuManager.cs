using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "" + highScore;
        Screen.SetResolution(936, 527, false);

    }

    public void onPlayClick()
    {
        SceneManager.LoadScene("MainScene");
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
