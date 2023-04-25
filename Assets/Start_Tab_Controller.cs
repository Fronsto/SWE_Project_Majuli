using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Start_Tab_Controller : MonoBehaviour
{
    public GameObject WorldSphere, tablet, leftHand, rightHand, introVideo, startMenu;
    public Material sphereBox;
    string sceneToBeLoaded;
    public GameObject leftRay, rightRay;

    // Start is called before the first frame update
    void Start()
    {
        leftHand.SetActive(true);
        rightHand.SetActive(true);
        introVideo.SetActive(false);
        startMenu.SetActive(true);
        sceneToBeLoaded = "";
        sphereBox.SetTexture("_MainTex", Resources.Load<Texture>("InitialTexture"));
        sphereBox.SetFloat("_Exposure", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitApplication() {
        Application.Quit();
    }

    IEnumerator PlayVideoAndChangeScene() {
        // disable left and right ray
        leftRay.SetActive(false);
        rightRay.SetActive(false);

        var videoPlayer = WorldSphere.GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Transitions/ferry");
        tablet.SetActive(false);

        // reduce exposure then increase it again
        var exposure = sphereBox.GetFloat("_Exposure");
        while(exposure > 0.0f) {
            exposure -= 0.01f;
            sphereBox.SetFloat("_Exposure", exposure);
            yield return new WaitForSeconds(0.01f);
        }
        videoPlayer.Play();
        while(exposure < 1.0f) {
            exposure += 0.01f;
            sphereBox.SetFloat("_Exposure", exposure);
            yield return new WaitForSeconds(0.01f);
        }

        // wait for video to finish
        while(videoPlayer.isPlaying) {
            yield return null;
        }
        SceneManager.LoadScene(sceneToBeLoaded);
       
    }

    IEnumerator PlayIntroVideo() {
        startMenu.SetActive(false);
        introVideo.SetActive(true);
        var introVideoPlayer = introVideo.GetComponent<UnityEngine.Video.VideoPlayer>();
        introVideoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Videos/akash_banti");
        introVideoPlayer.loopPointReached += OnVideoFinished;
        introVideoPlayer.Play();
        // wait for video to finish
        while(introVideoPlayer.isPlaying) {
            yield return null;
        }
    }

    void OnVideoFinished(VideoPlayer vp){
        StartCoroutine(PlayVideoAndChangeScene());
    }

    public void Devotee() {
        sceneToBeLoaded = "Devotee";
        StartCoroutine(PlayIntroVideo());
    }

    public void Traveller() {
        sceneToBeLoaded = "Traveller";
        StartCoroutine(PlayIntroVideo());
    }

    public void Expert() {
        sceneToBeLoaded = "Expert";
        StartCoroutine(PlayIntroVideo());
    }

    public void Back() {
        startMenu.SetActive(true);
        introVideo.SetActive(false);
    }

    public void SkipIntro() {
        StartCoroutine(PlayVideoAndChangeScene());
    }
}
