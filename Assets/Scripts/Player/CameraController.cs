using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


//�v���C���[�ɒǏ]����J�����̊Ǘ�����N���X
public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject secondCamera; //�v���C���[�̌������邽�߂̃J����
    private GameObject targetObj;                     //�J�������Ǐ]����I�u�W�F�N�g
    private Vector3 targetPos;                        //�Ǐ]����I�u�W�F�N�g�̏ꏊ
    private bool inversion = true;                    //�J�������]

    //�}�E�X���x�̒���
    private float mouseSensitivity = 5;
    public float GetSetMouseSensitivity { set { mouseSensitivity = value; } }

    string sceneName;
   

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "SettingScene") 
        {
            //Player�̏����擾
            targetObj = GameObject.Find("Player");
            targetPos = targetObj.transform.position;
            
        }
    }


    void Update()
    {
        //�^�C���X�P�[�����[���̏ꍇ�͏������Ȃ�
        if (Mathf.Approximately(Time.timeScale, 0f)|| sceneName == "SettingScene")
        {
            return;
        }

        //�^�[�Q�b�g�̈ړ����A�ړ�����
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        //�}�E�X�z�C�[���{�^���������Ă���ԃv���C���[�̑O�ɂ���J�������N������
        if (Input.GetMouseButtonDown(2) && inversion)
        {
            inversion = false;
            secondCamera.gameObject.SetActive(true);

        }//�}�E�X�z�C�[���{�^���𗣂�����J������߂�
        else if (Input.GetMouseButtonUp(2))
        {
            inversion = true;
            secondCamera.gameObject.SetActive(false);
        }

        float mouseInputX;

        if (Input.GetMouseButton(0))//�}�E�X�̍��N���b�N�������Ă����
        {
            //�}�E�X�̈ړ������]������
            mouseInputX = Input.GetAxis("Mouse X");

            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 2f*mouseSensitivity);
        }
    }


    //�J�����𔽓]�����鏈��
    public void InversionCamera()
    {
        transform.RotateAround(targetPos, Vector3.up, 180);
    }
}
