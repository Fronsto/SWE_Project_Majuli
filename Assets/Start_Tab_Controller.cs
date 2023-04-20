using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Start_Tab_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void Devotee()
    {
        SceneManager.LoadScene("Devotee");
    }

    public void Traveller()
    {
        SceneManager.LoadScene("Traveller");
    }

    public void Expert()
    {
        SceneManager.LoadScene("Expert");
    }
}
