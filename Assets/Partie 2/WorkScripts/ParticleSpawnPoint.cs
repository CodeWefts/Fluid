using Assets;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets
{
    public class ParticleSpawnPoint : MonoBehaviour
    {
        public bool isActive = true;
        public float spawnRate = 1.0f;
        public float particleMass = 1.0f;
        public float particleViscosity = 0.018f;
        public float randomSpread = 0.1f;
        public int particleRequiredCount = 20;
        public Vector2 outputVelocity;
        public Color particleColor = new Color(0.0f, 0.6822f, 1.0f);

        public float minParticleStat = 0.001f;

        private float spawnDelay = 0;
        private Transform render;
        private bool isSpawning = true;

        public void FixedUpdate()
        {
            if (!render) render = transform;
            Vector2 vel = outputVelocity + ParticleSpawner.SpawnerInstance.gravityForce * 0.1f;
            if (vel.sqrMagnitude < minParticleStat)
            {
                render.rotation = Quaternion.identity;
                return;
            }
            render.rotation = Quaternion.AngleAxis(MathF.Atan2(vel.y, vel.x) / MathF.PI * 180.0f, new Vector3(0,0,1));
        }

        public void SpawnParticles(ParticleSpawner spawner)
        {
            if (isSpawning && spawner.GetAvailableParticleCount() == 0)
            {
                isSpawning = false;
            }
            else if (!isSpawning && spawner.GetAvailableParticleCount() >= particleRequiredCount)
            {
                isSpawning = true;
            }
            if (!isSpawning) return;
            if (spawnRate < minParticleStat) spawnRate = minParticleStat;
            if (particleMass < minParticleStat) particleMass = minParticleStat;
            spawnDelay -= 1.0f;
            while (spawnDelay <= 0.0f)
            {
                if (!spawner.SpawnParticle(particleMass, particleColor, transform, outputVelocity, randomSpread, particleViscosity))
                {
                    while (spawnDelay <= 0.0f) spawnDelay += 1/spawnRate;
                    isSpawning = false;
                    break;
                }
                spawnDelay += 1 / spawnRate;
            }
        }

        public void ResetSpawnDelay()
        {
            spawnDelay = 0;
        }

        public void ToggleActive()
        {
            isActive = !isActive;
        }
    }
}
