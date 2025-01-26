using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//ボタン関係の処理のクラス
public class ButtonController : MonoBehaviour
{
    //各ボタンの処理
    private Pause pause;


    //タイトルボタン
    public void Title()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }


    //再開ボタン
    public void Restart()
    {
        GameObject pausePanel = GameObject.Find("Pause");
        pause = pausePanel.GetComponent<Pause>();
        pause.pause();
    }


    //プレイボタン
    public void Play()
    {
        SceneManager.LoadScene("Stage1");
    }


    //チュートリアルボタン
    public void Tutorial()
    {
        SceneManager.LoadScene(4);
    }


    //設定ボタン
    public void Setting()
    {
        SceneManager.LoadScene(3);
    }


    //クレジット表記ボタン
    public void Cregits()
    {
        SceneManager.LoadScene(5);
    }


    //やめるボタン
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
