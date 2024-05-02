using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HolesSettings : MonoBehaviour
{
    [SerializeField] GameObject templateHole;
    [SerializeField] Button AddButton;
    public List<GameObject> holesFields = new List<GameObject>();
    private FluidScript fluidScript;
    // Start is called before the first frame update
    void Start()
    {
        fluidScript = FindObjectOfType<FluidScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (holesFields.Count >= 3)
            AddButton.interactable = false;
        else
            AddButton.interactable = true;
    }

    public void RemoveHole()
    {
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        holesFields.Remove(clickedButton.transform.parent.gameObject);
        Destroy(clickedButton.transform.parent.gameObject);
        fluidScript.RemoveHole(clickedButton.transform.parent.gameObject);
    }

    public int GetFreeIndex()
    {
        int index = -1;
        for (int j = 1; j < 4; j++)
        {
            bool found = false;
            for (int i = 0; i < holesFields.Count; i++)
            {
                var comp = holesFields[i].GetComponent<HoleField>();
                if (comp.index == j)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                index = j;
                break;
            }
        }
        if (index == -1)
            index = 1;
        return index;
    }


    public void AddHole()
    {
        if (holesFields.Count >= 3)
            return;
        var go = Instantiate(templateHole, templateHole.transform.parent);
        go.SetActive(true);
        var hole = fluidScript.AddHole(go);
        var comps = go.GetComponent<HoleField>();
        comps.index = GetFreeIndex();
        comps.HoleSize.text = hole.OrificeSurface.ToString("F4");
        comps.HoleHeight.text = hole.OrificeHeight.ToString("F4");
        go.transform.Find("Hole name").GetComponent<TMP_Text>().text = $"Trou {comps.index}";
        holesFields.Add(go);
    }
}
