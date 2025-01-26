using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�|�[�Y���o���N���X
public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;
   

    // Update is called once per frame
    void Update()
    {
        //�^�C���X�P�[�����[���̏ꍇ�͏��������Ȃ�
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        //EscapeKey����������|�[�Y��ʂ��N������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
        }
    }


    public void pause()
    {
        //�|�[�Y��ʂ��o���A���Ԃ��~�߂�
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
