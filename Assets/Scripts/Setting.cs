using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


//設定関連のクラス
[RequireComponent(typeof(Slider))]
public class Setting : MonoBehaviour
{
    [SerializeField]AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    static float bgmValue = 5;
    static float seValue = 5;   


    private void Start()
    {
        bgmSlider.value = bgmValue;
        seSlider.value = seValue;
        bgmSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        seSlider.onValueChanged.AddListener(SetAudioMixerSE);
        
    }


    //BGMの音量調整バーのメソッド
    public void SetAudioMixerBGM(float value)
    {
        value /= 5;

        var volume = Mathf.Clamp(Mathf.Log10(value)*20f,-80f,0f);

        audioMixer.SetFloat("BGM",volume);

        bgmValue = bgmSlider.value;

    }


    //SEの音量調節バーのメソッド
    public void SetAudioMixerSE(float value)
    {
        value /= 5;

        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);

        audioMixer.SetFloat("SE", volume);

        seValue = seSlider.value;
    }
}
