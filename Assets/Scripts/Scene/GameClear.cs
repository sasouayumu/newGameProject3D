using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//�N���A���̃N���X
public class GameClear : MonoBehaviour
{
    [SerializeField] private GameObject getKeyUI;  //���擾�󋵂�\������UI
    [SerializeField] private AudioClip getKeySE;�@ //���擾����SE
    [SerializeField] private AudioClip goalSE;     //�S�[������SE
    private static bool key = false;�@�@�@�@�@�@�@ //���̎擾�󋵂̊Ǘ�
    private  AudioSource audioSource;
    static int sceneNumber;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    //�S�[���⌮�ɓ����������̃��\�b�h
    private void OnCollisionEnter(Collision collision)
    {
        //�����擾��ԂŃS�[���ɍs������Q�[���N���A�V�[���ɐi��
        if (collision.gameObject.CompareTag("Player") && key && this.gameObject.CompareTag("Goal"))
        {
            audioSource.PlayOneShot(goalSE);
            key = false;
            sceneNumber = SceneManager.GetActiveScene().buildIndex;�@//���݂̃V�[���ԍ����擾
            StartCoroutine("Goal");
        }

        //���ɓ��������献���擾��Ԃɂ��āA���̃I�u�W�F�N�g������
        if (collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Key"))
        {
            key = true;
            AudioSource.PlayClipAtPoint(getKeySE, transform.position);
            getKeyUI.SetActive(true);�@//�����Ɍ���\������
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
        if(sceneNumber == 10)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(sceneNumber + 1);
        }
    }
}
