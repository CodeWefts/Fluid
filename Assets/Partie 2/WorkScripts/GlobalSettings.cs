using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalSettings : MonoBehaviour
{
    [SerializeField] TMP_Text TextTime;
    [SerializeField] Button PlayPauseButton;
    [SerializeField] TMP_InputField TimeScaleInput;
    [SerializeField] TMP_InputField PressureInput;
    [SerializeField] TMP_InputField VolumicMassInput;
    private FluidScript fluidScript;
    // Start is called before the first frame update
    void Start()
    {
        fluidScript = FindObjectOfType<FluidScript>();
        TimeScaleInput.text = Time.timeScale.ToString("F2");
        PressureInput.text = fluidScript.Pressure.ToString("F2");
        VolumicMassInput.text = fluidScript.VolumicMass.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        TextTime.text = $"Temps : {fluidScript.CurrentTime.ToString("F2")}s";
    }

    bool play = true;
    float lastTimeScale = 1f;
    public void OnButtonClick()
    {
        play = !play;
        if (play)
        {
            PlayPauseButton.GetComponentInChildren<TMP_Text>().text = "Pause";
            Time.timeScale = lastTimeScale;
        }
        else
        {
            PlayPauseButton.GetComponentInChildren<TMP_Text>().text = "Play";
            lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
    }

    public void OnTimeScaleChange()
    {
        var text = TimeScaleInput.text.Replace('.', ',');
        if (float.TryParse(text, out float value))
            Time.timeScale = Mathf.Clamp(value, 0.0f, 100f);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPressureChange()
    {
        var text = PressureInput.text.Replace('.', ',');
        if (float.TryParse(text, out float value))
            fluidScript.Pressure = Mathf.Max(value, 0.0f);
    }

    public void OnVolumicMassChange()
    {
        var text = VolumicMassInput.text.Replace('.', ',');
        if (float.TryParse(text, out float value))
            fluidScript.VolumicMass = Mathf.Max(value, 0.1f);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

}
