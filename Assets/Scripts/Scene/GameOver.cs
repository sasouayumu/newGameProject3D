using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//�Q�[���I�[�o�[���Ɏg���N���X
public class GameOver : MonoBehaviour
{
    [SerializeField]private AudioClip gameOverSE;
    private AudioSource audioSource;
    private static string sceneName;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    //�Q�[���I�[�o�[�V�[���Ɉړ����郁�\�b�h
    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[�ɓ���������Q�[���I�[�o�[�V�[���Ɉړ�����
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(gameOverSE);
            sceneName = SceneManager.GetActiveScene().name;
            StartCoroutine("GameOverSE");
        }
    }


    //������x�{�^��
    public void Retry()
    {
        //Retry�{�^�����������瓯���V�[�����Đ�����
        SceneManager.LoadScene(sceneName);
    }


    //�Q�[���I�[�o�[���ɌĂяo�����R���[�`��
    IEnumerator GameOverSE()
    {
        //SE��炵�Ă���Q�[���I�[�o�[�V�[����
        Time.timeScale = 0f;   
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
 }
