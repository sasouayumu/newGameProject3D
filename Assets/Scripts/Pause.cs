using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ポーズを出すクラス
public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;
   

    void Update()
    {
        //タイムスケールがゼロの場合は処理をしない
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        //EscapeKeyを押したらポーズ画面を起動する
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
        }
    }


    public void pause()
    {
        //ポーズ画面を出し、時間を止める
        pauseUI.SetActive(!pauseUI.activeSelf);
        if (pauseUI.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
