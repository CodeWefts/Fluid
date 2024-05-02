using Assets;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets
{
    public class ParticleDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ParticleScript other = collision.attachedRigidbody.GetComponent<ParticleScript>();
            if (other) ParticleSpawner.SpawnerInstance.RemoveParticle(other);
        }
    }
}
