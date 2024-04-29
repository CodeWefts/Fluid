using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

using UnityEngine;

public struct SPH_UI_Refs
{
    public UnityEngine.UI.Button playSPH;
    public UnityEngine.UI.Button RemoveFluid;
    public UnityEngine.UI.Button loadPart1;
};


public class SPH_UI : MonoBehaviour
{
    public GameObject door;
    public bool isOpen = false;

    public Vector3 openPosition;
    public float openSpeed = 2f;
    private Vector3 initialPosition;

    public SPH_UI_Refs UiRefs;

    void Start()
    {
        UiRefs = FetchUIRefs();
        UiRefs.playSPH.onClick.AddListener(PlaySimulation);
        UiRefs.RemoveFluid.onClick.AddListener(OpenDoor);
        UiRefs.loadPart1.onClick.AddListener(OnLoadScene1Clicked);

        initialPosition = door.transform.position;
    }

    void Update()
    {
        if (isOpen)
        {
            door.transform.position = Vector3.Lerp(initialPosition, openPosition, openSpeed * Time.deltaTime);
        }
    }

    public void PlaySimulation()
    {
       
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public SPH_UI_Refs FetchUIRefs()
    {
        SPH_UI_Refs refs = new SPH_UI_Refs();
        refs.playSPH = GameObject.Find("PlayButton").GetComponent<UnityEngine.UI.Button>();
        refs.RemoveFluid = GameObject.Find("RemoveFluid").GetComponent<UnityEngine.UI.Button>();
        refs.loadPart1 = GameObject.Find("LoadScene1").GetComponent<UnityEngine.UI.Button>();

        return refs;
    }

    public void OnLoadScene1Clicked()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
