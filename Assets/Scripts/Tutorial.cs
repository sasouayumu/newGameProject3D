using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tutorialText;
    
    
   //チュートリアルの文
    private void OnTriggerStay(Collider other)
    {
        if (this.gameObject.CompareTag("TutorialCamera"))
        {
            tutorialText.text = "プレイヤーはカメラの見ている方向に進み続けます。左クリックを押しながらマウスを動かすことでカメラを左右に動かせます。";
        }

        if (this.gameObject.CompareTag("TutorialSecondCamera"))
        {
            tutorialText.text = "マウスホイールボタンを押すことでキャラの後ろを見ることができます。";
        }

        if (this.gameObject.CompareTag("TutorialDush"))
        {
            tutorialText.text = "右クリックを押すことで走ることができます。右上のゲージがなくなると走れなくなります。";
        }

        if (this.gameObject.CompareTag("TutorialJump"))
        {
            tutorialText.text = "地面にいる時にSpaceを押すことでジャンプすることができます。";
        }

        if (this.gameObject.CompareTag("TutorialJumpStand"))
        {
            tutorialText.text = "青色の場所でSpaceを押すことで通常より高くジャンプすることができます。";
        }

        if (this.gameObject.CompareTag("TutorialPole"))
        {
            tutorialText.text = "黄色のポールでSキーを押すことでポールを使ってジャンプすることがいます。";
        }

        if (this.gameObject.CompareTag("TutorialWallUp"))
        {
            tutorialText.text = "茶色の壁でAキーとDキーを交互に押すことで登ることができます。";
        }

        if (this.gameObject.CompareTag("TutorialWallKick"))
        {
            tutorialText.text = "茶色の壁でWキーを押すことで壁を蹴って反対側にジャンプできます。";
        }

        if (this.gameObject.CompareTag("TutorialEnemy"))
        {
            tutorialText.text = "モンスターはプレイヤーを追いかけてきます。当たるとゲームオーバーです。";
        }

        if (this.gameObject.CompareTag("TutorialPouse"))
        {
            tutorialText.text = "ESCキーを押すことでポーズ画面に行けます。";
        }

        if (this.gameObject.CompareTag("TutorialKey"))
        {
            tutorialText.text = "鍵を持って家のドアに行くとゲームクリアです。入手した鍵は左下の表示されます。";
        }
    }
    //該当の場所を過ぎたら戻す
    private void OnTriggerExit(Collider other)
    {
            tutorialText.text = "";
    }
}
