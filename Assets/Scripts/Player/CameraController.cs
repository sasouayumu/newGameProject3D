using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject targetObj;//�J�������Ǐ]����I�u�W�F�N�g
    private GameObject enemyObj;//Enemy�I�u�W�F�N�g
    [SerializeField]
    private GameObject SecondCamera;
    private Vector3 targetPos;//�Ǐ]����I�u�W�F�N�g�̏ꏊ
    private bool inversion = true; //�J�������]
    

    // Start is called before the first frame update
    void Start()
    {
        //Player�̏����擾
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g�̈ړ����A�ړ�����
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        //�}�E�X�z�C�[���{�^���������Ă���ԃJ�����𔽑΂Ɉړ�����
        if (Input.GetMouseButtonDown(2) && inversion)
        {
            //Debug.Log(MousePlayer.wallTouchgs);
            inversion = false;
            SecondCamera.gameObject.SetActive(true);

        }//�}�E�X�z�C�[���{�^���𗣂�����J������߂�
        else if (Input.GetMouseButtonUp(2))
        {
            inversion = true;
            SecondCamera.gameObject.SetActive(false);
        }

        float mouseInputX;
        //�}�E�X�̍��N���b�N�������Ă����
        if (Input.GetMouseButton(0))
        {
            //�}�E�X�̈ړ������]������
            mouseInputX = Input.GetAxis("Mouse X");

            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 50f);
        }
    }
    public void InversionCamera()
    {
        transform.RotateAround(targetPos, Vector3.up, 180);
    }
}
