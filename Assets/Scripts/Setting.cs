using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


//�ݒ�֘A�̃N���X
[RequireComponent(typeof(Slider))]
public class Setting : MonoBehaviour
{
    //���ʌn�̒���
    [SerializeField]AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    static float bgmValue = 5;
    static float seValue = 5;

    //�}�E�X���x�̒���
    [SerializeField] Slider mouseSlider;
    static float mouseValue = 5;
    [SerializeField] private CameraController cameraController;


    private void Start()
    {
        bgmSlider.value = bgmValue;
        seSlider.value = seValue;
        mouseSlider.value = mouseValue;
        bgmSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        seSlider.onValueChanged.AddListener(SetAudioMixerSE);
        mouseSlider.onValueChanged.AddListener(SetMouseSensitivity);
        GameObject mainCamera = GameObject.Find("Main Camera");
        cameraController = mainCamera.GetComponent<CameraController>();

    }


    //BGM�̉��ʒ����o�[�̃��\�b�h
    public void SetAudioMixerBGM(float value)
    {
        value /= 5;

        var volume = Mathf.Clamp(Mathf.Log10(value)*20f,-80f,0f);

        audioMixer.SetFloat("BGM",volume);

        bgmValue = bgmSlider.value;

    }


    //SE�̉��ʒ��߃o�[�̃��\�b�h
    public void SetAudioMixerSE(float value)
    {
        value /= 5;

        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);

        audioMixer.SetFloat("SE", volume);

        seValue = seSlider.value;
    }


    //�}�E�X���x�o�[�̃��\�b�h
    public void SetMouseSensitivity(float value)
    {
        value /= 5;

        var sensitivity = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);

        mouseValue = mouseSlider.value;

        cameraController.GetSetMouseSensitivity = mouseValue;
    }
}
