using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

struct SPH_UI_Refs
{
    public Button PlaySPH;
    public Button ReloadSPH;
    public Button LoadScene1Button;
};


public class SPH_UI : MonoBehaviour
{
    private SPH_UI_Refs UiRefs;

    // Start is called before the first frame update
    void Awake()
    {
        FetchUIRefs();
        UiRefs.LoadScene1Button.onClick.AddListener(OnLoadScene1Clicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FetchUIRefs()
    {
        UiRefs.ReloadSPH = GameObject.Find("PlaySPHButton").GetComponent<Button>();
        UiRefs.ReloadSPH = GameObject.Find("ReloadSPHButton").GetComponent<Button>();
        UiRefs.LoadScene1Button = GameObject.Find("LoadScene1Button").GetComponent<Button>();
    }

    public void OnLoadScene1Clicked()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
