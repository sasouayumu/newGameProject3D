using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//�`���[�g���A����\������N���X
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


    //�`���[�g���A���p�l���������{�^��
    public void ExitTutorialButton()
    {
        //�{�^������������߂�
        Time.timeScale = 1;
        tutorialUI.SetActive(false);
    }


    void Update()
    {
        //�^�C���X�P�[�����[���Ȃ�`���[�g���A���p�l����True�ɂ���
        if (Mathf.Approximately(Time.timeScale, 0f)&& !pauseUI.activeSelf)
        {
            tutorialUI.SetActive(true);
        }
    }


    //�`���[�g���A���̏ꏊ���Ƃɕ��͂�\������
    private void OnTriggerEnter(Collider other)
    {
        if (tutorial)
        {
            //�ꎞ��~���ă`���[�g���A����\������
            Time.timeScale = 0f;

            if (this.gameObject.CompareTag("TutorialCamera"))
            {
                tutorialText.text = "\r\n<color=red>���N���b�N������</color>�ƃ}�E�X�ړ���\r\n�v���C���[�̎��_����B" +
                    "\r\n�v���C���[�͎��_�̕����ɐi��";
            }

            if (this.gameObject.CompareTag("TutorialSecondCamera"))
            {
                tutorialText.text = "<color=red>�}�E�X�z�C�[���{�^��</color>��\r\n��������B";
            }

            if (this.gameObject.CompareTag("TutorialDush"))
            {
                tutorialText.text = "<color=red>�E�N���b�N������</color>�Ń_�b�V���B" +
                    "\r\n�E��̃Q�[�W���Ȃ��Ȃ��\r\n�_�b�V���ł��Ȃ��Ȃ�B";
            }

            if (this.gameObject.CompareTag("TutorialJump"))
            {
                tutorialText.text = "<color=red>Space</color>�ŃW�����v�B";
            }

            if (this.gameObject.CompareTag("TutorialJumpStand"))
            {
                tutorialText.text = "<color=blue>�F�̏ꏊ</color>��<color=red>Space</color>��\r\n��荂���W�����v�B";
            }

            if (this.gameObject.CompareTag("TutorialPole"))
            {
                tutorialText.text = "<color=yellow>���F�̃|�[��</color>��<color=red>S�L�[</color>��\r\n�|�[�����g���ăW�����v�B";
            }

            if (this.gameObject.CompareTag("TutorialWallUp"))
            {
                tutorialText.text = "<color=#B76F3B>���F�̕�</color>��\r\n<color=red>A�L�[��D�L�[������</color>�ɉ�����\r\n�ǂ�o��B";
            }

            if (this.gameObject.CompareTag("TutorialWallKick"))
            {
                tutorialText.text = "<color=#B76F3B>���F�̕�</color>��\r\n<color=red>W�L�[</color>�Ŕ��Α��փW�����v�B";
            }

            if (this.gameObject.CompareTag("TutorialPouse"))
            {
                tutorialText.text = "<color=red>ESC�L�[</color>�Ń|�[�Y�B";
            }

            if (this.gameObject.CompareTag("TutorialEnemy"))
            {
                tutorialText.text = "<color=blue>�����X�^�[</color>�̓v���C���[��ǂ��Ă���B" +
                    "\r\n�������<color=purple>�Q�[���I�[�o�[</color>�B";
            }

            if (this.gameObject.CompareTag("TutorialKey"))
            {
                tutorialText.text = "<color=yellow>��</color>�������čs����" +
                    "\r\n<color=#00FFFF>�Q�[���N���A</color>�B\r\n���肵��<color=yellow>��</color>�͍����ɕ\���B";
            }

            tutorial = false;
        }
    }
}
