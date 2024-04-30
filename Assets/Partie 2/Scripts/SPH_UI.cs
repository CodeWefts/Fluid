using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

using UnityEngine;
using System.Threading;

public struct SPH_UI_Refs
{
    public UnityEngine.UI.Button playSPH;
    public UnityEngine.UI.Button Restart;
    public UnityEngine.UI.Button loadPart1;
};


public class SPH_UI : MonoBehaviour
{
    public GameObject door;
    public bool isOpen = false;

    public Vector3 openPosition;
    public float openSpeed = 0.0001f;
    private Vector3 initialPosition;

    public SPH_UI_Refs UiRefs;

    private float Lockpercent = 0;

    void Start()
    {
        UiRefs = FetchUIRefs();
        UiRefs.playSPH.onClick.AddListener(PlaySimulation);
        UiRefs.Restart.onClick.AddListener(OpenDoor);
        UiRefs.loadPart1.onClick.AddListener(OnLoadScene1Clicked);

        initialPosition = door.transform.position;
    }

    void Update()
    {
        if (isOpen)
        {
            Lockpercent += openSpeed * Time.deltaTime;
            door.transform.position = Vector3.Lerp(initialPosition, initialPosition + openPosition, Lockpercent);

            if (Lockpercent >= 1.0f)
            {
                StartCoroutine(CloseDoorAfterDelay());
            }
        }
        else
        {
            if (door.transform.position != initialPosition)
            {
                Lockpercent += openSpeed * Time.deltaTime;
                door.transform.position = Vector3.Lerp(door.transform.position, initialPosition, Lockpercent);
            }
        }
    }

    IEnumerator CloseDoorAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        isOpen = false;
        Lockpercent = 0f;
    }

    public void PlaySimulation()
    {
       
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
        }
    }

    public SPH_UI_Refs FetchUIRefs()
    {
        SPH_UI_Refs refs = new SPH_UI_Refs();
        refs.playSPH = GameObject.Find("PlayButton").GetComponent<UnityEngine.UI.Button>();
        refs.Restart = GameObject.Find("Restart").GetComponent<UnityEngine.UI.Button>();
        refs.loadPart1 = GameObject.Find("LoadScene1").GetComponent<UnityEngine.UI.Button>();

        return refs;
    }

    public void OnLoadScene1Clicked()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
