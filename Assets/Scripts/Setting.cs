using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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


    //BGMÇ∆SEÇÃâπó í≤êÆÉoÅ[ÇÃèàóù
    public void SetAudioMixerBGM(float value)
    {
        value /= 5;

        var volume = Mathf.Clamp(Mathf.Log10(value)*20f,-80f,0f);

        audioMixer.SetFloat("BGM",volume);

        bgmValue = bgmSlider.value;

    }


    public void SetAudioMixerSE(float value)
    {
        value /= 5;

        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);

        audioMixer.SetFloat("SE", volume);

        seValue = seSlider.value;
    }
}
