using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tab_Controller : MonoBehaviour
{
    public InputActionProperty leftSelectValue;
    public GameObject tablet, video, map, menu, videoPlayerObject, leftHand, rightHand;
    [SerializeField] Scene sceneScript; 

    // Start is called before the first frame update
    void Start()
    {
        tablet.SetActive(true);
        video.SetActive(false);
        map.SetActive(true);
        menu.SetActive(false);
        leftHand.SetActive(true);
        rightHand.SetActive(true);
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

    // Start Video Player
    public void VideoPlayer()
    {
        video.SetActive(true);
        map.SetActive(false);
        menu.SetActive(false);
        var videoPlayer = videoPlayerObject.GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Videos/akash_banti");
        videoPlayer.Play();
    }

    // Show Map
    public void Map()
    {
        video.SetActive(false);
        map.SetActive(true);
        menu.SetActive(false);
    }

    // Show Menu
    public void Menu()
    {
        video.SetActive(false);
        map.SetActive(false);
        menu.SetActive(true);
    }

    // Exit To Start Scene
    public void Exit()
    {
        SceneManager.LoadScene("Start");
    }
}