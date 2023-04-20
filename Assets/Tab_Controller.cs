using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tab_Controller : MonoBehaviour
{
    public InputActionProperty leftSelectValue;
    public GameObject tablet, startMenu, continueMenu, mainMenu;
    [SerializeField] Scene sceneScript; 

    // Start is called before the first frame update
    void Start()
    {
        tablet.SetActive(true);
        startMenu.SetActive(true);
        continueMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Perform action if button is pressed
        // if(leftSelectValue.action.triggered) {
        //     tablet.SetActive(true);
        // }
        // else{
        //     tablet.SetActive(false);
        // }
        if(Input.GetKeyDown(KeyCode.Space)) {
            tablet.SetActive(!tablet.activeSelf);
        }
    }

    // Change to Scene Main
    public void ChangeToMain()
    {
        sceneScript.JumpToLocation(1,2);
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        continueMenu.SetActive(false);
    }

    // Change to Scene Tutorial
    public void ChangeToTutorial()
    {
        
    }

    // Exit Application
    public void ExitApplication()
    {
        Application.Quit();
    }

    // Exit Menu
    public void ChangeToStart()
    {
        startMenu.SetActive(true);
        mainMenu.SetActive(false);
        continueMenu.SetActive(false);
        sceneScript.JumpToSite("IntroVideo");
    }

    // Continue
    public void Continue()
    {
        continueMenu.SetActive(false);
        mainMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    // Main Menu
    public void MainMenu()
    {
        continueMenu.SetActive(true);
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
    }
}