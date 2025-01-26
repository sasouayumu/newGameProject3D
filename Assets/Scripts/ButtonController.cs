using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//�{�^���֌W�̏����̃N���X
public class ButtonController : MonoBehaviour
{
    //�e�{�^���̏���
    private Pause pause;


    //�^�C�g���{�^��
    public void Title()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }


    //�ĊJ�{�^��
    public void Restart()
    {
        GameObject pausePanel = GameObject.Find("Pause");
        pause = pausePanel.GetComponent<Pause>();
        pause.pause();
    }


    //�v���C�{�^��
    public void Play()
    {
        SceneManager.LoadScene("Stage1");
    }


    //�`���[�g���A���{�^��
    public void Tutorial()
    {
        SceneManager.LoadScene(4);
    }


    //�ݒ�{�^��
    public void Setting()
    {
        SceneManager.LoadScene(3);
    }


    //�N���W�b�g�\�L�{�^��
    public void Cregits()
    {
        SceneManager.LoadScene(5);
    }


    //��߂�{�^��
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
