using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//クリア時のクラス
public class GameClear : MonoBehaviour
{
    [SerializeField] private GameObject getKeyUI;  //鍵取得状況を表示するUI
    [SerializeField] private AudioClip getKeySE;　 //鍵取得時のSE
    [SerializeField] private AudioClip goalSE;     //ゴール時のSE
    private static bool key = false;　　　　　　　 //鍵の取得状況の管理
    private  AudioSource audioSource;
    static int sceneNumber;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    //ゴールや鍵に当たった時のメソッド
    private void OnCollisionEnter(Collision collision)
    {
        //鍵が取得状態でゴールに行ったらゲームクリアシーンに進む
        if (collision.gameObject.CompareTag("Player") && key && this.gameObject.CompareTag("Goal"))
        {
            audioSource.PlayOneShot(goalSE);
            key = false;
            sceneNumber = SceneManager.GetActiveScene().buildIndex;　//現在のシーン番号を取得
            StartCoroutine("Goal");
        }

        //鍵に当たったら鍵を取得状態にして、鍵のオブジェクトを消す
        if (collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Key"))
        {
            key = true;
            AudioSource.PlayClipAtPoint(getKeySE, transform.position);
            getKeyUI.SetActive(true);　//左下に鍵を表示する
            Destroy(this.gameObject);
        }
    }


    //ゴールに付いた時のコルーチン
    IEnumerator Goal()
    {
        //SEを鳴らしてからシーン移動させる
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;

        //チュートリアルならタイトルに普通のステージならクリア画面に移動する
        if (sceneNumber == 4)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }


    //次のステージ移動するメソッド
    public void Next()
    {
        //次のステージへ移動する最終ステージならタイトルへ戻る
        if(sceneNumber == 10)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(sceneNumber + 1);
        }
    }
}
