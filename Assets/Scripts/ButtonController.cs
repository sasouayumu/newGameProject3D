using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonController : MonoBehaviour
{
    private Pause pause;
    public void Title()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    public void Restart()
    {
        GameObject pausePanel = GameObject.Find("Pause");
        pause = pausePanel.GetComponent<Pause>();
        pause.pause();
    }

    public void Play()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(4);
    }

    public void Setting()
    {
        SceneManager.LoadScene(3);
    }

    public void Cregits()
    {
        SceneManager.LoadScene(5);
    }
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
