using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SetVars : MonoBehaviour
{
    public ParticleSpawnPoint spoint;
    public ParticleSpawner spawner;
    public TMP_InputField massField;
    public TMP_InputField visqField;

    void Start()
    {
        massField.text = spoint.particleMass.ToString("0.0000");
        visqField.text = spoint.particleViscosity.ToString("0.0000");
    }

    public void OnMassChanged()
    {
        var t = massField.text;
        Debug.Log(t);
        spoint.particleMass = float.Parse(t);
        spawner.ClearAllParticles();
    }

    public void OnVisqChanged()
    {
        var t = visqField.text;
        spoint.particleViscosity = float.Parse(t);
        spawner.ClearAllParticles();
    }
}
