using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject targetObj;
    Vector3 targetPos;
    private bool inversion = true;
    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g�̈ړ����A�ړ�����
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        float mouseInputX;
        //�}�E�X�̉E�N���b�N�������Ă����
        if (Input.GetMouseButton(0))
        {
            //�}�E�X�̈ړ������]������
            mouseInputX = Input.GetAxis("Mouse X");

            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 50f);
        }

        if (Input.GetMouseButton(2)&& inversion)
        {
            inversion = false;
            transform.RotateAround(targetPos, Vector3.up, 180);
        }
        else if (!Input.GetMouseButton(2) && !inversion)
        {
            inversion = true;
        }
    }
}
