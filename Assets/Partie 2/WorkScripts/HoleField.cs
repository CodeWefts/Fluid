using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoleField : MonoBehaviour
{
    public TMP_InputField HoleSize;
    public TMP_InputField HoleHeight;
    private HolesSettings settings;
    private FluidScript fluidScript;
    public int index = 0;
    // Start is called before the first frame update
    void Awake()
    {
        settings = FindObjectOfType<HolesSettings>();
        fluidScript = FindObjectOfType<FluidScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHoleSizeChanged()
    {
        var hole = fluidScript.GetHole(gameObject);

        var text = HoleSize.text.Replace('.', ',');
        if (float.TryParse(text, out float value))
            hole.SetSize(Mathf.Max(value, 0.0f));
    }

    public void OnHoleHeightChanged()
    {
        var hole = fluidScript.GetHole(gameObject);

        var text = HoleHeight.text.Replace('.', ',');
        if (float.TryParse(text, out float value))
            hole.SetHeight(Mathf.Clamp(value, 0.0f, 4.0f));
    }
}
