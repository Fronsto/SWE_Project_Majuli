using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public InputActionProperty rightActionValue;
    public InputActionProperty leftActionValue;
    public GameObject WorldSphere;
    public string currentLoc;
    public string currentSite;
    public GameObject[] arrows;
    public Material sphereBox;
    Dictionary<String, SiteData> siteData;
    Dictionary<String, LocationData> locData;
    Dictionary<String, Texture> locTextures;
    bool lockInput = false;

    public GameObject VideoIcon;
    public GameObject DetailIcon;

    public GameObject NavigationTextObjectLeft;
    public GameObject NavigationTextObjectRight;
    public GameObject NavigationTextObjectUp;
    public GameObject NavigationTextObjectDown;

    [SerializeField] Tab_Controller tabController;
    [SerializeField] Map_Controller mapController;

    [Serializable]
    public class SiteData {
        public string initImage;
        public string ambientAudio;
    }
    [Serializable]
    public class SiteInfo {
        public string name;
        public SiteData data;
    }
    [Serializable]
    public class SitesWrapper {
        public SiteInfo[] Sites;
        public string[] JumpToTraveller;
        public string[] JumpToExpert;
        public string[] JumpToDevotee;
    }

    // Locations (within a site) data
    [Serializable]
    public class LocationData {
        public string up;
        public string down;
        public string left;
        public string right;

        public bool transition;

        public int rotation;
        public string ambientAudio;
        public string tabVideo;
        public string tabText;
        public string navTextDown;
        public string navTextUp;
        public string navTextLeft;
        public string navTextRight;

        public string siteName;
        public string initImage;
        public string transVid;
    }
    [Serializable]
    public class LocationInfo {
        public string name;
        public LocationData data;
    }
    [Serializable]
    public class LocsWrapper {
        public LocationInfo[] Locs;
    }

    ///////////////////////////////////////////////////////////////////////////
    // Loads the grid data from locData.json
    void LoadGrid(){
        locData = new Dictionary<String, LocationData>();
        // Read the file and use JSON utility to parse it , and create the locData map
        TextAsset mytxtData = (TextAsset)Resources.Load(currentSite + "/locData");

        string jsonString = mytxtData.text;
        LocsWrapper locs_ = JsonUtility.FromJson<LocsWrapper>(jsonString);
        LocationInfo[] arrayOfLocations = locs_.Locs;

        foreach (LocationInfo loc in arrayOfLocations) {
            locData.Add(loc.name, loc.data);
            Debug.Log("loaded location " + loc.name);
        }
    }
    ///////////////////////////////////////////////////////////////////////////
    // Loads the textures from 360images folder
    void LoadTextures() {
        locTextures = new Dictionary<String, Texture>();
        Texture2D[] textures = Resources.LoadAll<Texture2D>(currentSite + "/360images");
        foreach(Texture2D tex in textures){
            String name = tex.name;
            locTextures.Add(name, tex);
            Debug.Log("loaded texture " + name);
        }
    }
    ///////////////////////////////////////////////////////////////////////////
    // Jump to a location within the current site
    ///////////////////////////////////////////////////////////////////////////
    IEnumerator ChangeTexture(){
        // first fade out
        float fadeTime = 0.2f;
        float t = 0.0f;
        if(sphereBox.GetFloat("_Exposure") == 0.0f) {
            t = fadeTime;
        }
        while(t < fadeTime){
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, t/fadeTime);
            sphereBox.SetFloat("_Exposure", alpha);
            yield return null;
        }

        // Change the texture
        sphereBox.SetTexture("_MainTex", locTextures[currentLoc]);
        // set rotation of worldSphere based on z rotation of camera
        int rotation = locData[currentLoc].rotation;
        WorldSphere.transform.rotation = Quaternion.Euler(0, rotation, 0);

        // set the arrows
        SetDirectionMarkers();
        // set the audio
        SetAndPlayAmbientAudio();
        // set tablet video content
        SetTabletVideo();
        // set tablet text content
        SetTabletText();

        // then fade in
        t = 0.0f;
        while(t < fadeTime){
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0.0f, 1.0f, t/fadeTime);
            sphereBox.SetFloat("_Exposure", alpha);
            yield return null;
        }

    }
    void SetTabletVideo(){
        // set the videoContent to be played on the tablet
        string videoContent = "";
        if(SceneManager.GetActiveScene().name == "Expert") {
            VideoIcon.SetActive(false);
            return;
        }
        if(locData[currentLoc].tabVideo != null) {
            videoContent = locData[currentLoc].tabVideo;
            VideoIcon.SetActive(true);
        } 
        else{
            VideoIcon.SetActive(false);
        }
        // call the tablet methods to provide the video and image content
        tabController.SetVideoContent(videoContent);
    }

   void SetTabletText(){
        // set the textContent to be played on the tablet
        string textContent = "";
        if(SceneManager.GetActiveScene().name != "Expert") {
            DetailIcon.SetActive(false);
            return;
        }
        if(locData[currentLoc].tabText != null) {
            textContent = locData[currentLoc].tabText;
            DetailIcon.SetActive(true);
        } else{
            DetailIcon.SetActive(false);
        }
        // call the tablet methods to provide the video and image content
        tabController.SetTextContent(textContent);
   }
    void SetAndPlayAmbientAudio() {
        string audioToPlay = null;
        var audioSource = WorldSphere.GetComponent<AudioSource>();
        if(locData[currentLoc].ambientAudio != null) {
            audioToPlay = locData[currentLoc].ambientAudio;
        } else if(siteData[currentSite].ambientAudio != null) {
            audioToPlay = siteData[currentSite].ambientAudio;
        } 
        if(audioToPlay == null) {
            audioSource.Stop();
        } else{
            // first check which audio is playing currently
            string currentAudio = null;
            if(audioSource.clip != null){
                currentAudio = audioSource.clip.name;
            }
            if( currentAudio != audioToPlay) {
                audioSource.clip = Resources.Load<AudioClip>("Sounds/" + audioToPlay);
                audioSource.Play();
            }
        }
    }
    void SetDirectionMarkers() {
        foreach(GameObject arrow in arrows) arrow.SetActive(false);
        LocationData loc = locData[currentLoc];
        if(loc.up != "" && loc.up != null) arrows[0].SetActive(true);
        if(loc.right != "" && loc.right != null) arrows[1].SetActive(true);
        if(loc.down != "" && loc.down != null) arrows[2].SetActive(true);
        if(loc.left != "" && loc.left != null) arrows[3].SetActive(true);

        string NavigationTextDown = "";
        if(locData[currentLoc].navTextDown != "") {
            NavigationTextDown = locData[currentLoc].navTextDown;
            NavigationTextObjectDown.GetComponent<NavigationTextUD>().displayText = NavigationTextDown;
            NavigationTextObjectDown.SetActive(true);
        } else{
            NavigationTextObjectDown.SetActive(false);
        }

        string NavigationTextUp = "";
        if(locData[currentLoc].navTextUp != "") {
            NavigationTextUp = locData[currentLoc].navTextUp;
            NavigationTextObjectUp.GetComponent<NavigationTextUD>().displayText = NavigationTextUp;
            NavigationTextObjectUp.SetActive(true);
        } else{
            NavigationTextObjectUp.SetActive(false);
        }

        string NavigationTextLeft = "";
        if(locData[currentLoc].navTextLeft != "") {
            NavigationTextLeft = locData[currentLoc].navTextLeft;
            NavigationTextObjectLeft.GetComponent<NavigationTextLR>().displayText = NavigationTextLeft;
            NavigationTextObjectLeft.SetActive(true);
        } else{
            NavigationTextObjectLeft.SetActive(false);
        }

        string NavigationTextRight = "";
        if(locData[currentLoc].navTextRight != "") {
            NavigationTextRight = locData[currentLoc].navTextRight;
            NavigationTextObjectRight.GetComponent<NavigationTextLR>().displayText = NavigationTextRight;
            NavigationTextObjectRight.SetActive(true);
        } else{
            NavigationTextObjectRight.SetActive(false);
        }
    }
    IEnumerator PlayVideoAndJump(LocationData loc) {
        var videoPlayer = WorldSphere.GetComponent<UnityEngine.Video.VideoPlayer>();
        // reduce exposure then increase it again
        var exposure = sphereBox.GetFloat("_Exposure");
        while(exposure > 0.0f) {
            exposure -= 0.01f;
            sphereBox.SetFloat("_Exposure", exposure);
            yield return new WaitForSeconds(0.01f);
        }
        // set rotation of worldsphere to that of camera angle
        WorldSphere.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        videoPlayer.enabled = true;
        videoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Transitions/" + loc.transVid);
        videoPlayer.Play();
        while(exposure < 1.0f) {
            exposure += 0.01f;
            sphereBox.SetFloat("_Exposure", exposure);
            yield return new WaitForSeconds(0.01f);
        }
        while(videoPlayer.isPlaying) {

            yield return null;
        }
        sphereBox.SetFloat("_Exposure", 0.0f);
        videoPlayer.enabled = false;
        JumpToSite(loc.siteName, loc.initImage);
    }
    public bool JumpToLocation(string name) {
        currentLoc = name;
        if(locData.ContainsKey(currentLoc) == false){
            Debug.Log("No location grid data for " + currentLoc + "");
            return false;
        }
        if(locData[currentLoc].transition == true) {
            // first disable all arrows
            foreach(GameObject arrow in arrows) arrow.SetActive(false);
            // then after the video finishes, jump to the new site
            StartCoroutine(PlayVideoAndJump(locData[currentLoc]));
            return true;
        }

        // else
        // print all entris in locTextures
        foreach(KeyValuePair<String, Texture> entry in locTextures) {
            Debug.Log(entry.Key);
        }
        if(locTextures.ContainsKey(currentLoc) == false){
            Debug.Log("No texture for "+ currentLoc);
            return false;
        }
        // animate this thing
        StartCoroutine(ChangeTexture());

        return true;
    }

    ///////////////////////////////////////////////////////////////////////////
    // Jump to a site
    ///////////////////////////////////////////////////////////////////////////
    
    public void JumpToSite(string siteName, string initImage = null) {
        currentSite = siteName;
        // First, we load the grid data from locData.json
        LoadGrid();
        // Next we load the textures from 360images folder
        LoadTextures();

        // jump to the initial location within this site
        if(initImage == null || initImage == "") {
            SiteData site = siteData[currentSite];
            if( JumpToLocation(site.initImage) == false){
                Debug.Log("Error in jumping to initial location");
            }
        } else {
            if( JumpToLocation(initImage) == false){
                Debug.Log("Error in jumping to initial location");
            }
        }
        mapController.UpdateMap(currentSite);
    }

    ///////////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {

        foreach(GameObject arrow in arrows) arrow.SetActive(false);
        VideoIcon.SetActive(false);
        DetailIcon.SetActive(false);
        NavigationTextObjectDown.SetActive(false);
        NavigationTextObjectUp.SetActive(false);
        NavigationTextObjectLeft.SetActive(false);
        NavigationTextObjectRight.SetActive(false);

        siteData = new Dictionary<String, SiteData>();

        // load site data
        TextAsset mytxtData=(TextAsset)Resources.Load("metaLocData");
        string jsonString=mytxtData.text;
        Debug.Log(jsonString);
        SitesWrapper sites_ = JsonUtility.FromJson<SitesWrapper>(jsonString);

        // depending upon the name of teh scene, get JumpTo data
        string scene_name = SceneManager.GetActiveScene().name;
        if(scene_name == "Traveller"){
            mapController.SetButtonsToShow(sites_.JumpToTraveller);
        } else if( scene_name == "Devotee"){
            mapController.SetButtonsToShow(sites_.JumpToDevotee);
        } else if(scene_name == "Expert"){
            mapController.SetButtonsToShow(sites_.JumpToExpert);
        }

        SiteInfo[] arrayOfSites = sites_.Sites;

        foreach (SiteInfo site in arrayOfSites) {
            siteData.Add(site.name, site.data);
        }

        // disable the video player on teh Gameobject WorldSphere
        var videoPlayer = WorldSphere.GetComponent<UnityEngine.Video.VideoPlayer>();
        // diable it
        videoPlayer.enabled = false;
        sphereBox.SetFloat("_Exposure", 1.0f);

        JumpToSite("Bahgora_ghat_Dhunaguri");
    }

    // Update is called once per frame
    void Update()
    {
        // chnage position of worldsphere to camera position
        WorldSphere.transform.position = Camera.main.transform.position;
        //getting angle of view
        float angle = Camera.main.transform.eulerAngles.y;

        // move
        LocationData loc = locData[currentLoc];
        if (!lockInput && (Input.GetKeyDown(KeyCode.UpArrow) || leftActionValue.action.ReadValue<float>() > 0.9f || rightActionValue.action.ReadValue<float>() > 0.9f))
        {
            lockInput = true;
            if ((angle >= 25 && angle < 140) && (loc.up != null && loc.up != ""))
            {
                if( JumpToLocation(loc.up) == false){
                    Debug.Log("Jump to location failed");
                }
                Debug.Log("moved up ");
            }
            else if( angle >= 140 && angle < 225 && (loc.right != null && loc.right != ""))
            {
                if(JumpToLocation(loc.right) == false){
                    Debug.Log("Jump to location failed");
                }
                Debug.Log("moved right "); 
            }
            else if( angle >= 225 && angle < 325 && (loc.down != null && loc.down != ""))
            {
                if( JumpToLocation(loc.down) == false){
                    Debug.Log("Jump to location failed");
                }
                Debug.Log("moved down ");
            }
            else if( (angle < 25 || angle >= 325) && (loc.left != null && loc.left != ""))
            {
                if(JumpToLocation(loc.left) == false){
                    Debug.Log("Jump to location failed");
                };
                Debug.Log("moved left ");
            }

        } else if(lockInput && leftActionValue.action.ReadValue<float>() < 0.1f && rightActionValue.action.ReadValue<float>() < 0.1f) {
            lockInput = false;
        }

    }
}