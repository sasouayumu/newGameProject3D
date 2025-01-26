using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//�N���A���̃N���X
public class GameClear : MonoBehaviour
{
    private static bool key = false;
    [SerializeField] private GameObject getKeyUI;
    [SerializeField] public AudioClip getKeySE;
    [SerializeField] public AudioClip goalSE;
    private  AudioSource audioSource;
    static int sceneNumber;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    //�S�[���⌮�ɓ����������̃��\�b�h
    private void OnCollisionEnter(Collision collision)
    {
        //���������ăS�[���ɍs������Q�[���N���A�V�[���ɐi��
        if (collision.gameObject.CompareTag("Player") && key && this.gameObject.CompareTag("Goal"))
        {
            audioSource.PlayOneShot(goalSE);
            key = false;
            sceneNumber = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine("Goal");
        }

        //���ɓ���������key��True�ɂ��āA��������
        if (collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Key"))
        {
            key = true;
            AudioSource.PlayClipAtPoint(getKeySE,transform.position);
            getKeyUI.SetActive(true);
            Destroy(this.gameObject);
        }
    }



    //�S�[���ɕt�������̃R���[�`��
    IEnumerator Goal()
    {
        //SE��炵�Ă���V�[���ړ�������
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;

        //�`���[�g���A���Ȃ�^�C�g���ɕ��ʂ̃X�e�[�W�Ȃ�N���A��ʂɈړ�����
        if (sceneNumber == 4)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }


    //���̃X�e�[�W�ړ����郁�\�b�h
    public void Next()
    {
        //���̃X�e�[�W�ֈړ�����ŏI�X�e�[�W�Ȃ�^�C�g���֖߂�
        if(sceneNumber == 8)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(sceneNumber + 1);
        }
    }
}
