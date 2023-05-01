using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tab_Controller : MonoBehaviour
{
    public InputActionProperty leftSelectValue;
    public GameObject tablet, video, map, menu, videoPlayerObject, tabInfoObject, leftHand, scrollTab;
    bool coolDown = false;
    string videoName, textName, scene_name;
    public GameObject videoIcon, detailIcon;

    public void SetVideoContent(string name) {
        videoName = name;
        Map();
    }

    public void SetTextContent(string name) {
        textName = name;
        Map();
    }

    // Start is called before the first frame update
    void Start()
    {
        scene_name = SceneManager.GetActiveScene().name;
        tablet.SetActive(false);
        video.SetActive(false);
        map.SetActive(true);
        menu.SetActive(false);
        leftHand.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!coolDown && (leftSelectValue.action.ReadValue<float>() > 0.9f)) {
                if(!tablet.activeSelf){
                    tablet.SetActive(true);
                    videoIcon.SetActive(false);
                    detailIcon.SetActive(false);
                    leftHand.SetActive(false);
                } else{
                    tablet.SetActive(false);
                    if(videoName != null && videoName != "")
                        videoIcon.SetActive(true);
                    if(textName != null && textName != "")
                        detailIcon.SetActive(true);
                    leftHand.SetActive(true);
                }
                coolDown = true;
        } else if(leftSelectValue.action.ReadValue<float>() < 0.1f) {
            coolDown = false;
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            tablet.SetActive(!tablet.activeSelf);
            if(!tablet.activeSelf){
                    if(videoName != null && videoName != "")
                        videoIcon.SetActive(true);
                    if(textName != null && textName != "")
                        detailIcon.SetActive(true);
            } else {
                videoIcon.SetActive(false);
                detailIcon.SetActive(false);
            }
        }
    }

    // Start Video Player
    public void MediaPlayer()
    {
        video.SetActive(true);
        map.SetActive(false);
        menu.SetActive(false);
        var textContent = tabInfoObject.GetComponent<TMPro.TextMeshProUGUI>();
        if(scene_name != "Expert"){

            if(videoName == "") {
                videoPlayerObject.SetActive(false);
                scrollTab.SetActive(true);
                textContent.text = "No Video available !";
            } else {
                scrollTab.SetActive(false);
                videoPlayerObject.SetActive(true);
                var videoPlayer = videoPlayerObject.GetComponent<UnityEngine.Video.VideoPlayer>();
                videoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Videos/" + videoName);
                videoPlayer.Play(); 
            }
        }
        else {
            Debug.Log("scene_name: " + scene_name);
            scrollTab.SetActive(true);
            videoPlayerObject.SetActive(false);
            if(textName == ""){
                textContent.text = "No Text available !";
            } else {
                textContent.text = Resources.Load<TextAsset>("InfoText/" + textName).text;
            }
        }
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