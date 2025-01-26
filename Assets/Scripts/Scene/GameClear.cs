using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//クリア時のクラス
public class GameClear : MonoBehaviour
{
    private static bool key = false;
    [SerializeField] private GameObject getKeyUI;
    [SerializeField] public AudioClip getKeySE;
    [SerializeField] public AudioClip goalSE;
    private  AudioSource audioSource;
    static int sceneNumber;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    //ゴールや鍵に当たった時のメソッド
    private void OnCollisionEnter(Collision collision)
    {
        //鍵を持ってゴールに行ったらゲームクリアシーンに進む
        if (collision.gameObject.CompareTag("Player") && key && this.gameObject.CompareTag("Goal"))
        {
            audioSource.PlayOneShot(goalSE);
            key = false;
            sceneNumber = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine("Goal");
        }

        //鍵に当たったらkeyをTrueにして、鍵を消す
        if (collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Key"))
        {
            key = true;
            AudioSource.PlayClipAtPoint(getKeySE,transform.position);
            getKeyUI.SetActive(true);
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
        if(sceneNumber == 8)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(sceneNumber + 1);
        }
    }
}
