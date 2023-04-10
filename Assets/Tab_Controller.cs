using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab_Controller : MonoBehaviour
{
    public GameObject[] tabs;
    public GameObject[] buttons;
    public string videoClip;
    // Start is called before the first frame update
    public void PlayVideo(){
        // disable other parts

        // play the video clip
        var videoPlayer = tabs[0].GetComponent<UnityEngine.Video.VideoPlayer>();
        if(videoClip != null)
        {
            Debug.Log("Playing video clip: " + videoClip);
            videoPlayer.enabled = true;
            videoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Videos/" + videoClip);
            videoPlayer.Play();
        } else {
            videoPlayer.enabled = false;
        }
    }
    void Start()
    {
        videoClip = null;
    }

    public void SetVideoContent(string videoClip)
    {
        this.videoClip = videoClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
