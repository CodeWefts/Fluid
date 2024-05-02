using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    ParticleSystem fluidParticleSystem;
    WaterSquare waterSquare;
    FluidScript fluidScript;
    [SerializeField] public float OrificeSurface = 0.0004f;
    [SerializeField] public float OrificeHeight = 0f;

    Vector3 defaultPos;
    Vector3 defaultSca;

    public void SetHeight(float value)
    {
        OrificeHeight = value;
        var position = defaultPos;
        position.y += OrificeHeight;
        position = new Vector3(transform.position.x, position.y + (transform.localScale.y / 4), transform.position.z);
        transform.position = position;
    }

    public void SetSize(float value)
    {
        OrificeSurface = value;
        var scale = defaultSca;
        scale.y = scale.y * Mathf.Sqrt(OrificeSurface) / 0.02f;
        transform.localScale = scale;

        SetHeight(OrificeHeight);
    }

    public float LastTime;

    public float Velocity;
    // Start is called before the first frame update
    void Awake()
    {
        defaultPos = transform.position;
        defaultSca = transform.localScale;
        fluidParticleSystem = GetComponentInChildren<ParticleSystem>();
        fluidScript = FindObjectOfType<FluidScript>();
        waterSquare = FindObjectOfType<WaterSquare>();


        var scale = transform.localScale;
        scale.y =  scale.y * Mathf.Sqrt(OrificeSurface) / 0.02f;
        transform.localScale = scale;


        var position = transform.position;
        position.y += OrificeHeight;
        position = new Vector3(transform.position.x, position.y + (transform.localScale.y / 4), transform.position.z);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRenderer()
    {
        ParticleSystem.EmissionModule emissionModule = fluidParticleSystem.emission;
        float FluidPercent = 100f * Mathf.Sqrt(OrificeSurface) / fluidScript.InitialHeight;
        float FluidHeight = FluidPercent + 100 * OrificeHeight / fluidScript.InitialHeight;
        if (OrificeSurface <= 0)
        {
            emissionModule.rateOverTime = 0f;
        }
        else if (waterSquare.Percent > FluidHeight)
        {
            emissionModule.rateOverTime = Mathf.Clamp(waterSquare.ParticleNumber * OrificeSurface * 25f, 25f, 250f) * 5f;
        }
        else if (waterSquare.Percent > 100 * OrificeHeight / fluidScript.InitialHeight)
        {
            emissionModule.rateOverTime = Mathf.Clamp(waterSquare.ParticleNumber * OrificeSurface * 25f, 25f, 250f) * ((waterSquare.Percent - 100 * OrificeHeight / fluidScript.InitialHeight) * 0.25f);
        }
        else
        {
            emissionModule.rateOverTime = 0f;
        }

        ParticleSystem.VelocityOverLifetimeModule velocity = fluidParticleSystem.velocityOverLifetime;
        ParticleSystem.MinMaxCurve minMax = velocity.x;
        minMax.constantMax = Mathf.Max(Velocity, 0f);
        minMax.constantMin = Mathf.Max(Velocity - 1f, 0);
        velocity.x = minMax;

        float d = fluidScript.CurrentHeight - (OrificeHeight - (Mathf.Sqrt(OrificeSurface) / 2f));
        float diff = Mathf.Clamp(d, 0f, Mathf.Sqrt(OrificeSurface));
        ParticleSystem.ShapeModule shape = fluidParticleSystem.shape;
        shape.scale = new Vector3(shape.scale.x, diff / 2f, shape.scale.z);
        float newDif = (Mathf.Sqrt(OrificeSurface) - diff);
        shape.position = new Vector3(shape.position.x, -newDif, shape.position.z);
    }
}
