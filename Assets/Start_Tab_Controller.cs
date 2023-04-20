using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Start_Tab_Controller : MonoBehaviour
{
    public GameObject WorldSphere, tablet, leftHand;
    public Material sphereBox;

    // Start is called before the first frame update
    void Start()
    {
        leftHand.SetActive(true);
        sphereBox.SetTexture("_MainTex", Resources.Load<Texture>("InitialTexture"));
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

        // reduce exposure then increase it again
        var exposure = sphereBox.GetFloat("_Exposure");
        while(exposure > 0.0f) {
            exposure -= 0.01f;
            sphereBox.SetFloat("_Exposure", exposure);
            yield return new WaitForSeconds(0.01f);
        }
        while(exposure < 1.0f) {
            exposure += 0.01f;
            sphereBox.SetFloat("_Exposure", exposure);
            yield return new WaitForSeconds(0.01f);
        }

        // wait for video to finish
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
