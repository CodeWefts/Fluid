using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void LoadPart1()
    {
        SceneManager.LoadScene("Part1");
    }

    public void LoadPart2()
    {
        SceneManager.LoadScene("Part2");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}