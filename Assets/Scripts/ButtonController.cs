using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonController : MonoBehaviour
{
    Pause pause;
    public void Title()
    {
        SceneManager.LoadScene(3);
    }

    public void Restart()
    {
        GameObject pausePanel = GameObject.Find("Pause");
        pause = pausePanel.GetComponent<Pause>();
        pause.pause();
    }

    public void Play()
    {
        SceneManager.LoadScene(0);
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
