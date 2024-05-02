using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidScript : MonoBehaviour
{
    [SerializeField] public float CurrentTime = 0;
    [SerializeField] public float Surface = 16f;
    [SerializeField] public float InitialHeight = 4f;
    [SerializeField] public float Pressure = 1.01325f;
    [SerializeField] public float VolumicMass = 1000f;
    [SerializeField] public float TimeScale = 1f;

    public float Gravity = 9.81f;

    public Hole TemplateHole;
    public Dictionary<GameObject, Hole> Holes = new Dictionary<GameObject, Hole>();

    public float CurrentHeight;

    WaterSquare waterSquare;
    // Start is called before the first frame update
    void Start()
    {
        waterSquare = FindObjectOfType<WaterSquare>();
        CurrentHeight = InitialHeight;
    }

    public Hole AddHole(GameObject gameobject)
    {
        GameObject newHole = Instantiate(TemplateHole.gameObject, TemplateHole.transform.parent);
        newHole.SetActive(true);
        var hole = newHole.GetComponent<Hole>();
        Holes.Add(gameobject, hole);
        return hole;
    }

    public Hole GetHole(GameObject gameobject)
    {
        return Holes[gameobject];
    }

    public void RemoveHole(GameObject gameobject)
    {
        var hole = Holes[gameobject];
        Holes.Remove(gameobject);
        Destroy(hole.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        float heightDelta = 0;
        foreach (var hole in Holes)
        {
            float orificeToSurfaceRatio = hole.Value.OrificeSurface / Surface;
            float orificeSize = Mathf.Sqrt(hole.Value.OrificeSurface);
            float squareOrificeToSurface = orificeToSurfaceRatio * orificeToSurfaceRatio;
            float heightWithPressure = InitialHeight - (hole.Value.OrificeHeight) + Pressure / (VolumicMass * Gravity);
            float sqrtTerm = Mathf.Sqrt(2f * Gravity * (heightWithPressure) / (1f - squareOrificeToSurface));

            if (CurrentHeight > hole.Value.OrificeHeight)
            {
                hole.Value.LastTime = CurrentTime;
            }

            float height = orificeToSurfaceRatio * sqrtTerm * hole.Value.LastTime;
            heightDelta += height;

            hole.Value.Velocity = Mathf.Sqrt(2f * Gravity * (CurrentHeight - hole.Value.OrificeHeight + Pressure / (VolumicMass * Gravity)) / (1f - squareOrificeToSurface));
            hole.Value.UpdateRenderer();
        }

        CurrentHeight = InitialHeight - heightDelta;

        // Set values to renderer
        //waterSquare.Velocity = speed;
        waterSquare.Percent = (CurrentHeight / InitialHeight) * 100f;

        CurrentTime += Time.deltaTime * TimeScale;
        CurrentTime = Mathf.Clamp(CurrentTime, 0f, float.MaxValue);
    }

    public void FillIn()
    {
        CurrentTime = 0f;
        CurrentHeight = InitialHeight;
    }
}
