using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject startScreenPanel;

    void Start()
    {
        Time.timeScale = 0;
        startScreenPanel.SetActive(true);
    }

    public void StartGame()
    {
        startScreenPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
