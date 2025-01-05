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
        //プレイヤーに当たったらゲームオーバーシーンに移動する
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(gameOverSE);
            sceneName = SceneManager.GetActiveScene().name;
            StartCoroutine("GameOverSE");
        }
    }

    public void Retry()
    {
        //Retryボタンを押したら同じシーンを再生する
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator GameOverSE()
    {
        //SEを鳴らしてからゲームオーバーシーンへ
        Time.timeScale = 0f;   
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
 }
