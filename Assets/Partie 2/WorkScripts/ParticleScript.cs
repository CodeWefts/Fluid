using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    struct CollisionHolder
    {
        public ParticleScript particle;
        public float lengthSqr;
        public float len;
    }

    public class ParticleScript : MonoBehaviour
    {
        
        public float Mass = 1.0f;
        public float Viscosity = 0.018f;
        private List<CollisionHolder> particles = new List<CollisionHolder>();

        public float cachedDensity;
        public float cachedPressure;
        public Vector2 cachedAcceleration;
        public Vector2 velocity;
        private Rigidbody2D body;
        private SpriteRenderer sprite;

        public void Start()
        {
            body = GetComponent<Rigidbody2D>();
            if (body == null)
            {
                print("missing Rigidbody2D");
            }
            body.gravityScale = 0;
            sprite = GetComponent<SpriteRenderer>();
            if (sprite == null)
            {
                print("missing sprite");
            }
        }

        public void SetColor(Color colorIn)
        {
            if (!sprite) sprite = GetComponent<SpriteRenderer>();
            sprite.color = colorIn;
        }

        public void UpdateDensity(float poly6Coefficient, float SmoothingRadius, float RefDensity, float PressureConstant)
        {
            cachedDensity = 0.0f;
            foreach (var item in particles)
            {
                cachedDensity += item.particle.Mass * poly6Coefficient * MathF.Pow(MathF.Pow(SmoothingRadius, 2) - item.lengthSqr, 3);
            }
            cachedDensity = MathF.Max(cachedDensity, RefDensity);
            cachedPressure = PressureConstant * (cachedDensity - RefDensity);
            if (!sprite)
            {
                body = GetComponent<Rigidbody2D>();
                body.gravityScale = 0;
                sprite = GetComponent<SpriteRenderer>();
            }
            //sprite.color = new Color(particles.Count / 10.0f, 0.0f, 0.0f);
            float factor = MathF.Min(1 + particles.Count / 10.0f, 4);
            sprite.size = new Vector2(factor, factor);
        }

        public void UpdateAccel(float SmoothingRadius, float spikeyGradCoefficient, Vector2 gravityForce, float maximumAcceleration, float motionDampingCoefficient)
        {
            cachedAcceleration = Vector2.zero;
            foreach (var item in particles)
            {
                float diff = SmoothingRadius - item.len;
                float spikey = spikeyGradCoefficient * diff * diff;
                float massRatio = item.particle.Mass / Mass;
                float pterm = (cachedPressure + item.particle.cachedPressure) / (2 * cachedDensity * item.particle.cachedDensity);
                Vector3 dir = item.particle.transform.position - transform.position;
                cachedAcceleration += (massRatio * pterm * spikey) * new Vector2(dir.x, dir.y).normalized;

                float lap = -spikeyGradCoefficient * diff;
                Vector2 vdiff = item.particle.velocity - velocity;
                cachedAcceleration += (Viscosity * massRatio * (1 / item.particle.cachedDensity) * lap) * vdiff;
            }
            cachedAcceleration += gravityForce;
            float mag = cachedAcceleration.magnitude;
            
            if (motionDampingCoefficient > 0)
            {
                Vector2 damp = velocity * motionDampingCoefficient;
                if (damp.magnitude > mag)
                {
                    cachedAcceleration = Vector3.zero;
                }
                else
                {
                    cachedAcceleration -= damp;
                }
            }
            
            if (mag > maximumAcceleration)
            {
                cachedAcceleration = (cachedAcceleration / mag) * maximumAcceleration;
                //print("maximum acceleration was reached");
            }
            velocity += cachedAcceleration * Time.fixedDeltaTime;
            body.velocity = velocity;
            particles.Clear();
        }
        public void OnTriggerStay2D(Collider2D collision)
        {
            ParticleScript other = collision.gameObject.GetComponent<ParticleScript>();
            if (other != null)
            {
                foreach (var item in particles)
                {
                    if (item.particle == other) return;
                }
                CollisionHolder result;
                result.particle = other;
                result.lengthSqr = (other.transform.position - transform.position).sqrMagnitude;
                result.len = MathF.Sqrt(result.lengthSqr);
                particles.Add(result);
            }
        }

        public void OnCollisionStay2D(Collision2D collision)
        {
            Vector2 normal = collision.GetContact(0).normal;
            Vector2 rotated = new Vector2(normal.y, -normal.x); 
            velocity = Vector2.Dot(rotated, velocity) * rotated;
            velocity += normal * 0.5f;
        }
    }
}
