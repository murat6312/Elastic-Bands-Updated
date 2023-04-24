using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public int level;
    //public Text levelText;
    void Start()
    {
        //levelText.text = level.ToString(); 
    }

    public void openScene()
    {
        SceneManager.LoadScene("Level " + level.ToString());
    }
}
