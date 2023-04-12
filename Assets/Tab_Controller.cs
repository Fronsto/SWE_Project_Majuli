using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tab_Controller : MonoBehaviour
{
    public InputActionProperty rightSelectAction; // Trigger
    public InputActionProperty leftSelectAction;
    public GameObject tablet;

    // Start is called before the first frame update
    void Start()
    {
        tablet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(leftSelectAction.action.ReadValue<float>() > 0.8f || rightSelectAction.action.ReadValue<float>() > 0.8f) {
            tablet.SetActive(true);
        }
        else{
            tablet.SetActive(false);
        }
    }

    // Change to Scene Main
    public void ChangeToMain()
    {
        SceneManager.LoadScene("Main");
    }

    // Change to Scene Tutorial
    public void ChangeToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // Exit Application
    public void ExitApplication()
    {
        Application.Quit();
    }
}