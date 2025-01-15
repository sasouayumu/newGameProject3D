using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    
   //�`���[�g���A���̏ꏊ���Ƃɕ��͂�\������
    private void OnTriggerStay(Collider other)
    {
        if (this.gameObject.CompareTag("TutorialCamera"))
        {
            tutorialText.text = "�v���C���[�͂܂������i�ށB���N���b�N�������ƃ}�E�X�ړ��ŃJ��������B";
        }

        if (this.gameObject.CompareTag("TutorialSecondCamera"))
        {
            tutorialText.text = "�}�E�X�z�C�[���{�^���Ō��������B";
        }

        if (this.gameObject.CompareTag("TutorialDush"))
        {
            tutorialText.text = "�E�N���b�N�������Ń_�b�V���B";
        }

        if (this.gameObject.CompareTag("TutorialJump"))
        {
            tutorialText.text = "Space�ŃW�����v�B";
        }

        if (this.gameObject.CompareTag("TutorialJumpStand"))
        {
            tutorialText.text = "�F�̏ꏊ��Space�ł�荂���W�����v�B";
        }

        if (this.gameObject.CompareTag("TutorialPole"))
        {
            tutorialText.text = "���F�̃|�[����S�L�[�Ń|�[�����g���ăW�����v�B";
        }

        if (this.gameObject.CompareTag("TutorialWallUp"))
        {
            tutorialText.text = "���F�̕ǂ�A�L�[��D�L�[���݂ɉ����ĕǂ�o��B";
        }

        if (this.gameObject.CompareTag("TutorialWallKick"))
        {
            tutorialText.text = "���F�̕ǂ�W�L�[�Ŕ��Α��փW�����v�B";
        }

        if (this.gameObject.CompareTag("TutorialEnemy"))
        {
            tutorialText.text = "�����X�^�[�̓v���C���[��ǂ��Ă���B������ƃQ�[���I�[�o�[�B";
        }

        if (this.gameObject.CompareTag("TutorialPouse"))
        {
            tutorialText.text = "ESC�L�[�Ń|�[�Y�B";
        }

        if (this.gameObject.CompareTag("TutorialKey"))
        {
            tutorialText.text = "���������ĉƂ̃h�A�ɍs���ƃQ�[���N���A�B���肵�����͍����̕\���B";
        }
    }
    //���ꂽ��Text��߂�
    private void OnTriggerExit(Collider other)
    {
            tutorialText.text = "";
    }
}
