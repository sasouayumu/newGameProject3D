using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Setting : MonoBehaviour
{
    Slider m_Slider;
    [SerializeField]
    bool isInput;
    [SerializeField]
    float ScroolSpeed = 1;
    void Awake()
    {
        m_Slider = GetComponent<Slider>();
        m_Slider.value = AudioListener.volume;
    }

    private void OnEnable()
    {
        m_Slider.value = AudioListener.volume;
        m_Slider.onValueChanged.AddListener((sliderValue) =>AudioListener.volume = sliderValue);
    }

    private void OnDisable()
    {
        m_Slider.onValueChanged.RemoveAllListeners();
    }
}
