using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//チュートリアルを表示するクラス
public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    private GameObject tutorialUI;
    private GameObject pauseUI;
    private bool tutorial = true;


    private void Awake()
    {
        tutorialUI = GameObject.Find("TutorialPanel");
        pauseUI = GameObject.Find("PausePanel");
    }


    void Start()
    {
        tutorialUI.SetActive(false);
        pauseUI.SetActive(false);
    }


    //チュートリアルパネルを消すボタン
    public void ExitTutorialButton()
    {
        //ボタンを押したら戻す
        Time.timeScale = 1;
        tutorialUI.SetActive(false);
    }


    void Update()
    {
        //タイムスケールがゼロならチュートリアルパネルをTrueにする
        if (Mathf.Approximately(Time.timeScale, 0f)&& !pauseUI.activeSelf)
        {
            tutorialUI.SetActive(true);
        }
    }


    //チュートリアルの場所ごとに文章を表示する
    private void OnTriggerEnter(Collider other)
    {
        if (tutorial)
        {
            //一時停止してチュートリアルを表示する
            Time.timeScale = 0f;

            if (this.gameObject.CompareTag("TutorialCamera"))
            {
                tutorialText.text = "\r\n<color=red>左クリック長押し</color>とマウス移動で\r\nプレイヤーの視点操作。" +
                    "\r\nプレイヤーは視点の方向に進む";
            }

            if (this.gameObject.CompareTag("TutorialSecondCamera"))
            {
                tutorialText.text = "<color=red>マウスホイールボタン</color>で\r\n後ろを見る。";
            }

            if (this.gameObject.CompareTag("TutorialDush"))
            {
                tutorialText.text = "<color=red>右クリック長押し</color>でダッシュ。" +
                    "\r\n右上のゲージがなくなると\r\nダッシュできなくなる。";
            }

            if (this.gameObject.CompareTag("TutorialJump"))
            {
                tutorialText.text = "<color=red>Space</color>でジャンプ。";
            }

            if (this.gameObject.CompareTag("TutorialJumpStand"))
            {
                tutorialText.text = "<color=blue>青色の場所</color>で<color=red>Space</color>で\r\nより高くジャンプ。";
            }

            if (this.gameObject.CompareTag("TutorialPole"))
            {
                tutorialText.text = "<color=yellow>黄色のポール</color>で<color=red>Sキー</color>で\r\nポールを使ってジャンプ。";
            }

            if (this.gameObject.CompareTag("TutorialWallUp"))
            {
                tutorialText.text = "<color=#B76F3B>茶色の壁</color>で\r\n<color=red>AキーとDキーを交互</color>に押して\r\n壁を登る。";
            }

            if (this.gameObject.CompareTag("TutorialWallKick"))
            {
                tutorialText.text = "<color=#B76F3B>茶色の壁</color>で\r\n<color=red>Wキー</color>で反対側へジャンプ。";
            }

            if (this.gameObject.CompareTag("TutorialPouse"))
            {
                tutorialText.text = "<color=red>ESCキー</color>でポーズ。";
            }

            if (this.gameObject.CompareTag("TutorialEnemy"))
            {
                tutorialText.text = "<color=blue>モンスター</color>はプレイヤーを追ってくる。" +
                    "\r\n当たると<color=purple>ゲームオーバー</color>。";
            }

            if (this.gameObject.CompareTag("TutorialKey"))
            {
                tutorialText.text = "<color=yellow>鍵</color>を持って行くと" +
                    "\r\n<color=#00FFFF>ゲームクリア</color>。\r\n入手した<color=yellow>鍵</color>は左下に表示。";
            }

            tutorial = false;
        }
    }
}
