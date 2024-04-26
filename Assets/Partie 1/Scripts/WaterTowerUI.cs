using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using static System.Collections.Specialized.BitVector32;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public struct UIReferences
{
    public TextMeshProUGUI VolumicMassText;
    public TextMeshProUGUI FluidHeightText;
    public TextMeshProUGUI OrificeText;

    public TextMeshProUGUI TotalTimeText;
    public TextMeshProUGUI InitialSpeedText;

    public TextMeshProUGUI PlayButtonText;
    public TextMeshProUGUI TimeScaleValueText;

    public UnityEngine.UI.Button Play;
    public UnityEngine.UI.Button Fill;
    public UnityEngine.UI.Button LoadPart2;

    public UnityEngine.UI.Slider TimeScaleSlider;
};

public class WaterTowerUI : MonoBehaviour
{
    [Tooltip("True if the simulation is updating")]
    public bool shouldUpdateSimulation = false;

    public UIReferences UIRefs;
    void Start()
    {
        UIRefs = FecthUIReferences();
        InitialiseUI();
    }

    void Update()
    {
        
    }

    public void InitialiseUI()
    {
        UIRefs.Play.onClick.AddListener(OnClickPlayButton);
        UIRefs.Fill.onClick.AddListener(OnClickFillButon);
        UIRefs.LoadPart2.onClick.AddListener(OnClickLoadPart2);
    }

    public UIReferences FecthUIReferences()
    {
        UIReferences refs = new UIReferences();

        refs.Play = GameObject.Find("Play").GetComponent<UnityEngine.UI.Button>();
        refs.Fill = GameObject.Find("Fill").GetComponent<UnityEngine.UI.Button>();
        refs.LoadPart2 = GameObject.Find("LoadPart2").GetComponent<UnityEngine.UI.Button>();

        refs.VolumicMassText = GameObject.Find("VolumicMass").GetComponent<TextMeshProUGUI>();
        refs.FluidHeightText = GameObject.Find("FluidHeight").GetComponent<TextMeshProUGUI>();
        refs.OrificeText = GameObject.Find("Orifice").GetComponent<TextMeshProUGUI>();

        refs.TotalTimeText = GameObject.Find("TotalTime").GetComponent<TextMeshProUGUI>();
        refs.InitialSpeedText = GameObject.Find("InitialSpeed").GetComponent<TextMeshProUGUI>();

        refs.PlayButtonText = GameObject.Find("PlayText").GetComponent<TextMeshProUGUI>();

        refs.TimeScaleSlider = GameObject.Find("TimeScaleSlider").GetComponent<UnityEngine.UI.Slider>();
        refs.TimeScaleValueText = GameObject.Find("TimeScaleValue").GetComponent<TextMeshProUGUI>();


        return refs;
    }

    public void OnClickPlayButton()
    {
        //Start simulation
        shouldUpdateSimulation = !shouldUpdateSimulation;
        
        //UI
        UIRefs.PlayButtonText.text = shouldUpdateSimulation ? "Pause" : "Play";
    }

    public void OnClickFillButon()
    {

    }

    public void OnClickLoadPart2()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
