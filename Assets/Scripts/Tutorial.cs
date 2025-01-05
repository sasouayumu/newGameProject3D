using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tutorialText;
    
    
   //�`���[�g���A���̕�
    private void OnTriggerStay(Collider other)
    {
        if (this.gameObject.CompareTag("TutorialCamera"))
        {
            tutorialText.text = "�v���C���[�̓J�����̌��Ă�������ɐi�ݑ����܂��B���N���b�N�������Ȃ���}�E�X�𓮂������ƂŃJ���������E�ɓ������܂��B";
        }

        if (this.gameObject.CompareTag("TutorialSecondCamera"))
        {
            tutorialText.text = "�}�E�X�z�C�[���{�^�����������ƂŃL�����̌������邱�Ƃ��ł��܂��B";
        }

        if (this.gameObject.CompareTag("TutorialDush"))
        {
            tutorialText.text = "�E�N���b�N���������Ƃő��邱�Ƃ��ł��܂��B�E��̃Q�[�W���Ȃ��Ȃ�Ƒ���Ȃ��Ȃ�܂��B";
        }

        if (this.gameObject.CompareTag("TutorialJump"))
        {
            tutorialText.text = "�n�ʂɂ��鎞��Space���������ƂŃW�����v���邱�Ƃ��ł��܂��B";
        }

        if (this.gameObject.CompareTag("TutorialJumpStand"))
        {
            tutorialText.text = "�F�̏ꏊ��Space���������ƂŒʏ��荂���W�����v���邱�Ƃ��ł��܂��B";
        }

        if (this.gameObject.CompareTag("TutorialPole"))
        {
            tutorialText.text = "���F�̃|�[����S�L�[���������ƂŃ|�[�����g���ăW�����v���邱�Ƃ����܂��B";
        }

        if (this.gameObject.CompareTag("TutorialWallUp"))
        {
            tutorialText.text = "���F�̕ǂ�A�L�[��D�L�[�����݂ɉ������Ƃœo�邱�Ƃ��ł��܂��B";
        }

        if (this.gameObject.CompareTag("TutorialWallKick"))
        {
            tutorialText.text = "���F�̕ǂ�W�L�[���������Ƃŕǂ��R���Ĕ��Α��ɃW�����v�ł��܂��B";
        }

        if (this.gameObject.CompareTag("TutorialEnemy"))
        {
            tutorialText.text = "�����X�^�[�̓v���C���[��ǂ������Ă��܂��B������ƃQ�[���I�[�o�[�ł��B";
        }

        if (this.gameObject.CompareTag("TutorialPouse"))
        {
            tutorialText.text = "ESC�L�[���������ƂŃ|�[�Y��ʂɍs���܂��B";
        }

        if (this.gameObject.CompareTag("TutorialKey"))
        {
            tutorialText.text = "���������ĉƂ̃h�A�ɍs���ƃQ�[���N���A�ł��B���肵�����͍����̕\������܂��B";
        }
    }
    //�Y���̏ꏊ���߂�����߂�
    private void OnTriggerExit(Collider other)
    {
            tutorialText.text = "";
    }
}
