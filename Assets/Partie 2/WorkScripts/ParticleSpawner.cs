using Assets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public int ParticleCount = 1000;
    public GameObject objectToSpawn;

    public float SmoothingRadius = 3.0f;
    public float RefDensity = 0.1f;
    public float PressureConstant = 2.0f;
    private float spikeyGradCoefficient = -45.0f / MathF.PI * MathF.Pow(3.0f, 6);
    private float poly6Coefficient = 315.0f / (64.0f * MathF.PI * MathF.Pow(3.0f, 9));
    public Vector2 gravityForce = new Vector2(0, -9.81f);
    public float motionDampingCoefficient = 0.0f;
    public float maximumAcceleration = 75.0f;
    public List<ParticleSpawnPoint> spawnPoints;
    private static ParticleSpawner spawnerInstance;
    public static ParticleSpawner SpawnerInstance { get => spawnerInstance; }

    private List<ParticleScript> particles = new List<ParticleScript>();
    private List<ParticleScript> particlesToRemove = new List<ParticleScript>();
    // Start is called before the first frame update
    void Start()
    {
        spawnerInstance = this;
        spikeyGradCoefficient = -45.0f / MathF.PI * MathF.Pow(SmoothingRadius, 6);
        poly6Coefficient = 315.0f / (64.0f * MathF.PI * MathF.Pow(SmoothingRadius, 9));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var item in particles)
        {
            item.UpdateDensity(poly6Coefficient, SmoothingRadius, RefDensity == 0 ? 0.1f : RefDensity, PressureConstant);
            item.UpdateAccel(SmoothingRadius, spikeyGradCoefficient, gravityForce, maximumAcceleration, motionDampingCoefficient);
        }
        /*foreach (var item in particles)
        {
        }*/
        
        foreach (var item in particlesToRemove)
        {
            if (particles.Remove(item))
            {
                Destroy(item.gameObject);
            }
        }
        particlesToRemove.Clear();
        foreach (var item in spawnPoints)
        {
            if (!item.isActive) continue;
            item.SpawnParticles(this);
            if (particles.Count >= ParticleCount) break;
        }
    }

    public void RemoveParticle(ParticleScript item)
    {
        particlesToRemove.Add(item);
    }

    public int GetAvailableParticleCount()
    {
        int result = ParticleCount - particles.Count;
        return result < 0 ? 0 : result;
    }
    public bool SpawnParticle(float particleMass, Color particleColor, Transform parent, Vector2 velocity, float spread, float viscosity)
    {
        if (particles.Count >= ParticleCount)
        {
            return false;
        }
        GameObject obj = Instantiate(objectToSpawn, parent.position, parent.rotation, transform);
        obj.SetActive(true);
        ParticleScript particle = obj.GetComponent<ParticleScript>();
        particle.Mass = particleMass;
        particle.Viscosity = viscosity;
        particle.velocity = velocity;
        if (spread != 0)
        {
            obj.transform.position += new Vector3(UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread), 0);
            particle.velocity += new Vector2(UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread));
        }
        particle.SetColor(particleColor);
        particles.Add(particle);
        return true;
    }

    public void ClearAllParticles()
    {
        foreach (var item in particles)
        {
            Destroy(item.gameObject);
        }
        particles.Clear();
    }
}
