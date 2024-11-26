using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject targetObj;
    Vector3 targetPos;
    private bool inversion = true; //�J�������]
    MousePlayerController MousePlayer;
    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
        MousePlayer = targetObj.GetComponent<MousePlayerController>();

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
            transform.RotateAround(targetPos, Vector3.up, 180);
            
        }
        
        //�}�E�X�z�C�[���{�^���𗣂�����J������߂�
        if (Input.GetMouseButtonUp(2))
        {
            inversion = true;
            transform.RotateAround(targetPos, Vector3.up, 180);
        }

        float mouseInputX;
        //�}�E�X�̍��N���b�N�������Ă����
        if (Input.GetMouseButton(0))
        {
            //�}�E�X�̈ړ������]������
            mouseInputX = Input.GetAxis("Mouse X");

            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 50f);
        }

        //�ǃL�b�N�̔���@�ǂɓ�����Ȃ���"W"Key�������ƃJ�����𔽓]�������Ε����ɔ��
        if (Input.GetKey("w") && MousePlayer.wallTouchgs)
        {
            MousePlayer.rbWallKick.velocity = - 1 * Vector3.right +Vector3.up * 7;
            transform.RotateAround(targetPos, Vector3.up, 180);

            
        }
    }
}
