using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//ゲームオーバー時に使うクラス
public class GameOver : MonoBehaviour
{
    [SerializeField]private AudioClip gameOverSE;
    private AudioSource audioSource;
    private static string sceneName;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    //ゲームオーバーシーンに移動するメソッド
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


    //もう一度ボタン
    public void Retry()
    {
        //Retryボタンを押したら同じシーンを再生する
        SceneManager.LoadScene(sceneName);
    }


    //ゲームオーバー時に呼び出されるコルーチン
    IEnumerator GameOverSE()
    {
        //SEを鳴らしてからゲームオーバーシーンへ
        Time.timeScale = 0f;   
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
 }
