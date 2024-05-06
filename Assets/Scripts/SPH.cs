using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class SPHProperties : MonoBehaviour
{
    public float density;
    public float pressure;
}

public class SPH : MonoBehaviour
{
    public GameObject particlePrefab;
    public float spawnInterval = 0.00f;
    
    private List<GameObject> particles = new List<GameObject>();

    float k = 2f; // pressure constant
    private float h = 3f; // distance of a particle 

    private float mass = 4.19f; // Kg Mass
    private float viscosity = 10e-3f; // Pa.s 

    int index = 0;
    Vector2 gravity = new Vector2(0, -9.81f);

    [Header("UI boolean")]
    [SerializeField] public Button playButton;
    [SerializeField] public Button resetButton;

    [SerializeField] private TextMeshProUGUI massText = null;
    public float massLocalValue = 4.19f;

    [SerializeField] private TextMeshProUGUI viscosityText = null;
    public float viscosityLocalValue = 10e-3f;
    private bool isPlaying = false;
    private bool isReset = false;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Start()
    {
        massText.text = mass.ToString("0.00");
        viscosityText.text = viscosity.ToString("0.00");
    }

    public void NextScene()
    {
        SceneManager.LoadScene("Fluid");
    }

    public void Play()
    {
        isPlaying = true;
    }
    public void ResetPlay()
    {
        isReset = true;
    }

    public void Mass(float value)
    {
        massLocalValue = value;
        massText.text = massLocalValue.ToString("0.00");
        mass = massLocalValue * 1000.0f;
    }

    public void Viscosity(float value)
    {
        viscosityLocalValue = value;
        viscosityText.text = viscosityLocalValue.ToString("0.00");
        viscosity = viscosityLocalValue;
    }

    // UPDATES //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // -------- ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Update()
    {
        if (isPlaying)
        {
            if (Time.time > spawnInterval)
            {
                spawnInterval = Time.time + 0.1f;
                if (index < 150)
                {
                    index++;
                    SpawnParticles();
                }
                UpdateSPH();
            }
        }
        if (isReset)
        {
            isReset = false;
            isPlaying = false;
            index = 0;
            foreach (GameObject particle in particles)
            {
                Destroy(particle);
            }
            particles.Clear();
        }

    }
    void UpdateSPH()
    {
        foreach (GameObject particle in particles)
        {
            SPHProperties props = particle.GetComponent<SPHProperties>();
            props.density = CalculateDensity(particle.transform.position);
            props.pressure = CalculatePressure(props.density);

        }
        foreach (GameObject particle in particles)
        {
            ApplyForces(particle);
        }
    }

    // PARTICLES //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // -------- ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void SpawnParticles()
    {
        Vector2 spawnPosition1 = new Vector2(Random.Range(0.0f, 2.0f), Random.Range(1.0f, 3.0f));
        particles.Add(CreateParticle(spawnPosition1));
    }

    GameObject CreateParticle(Vector2 spawnPosition)
    {
        if (particlePrefab == null)
        {
            Debug.LogError("Particle prefab is not assigned.");
            return null;
        }

        GameObject particle = Instantiate(particlePrefab, spawnPosition, Quaternion.identity);
        if (particle == null)
        {
            Debug.LogError("Failed to instantiate particle prefab.");
            return null;
        }

        particle.transform.localScale = new Vector3(1.0f, 1.0f, 1); // Particle size
        Rigidbody2D rb = particle.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = particle.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 1.0f;
        rb.mass = mass;

        SPHProperties props = particle.GetComponent<SPHProperties>();
        if (props == null)
        {
            particle.AddComponent<SPHProperties>(); // SPH Properties
        }

        return particle;
    }

    // CALCULS //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ------- //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    float Kernel(float distance)
    {
        //float distanceVar = distance * 10e-6f;
        if (distance >= h) return 0;

        float a = 315.0f / (64.0f * Mathf.PI * Mathf.Pow(h, 9));
        float b = Mathf.Pow((h * h) - (distance * distance), 3);

        return a * b;
    }
    float CalculateDensity(Vector2 position)
    {
        float density = 0;
        foreach (GameObject other in particles)
        {
            float distance = Vector2.Distance(position, other.transform.position);

            if (distance < h)
            {
                density += mass * Kernel(distance);
            }
        }
        return density;
    }

    float CalculatePressure(float density)
    {
        return k * (density - mass);
    }

    // PRESSURE //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // -------- //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Vector2 GradKernelSpiky(Vector2 r)
    {
        float rLength = r.magnitude;
        if (rLength == 0 || rLength > h) return Vector2.zero;

        float a = -45f / (Mathf.PI * Mathf.Pow(h, 6));
        float b = Mathf.Pow(h - rLength, 2);

        float coeff = a * 1.0f/rLength * b ;
        return new Vector2(coeff * r.x, coeff * r.y);
    }

    Vector2 CalculatePressureForce(GameObject particle, GameObject other)
    {
        SPHProperties props = particle.GetComponent<SPHProperties>();
        SPHProperties otherProps = other.GetComponent<SPHProperties>();

        // Pressure for both particles
        float p1 = props.pressure;
        float p2 = otherProps.pressure;

        // Density for both particles
        float rho1 = props.density;
        float rho2 = otherProps.density;

        Vector2 r = particle.transform.position - other.transform.position;
        Vector2 gradP = GradKernelSpiky(r);

        Vector2 force = -mass * ((p1 + p2) / (rho2 + rho2)) * gradP; // Selon le cours page 101
        return force;
    }

    // VISCOSITY //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // --------- //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    float LaplacianKernelViscosity(float r)
    {
        if (r >= h) return 0;
        return (45.0f / (Mathf.PI * Mathf.Pow(h, 6))) * (h - r);
    }

    Vector2 CalculateViscosityForce(GameObject particle, GameObject other)
    {
        SPHProperties otherProps = other.GetComponent<SPHProperties>();

        Vector2 u = particle.GetComponent<Rigidbody2D>().velocity;
        Vector2 uOther = other.GetComponent<Rigidbody2D>().velocity;
        Vector2 r = particle.transform.position - other.transform.position;

        float laplacianV = LaplacianKernelViscosity(r.magnitude);
        Vector2 force = viscosity * mass * ((uOther - u) / otherProps.density) * laplacianV;
        return force;
    }

    // APPLY FORCES //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ------------ //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void ApplyForces(GameObject particle)
    {
        Vector2 forcePressure = Vector2.zero;
        Vector2 forceViscosity = Vector2.zero;
        SPHProperties props = particle.GetComponent<SPHProperties>();

        foreach (GameObject other in particles)
        {
            if (other != particle)
            {
                forcePressure += CalculatePressureForce(particle, other);
                forceViscosity += CalculateViscosityForce(particle, other);
            }
        }
        props.GetComponent<Rigidbody2D>().velocity += gravity + ((forcePressure + forceViscosity) / props.density); // Selon le cours page 103
    }
}