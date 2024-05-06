using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;

public class Height : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;
    [SerializeField] private float maxAmout = 3600.0f;
    [SerializeField] public float SliderlocalValue;

    [SerializeField] public Slider oriSlider;

    [Header("User's Values")]
    [SerializeField] public float initial_Height = 4.0f; // User can change this value
    [SerializeField] public float hole_diameter; // User can change this value
    [SerializeField] public float water_volumic_mass = 1000; // User can change this value
    [SerializeField] public float hole_height = 0.0f; // User can change this value
    [SerializeField] private float max_height = 4.0f;

    [Header("GameObjects")]
    [SerializeField] public GameObject ori; // The cylinder representing the orifice

    [Header("Initial Values")]
    [SerializeField] private float waterTower_surface;
    [SerializeField] private float waterTower_diameter = 4.0f;
    [SerializeField] private float hole_surface = 0.0004f;
    [SerializeField] private float total_Time;

    [Header("Output's Values")]
    [SerializeField] private TMP_Text distanceTXT;
    [SerializeField] private TMP_Text current_HeightTXT;
    [SerializeField] private TMP_Text current_timeTXT;
    [SerializeField] private TMP_Text initial_speed_oriTXT;
    [SerializeField] private TMP_Text hole_diameterTXT;
    [SerializeField] private TMP_Text hole_surfaceTXT;

    [SerializeField] private float distance;
    [SerializeField] private float current_Height;
    [SerializeField] private float current_time;
    [SerializeField] private float current_additional_time;
    [SerializeField] private float initial_speed_ori;

    [Header("UI boolean")]
    [SerializeField] private bool isStart = false;
    [SerializeField] private bool isPaused = true;
    [SerializeField] private bool isFill = true;

    [Header("Water Height")]
    public List<GameObject> points;
    public int nbrOfPoints = 100;
    [SerializeField] private Material waterMaterial;

    private float water_height;

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void NextScene()
    {
        SceneManager.LoadScene("SPH");
    }

    // Start is called before the first frame update
    private void Start()
    {
        water_height = gameObject.transform.localScale.y;
        // User cannot change this value (so we don't need conditions)
        waterTower_surface = Mathf.Pow(waterTower_diameter / 2.0f, 2) * Mathf.PI;

        // User can change this value
        if (hole_diameter == 0.0f)
            hole_diameter = Mathf.Sqrt(hole_surface / Mathf.PI) * 2.0f;
        if (hole_diameter != 0.0f)
            hole_surface = Mathf.Pow(hole_diameter / 2.0f, 2) * Mathf.PI;
        hole_diameterTXT.text  = hole_diameter.ToString("0.00");
        hole_surfaceTXT.text = (hole_surface*100*100).ToString("0.00");

        // Initial values
        current_Height = initial_Height;
        initial_speed_ori = SpeedOri();
        distance = Distance();
        total_Time = TotalTime();
        InitializeOrifice();
    }

    // Update is called once per frame
    private void Update()
    {
        hole_height = oriSlider.value;
        ori.transform.position = new Vector3((0.5f + 0.1f / 2.0f), (0.5f * 2.0f ) + (hole_height / 4.0f), (0.0f)); // POSITION

        if (isFill)
        {
            current_Height = initial_Height; // change by initial value
            current_time = 0.0f;
            isFill = false;
        }

        if (current_Height > hole_height && isStart && !isPaused)
        {
            //Timers
            current_additional_time += Time.deltaTime * SliderlocalValue;
            current_time += Time.deltaTime * SliderlocalValue;
            current_timeTXT.text = current_time.ToString("0.00");

            // Height
            current_Height = HeightTime(); // real value
            current_HeightTXT.text = current_Height.ToString("0.00");

            // Speed
            initial_speed_ori = SpeedOri();
            initial_speed_oriTXT.text = initial_speed_ori.ToString("0.00");

            // Distance
            distance = Distance();
            distanceTXT.text = distance.ToString("0.00");

            water_height = current_Height / max_height; // current height in unity scale ( water tower = 4m , in unity is 1m )
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, water_height, gameObject.transform.localScale.z);
            //gameObject.transform.position = new Vector3(gameObject.transform.position.x, hole_height, gameObject.transform.position.z);

            SpawnProjectile();
        }
    }
    // INPUTS OUTPUTS ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // -------------- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    public void ReadRhoStringInput(string s)
    {
        var input = s;
        water_volumic_mass = float.Parse(input);
    }
    public void ReadHeightStringInput(string s)
    {
        if(float.Parse(s) > 4.0f)
        {
            Debug.Log("Height is too high");
        }
        else
        {
            var input = s;
            initial_Height = float.Parse(input);
            current_Height = initial_Height;
        }
    }
    public void ReadDiamOriStringInput(string s)
    {
        var input = s;
        if (float.Parse(input) < waterTower_diameter)
        {
            hole_diameter = float.Parse(input);
            hole_surface = Mathf.Pow(hole_diameter / 2.0f, 2) * Mathf.PI;

            ori.transform.localScale = new Vector3(hole_diameter/4.0f, 0.1f / 2.0f, hole_diameter/4.0f); // SIZE
            //ori.transform.position = new Vector3((0.5f + 0.1f / 2.0f), (0.5f * 2.0f + hole_diameter / 2.0f) + (hole_height / 4.0f), (0.0f)); // POSITION

            isFill = true;

            // Update the text
            hole_diameterTXT.text = hole_diameter.ToString("0.00");
            hole_surfaceTXT.text = (hole_surface * 100 * 100).ToString("0.00");
        }
        
    }
    public void SliderChange(float value)
    {
        SliderlocalValue = value * maxAmout;
        sliderText.text = SliderlocalValue.ToString("0");
    }

    public void OnClickListener(string name)
    {
        if (name == "fill")
        {
            isFill = true;
        }
        else if (name == "play")
        {
            isStart = true;
            isPaused = false;
        }
        else if (name == "pause")
        {
            isPaused = true;
            isStart = false;

        }
    }

    // INITIALIZATION ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // -------------- ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
    private void InitializeOrifice()
    {
        if (ori == null)
            ori = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ori.transform.localScale = new Vector3(hole_diameter, 0.1f / 2.0f, hole_diameter); // SIZE
        ori.transform.position = new Vector3((0.5f + 0.1f / 2.0f), (0.5f * 2.0f + hole_diameter / 2.0f) + (hole_height / 4.0f), (0.0f)); // POSITION
        ori.transform.Rotate(0.0f, 0.0f, 90.0f); // ROTATION
    }

    private float SpeedOri()
    {
        return Mathf.Sqrt(2.0f * 9.81f * (current_Height - hole_height)); // inital speed ori
    }

    private float Distance()
    {
        return initial_speed_ori * Mathf.Sqrt((2.0f * current_Height) / 9.81f);
    }

    private float TotalTime()
    {
        float calc1 = Mathf.Sqrt(hole_height) - Mathf.Sqrt(initial_Height);
        float calc2 = (-hole_surface / waterTower_surface) * Mathf.Sqrt(9.81f / 2.0f);
        return calc1 / calc2;
    }

    public float HeightTime()
    {
        float calc2 = (hole_surface / waterTower_surface) * Mathf.Sqrt(9.81f / 2.0f);
        float calc = Mathf.Sqrt(initial_Height) - (calc2 * current_time) ;

        if(calc < 0.0f)
            return 0.0f;
       return Mathf.Pow(calc, 2) ; // current height
    }

    // WATER FALLING ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ------------- ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    public void SpawnProjectile()
    {
        GameObject waterBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        waterBall.transform.position = ori.transform.position;
        waterBall.transform.localScale = ori.transform.localScale;
        waterBall.transform.Rotate(90.0f, 0.0f, 0.0f);
        waterBall.GetComponent<Renderer>().material = waterMaterial;

        StartCoroutine(UpdateWaterBall(waterBall));
    }

    private System.Collections.IEnumerator UpdateWaterBall(GameObject waterBall)
    {
        float time = 0.0f;
        while (waterBall != null && waterBall.transform.position.y > 0)
        {
            time += Time.deltaTime;

            float xPos = ori.transform.position.x + ori.transform.localScale.y / 2.0f + ori.transform.localScale.x * 0.5f + initial_speed_ori * time;
            float yPos = ori.transform.position.y - 0.5f * 9.81f * time * time;

            waterBall.transform.position = new Vector3(xPos, yPos, 0);

            yield return null;

            // Vérifie si le waterBall a atteint le sol
            if (yPos <= 0)
            {
                Destroy(waterBall);
            }
        }
    }
}