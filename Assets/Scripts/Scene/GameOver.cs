using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static string sceneName;
    public AudioClip gameOverSE;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
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

    public void Retry()
    {
        //Retry�{�^�����������瓯���V�[�����Đ�����
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator GameOverSE()
    {
        //SE��炵�Ă���Q�[���I�[�o�[�V�[����
        Time.timeScale = 0f;   
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
 }
