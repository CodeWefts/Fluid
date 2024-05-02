using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterSquare : MonoBehaviour
{
    [Range(0, 100)]
    public float Percent = 100f;
    public float FluidHeight = 4f;
    public float OutputSize = 0.0004f;

    [Tooltip("Velocity in m/s")]
    public float Velocity = 7.67f;

    public int ParticleNumber = 1000;

    private float defaultSize;
    private float defaultPosition;
    private ParticleSystem fluidParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        defaultSize = transform.localScale.y;
        defaultPosition = gameObject.transform.position.y;
        fluidParticleSystem = FindObjectOfType<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set Position and Scale.
        //Vector3 scale = new Vector3(gameObject.transform.localScale.x, (defaultSize * 0.01f) * Mathf.Max(Percent, 0f), gameObject.transform.localScale.z);
        Vector3 scale = new Vector3(transform.localScale.x, Mathf.Max((defaultSize * 0.01f) * Percent, 0f), transform.localScale.z);
        transform.localScale = scale;
        Vector3 position = new Vector3(transform.position.x, (transform.localScale.y / 2) - defaultPosition, transform.position.z);
        transform.position = position;

    }
}
