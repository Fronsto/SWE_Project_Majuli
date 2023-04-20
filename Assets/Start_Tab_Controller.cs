using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Start_Tab_Controller : MonoBehaviour
{
    public GameObject WorldSphere, tablet, leftHand;

    // Start is called before the first frame update
    void Start()
    {
        leftHand.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
    IEnumerator PlayVideoAndChangeScene(string scene) {
        var videoPlayer = WorldSphere.GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.enabled = true;
        videoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Transitions/ferry");
        videoPlayer.Play();
        tablet.SetActive(false);
        while(videoPlayer.isPlaying) {
            yield return null;
        }
        videoPlayer.enabled = false;
        SceneManager.LoadScene(scene);
       
    }
    public void Devotee()
    {
        StartCoroutine(PlayVideoAndChangeScene("Devotee"));
    }

    public void Traveller()
    {
        StartCoroutine(PlayVideoAndChangeScene("Traveller"));
    }

    public void Expert()
    {
        StartCoroutine(PlayVideoAndChangeScene("Expert"));
    }
}
