using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets;

public class GlobalSettingsButLessGlobal : MonoBehaviour
{
    [SerializeField] TMP_InputField ParticleCount;
    [SerializeField] TMP_InputField GravityX;
    [SerializeField] TMP_InputField GravityY;
    [SerializeField] TMP_InputField PlatformAngle;

    [SerializeField] TMP_InputField Outlet1Rate;
    [SerializeField] TMP_InputField Outlet1Mass;
    [SerializeField] TMP_InputField Outlet1Viscosity;
    [SerializeField] TMP_InputField Outlet1Spread;
    [SerializeField] TMP_InputField Outlet1VelX;
    [SerializeField] TMP_InputField Outlet1VelY;

    [SerializeField] TMP_InputField Outlet2Rate;
    [SerializeField] TMP_InputField Outlet2Mass;
    [SerializeField] TMP_InputField Outlet2Viscosity;
    [SerializeField] TMP_InputField Outlet2Spread;
    [SerializeField] TMP_InputField Outlet2VelX;
    [SerializeField] TMP_InputField Outlet2VelY;

    [SerializeField] ParticleSpawner ParticleSpawner;
    [SerializeField] ParticleSpawnPoint Outlet1;
    [SerializeField] ParticleSpawnPoint Outlet2;
    [SerializeField] Transform Platform;
    // Start is called before the first frame update
    void Start()
    {
        ParticleCount.text = ParticleSpawner.ParticleCount.ToString();
        GravityX.text = ParticleSpawner.gravityForce.x.ToString("F2");
        GravityY.text = ParticleSpawner.gravityForce.y.ToString("F2");
        PlatformAngle.text = Platform.rotation.eulerAngles.z.ToString("F2");
        
        Outlet1Rate.text = Outlet1.spawnRate.ToString("F2");
        Outlet1Mass.text = Outlet1.particleMass.ToString("F2");
        Outlet1Viscosity.text = Outlet1.particleViscosity.ToString("F2");
        Outlet1Spread.text = Outlet1.randomSpread.ToString("F2");
        Outlet1VelX.text = Outlet1.outputVelocity.x.ToString("F2");
        Outlet1VelY.text = Outlet1.outputVelocity.y.ToString("F2");

        Outlet2Rate.text = Outlet2.spawnRate.ToString("F2");
        Outlet2Mass.text = Outlet2.particleMass.ToString("F2");
        Outlet2Viscosity.text = Outlet2.particleViscosity.ToString("F2");
        Outlet2Spread.text = Outlet2.randomSpread.ToString("F2");
        Outlet2VelX.text = Outlet2.outputVelocity.x.ToString("F2");
        Outlet2VelY.text = Outlet2.outputVelocity.y.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnUpdateParticleCount()
    {
        if (int.TryParse(ParticleCount.text, out int value))
        {
            ParticleSpawner.ParticleCount = Mathf.Clamp(value, 10, 200);
        }
    }

    public void OnUpdatePlatformAngle()
    {
        var text = PlatformAngle.text.Replace('.', ',');
        if (float.TryParse(text, out float value))
        {
            Platform.rotation = Quaternion.Euler(0, 0, value);
        }
    }

    public void OnGravityChange(bool y)
    {
        var text = (y ? GravityY : GravityX).text.Replace('.', ',');
        if (float.TryParse(text, out float value))
        {
            ParticleSpawner.gravityForce[y ? 1 : 0] = value;
        }
    }

    public void OnVelocityXChange(bool isSecond)
    {
        string text = (isSecond ? Outlet2VelX : Outlet1VelX).text.Replace('.', ',');
        if (float.TryParse(text, out float value))
        {
            (isSecond ? Outlet2 : Outlet1).outputVelocity.x = value;
        }
    }

    public void OnVelocityYChange(bool isSecond)
    {
        string text = (isSecond ? Outlet2VelY : Outlet1VelY).text.Replace('.', ',');
        if (float.TryParse(text, out float value))
        {
            (isSecond ? Outlet2 : Outlet1).outputVelocity.y = value;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnSpawnRateChange(bool isSecond)
    {
        var text = (isSecond ? Outlet2Rate : Outlet1Rate).text.Replace('.', ',');
        if (float.TryParse(text, out float value))
        {
            if (isSecond)
            {
                Outlet2.spawnRate = value;
                Outlet2.ResetSpawnDelay();
            }
            else
            {
                Outlet1.spawnRate = value;
                Outlet1.ResetSpawnDelay();
            }
        }
    }

    public void OnMassChange(bool isSecond)
    {
        var text = (isSecond ? Outlet2Mass : Outlet1Mass).text.Replace('.', ',');
        if (float.TryParse(text, out float value))
        {
            (isSecond ? Outlet2 : Outlet1).particleMass = value;
        }
    }
    public void OnViscosityChange(bool isSecond)
    {
        var text = (isSecond ? Outlet2Viscosity : Outlet1Viscosity).text.Replace('.', ',');
        if (float.TryParse(text, out float value))
        {
            (isSecond ? Outlet2 : Outlet1).particleViscosity = value;
        }
    }
    public void OnSpreadChange(bool isSecond)
    {
        var text = (isSecond ? Outlet2Spread : Outlet1Spread).text.Replace('.', ',');
        if (float.TryParse(text, out float value))
        {
            (isSecond ? Outlet2 : Outlet1).randomSpread = Mathf.Max(value, 0.0f);
        }
    }
}
