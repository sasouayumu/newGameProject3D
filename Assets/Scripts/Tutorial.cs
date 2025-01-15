using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    
   //チュートリアルの場所ごとに文章を表示する
    private void OnTriggerStay(Collider other)
    {
        if (this.gameObject.CompareTag("TutorialCamera"))
        {
            tutorialText.text = "プレイヤーはまっすぐ進む。左クリック長押しとマウス移動でカメラ操作。";
        }

        if (this.gameObject.CompareTag("TutorialSecondCamera"))
        {
            tutorialText.text = "マウスホイールボタンで後ろを見れる。";
        }

        if (this.gameObject.CompareTag("TutorialDush"))
        {
            tutorialText.text = "右クリック長押しでダッシュ。";
        }

        if (this.gameObject.CompareTag("TutorialJump"))
        {
            tutorialText.text = "Spaceでジャンプ。";
        }

        if (this.gameObject.CompareTag("TutorialJumpStand"))
        {
            tutorialText.text = "青色の場所でSpaceでより高くジャンプ。";
        }

        if (this.gameObject.CompareTag("TutorialPole"))
        {
            tutorialText.text = "黄色のポールでSキーでポールを使ってジャンプ。";
        }

        if (this.gameObject.CompareTag("TutorialWallUp"))
        {
            tutorialText.text = "茶色の壁でAキーとDキー交互に押して壁を登る。";
        }

        if (this.gameObject.CompareTag("TutorialWallKick"))
        {
            tutorialText.text = "茶色の壁でWキーで反対側へジャンプ。";
        }

        if (this.gameObject.CompareTag("TutorialEnemy"))
        {
            tutorialText.text = "モンスターはプレイヤーを追ってくる。当たるとゲームオーバー。";
        }

        if (this.gameObject.CompareTag("TutorialPouse"))
        {
            tutorialText.text = "ESCキーでポーズ。";
        }

        if (this.gameObject.CompareTag("TutorialKey"))
        {
            tutorialText.text = "鍵を持って家のドアに行くとゲームクリア。入手した鍵は左下の表示。";
        }
    }
    //離れたらTextを戻す
    private void OnTriggerExit(Collider other)
    {
            tutorialText.text = "";
    }
}
