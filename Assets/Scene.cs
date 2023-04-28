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
    public int currentX, currentY;
    public string currentSite;
    public GameObject[] arrows;
    public Material sphereBox;
    int maxX, maxY;
    Dictionary<String, SiteData> siteData;
    Dictionary<Vector2Int, LocationData> locData;
    Dictionary<Vector2Int, Texture> locTextures;
    bool lockInput = false;

    public GameObject VideoIcon;
    public GameObject DetailIcon;

    public GameObject NavigationTextObjectLeft;
    public GameObject NavigationTextObjectRight;
    public GameObject NavigationTextObjectUp;
    public GameObject NavigationTextObjectDown;

    [SerializeField] Tab_Controller tabController;
    [SerializeField] Map_Controller mapController;

    // Site data 
    [Serializable]
    public class Coordinates {
        public int x; 
        public int y;
        public int z;
    }
    [Serializable]
    public class SiteData {
        public Coordinates initialLocation;
        public string ambientAudio;
        public string tabVideo;
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

    // Transition data
    [Serializable]
    public class TransitionData {
        public string siteName;
        public string transitionVideo;
        public Coordinates initialLocation;
    }
    // Locations (within a site) data
    [Serializable]
    public class LocationData {
        public int rotation;
        public List<string> obstacles;
        public string ambientAudio;
        public string tabVideo;
        public TransitionData transition;
        public string navTextDown;
        public string navTextUp;
        public string navTextLeft;
        public string navTextRight;
    }
    [Serializable]
    public class LocationInfo {
        public int x;
        public int y;
        public LocationData data;
    }
    [Serializable]
    public class LocsWrapper {
        public LocationInfo[] Locs;
    }

    ///////////////////////////////////////////////////////////////////////////
    // Loads the grid data from locData.json
    void LoadGrid(){
        locData = new Dictionary<Vector2Int, LocationData>();
        // Read the file and use JSON utility to parse it , and create the locData map
        TextAsset mytxtData = (TextAsset)Resources.Load(currentSite + "/locData");

        string jsonString = mytxtData.text;
        LocsWrapper locs_ = JsonUtility.FromJson<LocsWrapper>(jsonString);
        LocationInfo[] arrayOfLocations = locs_.Locs;

        maxX = 0; maxY = 0; 
        foreach (LocationInfo loc in arrayOfLocations) {
            locData.Add(new Vector2Int(loc.x, loc.y), loc.data);
            maxX = Math.Max(maxX, loc.x);
            maxY = Math.Max(maxY, loc.y);
        }
        maxX++; maxY++;
    }
    ///////////////////////////////////////////////////////////////////////////
    // Loads the textures from 360images folder
    void LoadTextures() {
        locTextures = new Dictionary<Vector2Int, Texture>();
        Texture2D[] textures = Resources.LoadAll<Texture2D>(currentSite + "/360images");
        foreach(Texture2D tex in textures){
            String name = tex.name;
            // first 3 letters are img, so remove them
            name = name.Substring(4);
            int X = Int32.Parse(name.Split('_')[0]);
            int Y = Int32.Parse(name.Split('_')[1]);
            locTextures.Add(new Vector2Int(X,Y), tex);
            Debug.Log("Loaded texture " + X + " " + Y);
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
        sphereBox.SetTexture("_MainTex", locTextures[new Vector2Int(currentX, currentY)]);
        // set rotation of worldSphere based on z rotation of camera
        int rotation = locData[new Vector2Int(currentX, currentY)].rotation;
        WorldSphere.transform.rotation = Quaternion.Euler(0, rotation, 0);

        // set the arrows
        SetDirectionMarkers();
        // set the audio
        SetAndPlayAmbientAudio();
        // set tablet video content
        SetTabletVideo();

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
        // set the videoContent to be played on the tablet - need to add for text content to be shown on tablet too
        string videoContent = "";

        if(locData[new Vector2Int(currentX, currentY)].tabVideo != null) {
            videoContent = locData[new Vector2Int(currentX, currentY)].tabVideo;
            VideoIcon.SetActive(true);
        } else if(siteData[currentSite].tabVideo != null) {
            videoContent = siteData[currentSite].tabVideo;
            VideoIcon.SetActive(true);
        } else{
            VideoIcon.SetActive(false);
        }
        // call the tablet methods to provide the video and image content
        tabController.SetVideoContent(videoContent);
    }
    void SetAndPlayAmbientAudio() {
        string audioToPlay = null;
        var audioSource = WorldSphere.GetComponent<AudioSource>();
        if(locData[new Vector2Int(currentX, currentY)].ambientAudio != null) {
            audioToPlay = locData[new Vector2Int(currentX, currentY)].ambientAudio;
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
        List<string> obstacles = locData[new Vector2Int(currentX, currentY)].obstacles;
        foreach(GameObject arrow in arrows) arrow.SetActive(true);
        foreach (string item in obstacles)
        {
            if(item == "u") arrows[0].SetActive(false);
            if(item == "r") arrows[1].SetActive(false);
            if(item == "d") arrows[2].SetActive(false);
            if(item == "l") arrows[3].SetActive(false);
        }
        string NavigationTextDown = "";
        if(locData[new Vector2Int(currentX, currentY)].navTextDown != "") {
            NavigationTextDown = locData[new Vector2Int(currentX, currentY)].navTextDown;
            NavigationTextObjectDown.GetComponent<NavigationTextUD>().displayText = NavigationTextDown;
            NavigationTextObjectDown.SetActive(true);
        } else{
            NavigationTextObjectDown.SetActive(false);
        }

        string NavigationTextUp = "";
        if(locData[new Vector2Int(currentX, currentY)].navTextUp != "") {
            NavigationTextUp = locData[new Vector2Int(currentX, currentY)].navTextUp;
            NavigationTextObjectUp.GetComponent<NavigationTextUD>().displayText = NavigationTextUp;
            NavigationTextObjectUp.SetActive(true);
        } else{
            NavigationTextObjectUp.SetActive(false);
        }

        string NavigationTextLeft = "";
        if(locData[new Vector2Int(currentX, currentY)].navTextLeft != "") {
            NavigationTextLeft = locData[new Vector2Int(currentX, currentY)].navTextLeft;
            NavigationTextObjectLeft.GetComponent<NavigationTextLR>().displayText = NavigationTextLeft;
            NavigationTextObjectLeft.SetActive(true);
        } else{
            NavigationTextObjectLeft.SetActive(false);
        }

        string NavigationTextRight = "";
        if(locData[new Vector2Int(currentX, currentY)].navTextRight != "") {
            NavigationTextRight = locData[new Vector2Int(currentX, currentY)].navTextRight;
            NavigationTextObjectRight.GetComponent<NavigationTextLR>().displayText = NavigationTextRight;
            NavigationTextObjectRight.SetActive(true);
        } else{
            NavigationTextObjectRight.SetActive(false);
        }
    }
    IEnumerator PlayVideoAndJump(TransitionData transition) {
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
        videoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Transitions/" + transition.transitionVideo);
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
        JumpToSite(transition.siteName, transition.initialLocation);
    }
    public bool JumpToLocation(int x, int y) {
        currentX = x; currentY = y; 
        if(locData.ContainsKey(new Vector2Int(currentX, currentY)) == false){
            Debug.Log("No location grid data for " + currentX + " " + currentY + "");
            return false;
        }
        if(locData[new Vector2Int(currentX, currentY)].transition.siteName != null) {
            TransitionData transition = locData[new Vector2Int(currentX, currentY)].transition;
            // first disable all arrows
            foreach(GameObject arrow in arrows) arrow.SetActive(false);
            // then after the video finishes, jump to the new site
            StartCoroutine(PlayVideoAndJump(transition));
            return true;
        }

        // else
        if(locTextures.ContainsKey(new Vector2Int(currentX, currentY)) == false){
            Debug.Log("No texture for " + currentX + " " + currentY + "");
            return false;
        }
        // animate this thing
        StartCoroutine(ChangeTexture());

        return true;
    }

    ///////////////////////////////////////////////////////////////////////////
    // Jump to a site
    ///////////////////////////////////////////////////////////////////////////
    
    public void JumpToSite(string siteName, Coordinates initialLocation = null) {
        currentSite = siteName;
        // First, we load the grid data from locData.json
        LoadGrid();
        // Next we load the textures from 360images folder
        LoadTextures();

        // jump to the initial location within this site
        if(initialLocation == null || (initialLocation.x == 0 && initialLocation.y == 0)) {
            SiteData site = siteData[currentSite];
            if( JumpToLocation(site.initialLocation.x, site.initialLocation.y) == false){
                Debug.Log("Error in jumping to initial location");
            }
        } else {
            if( JumpToLocation(initialLocation.x, initialLocation.y) == false){
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

        JumpToSite("Auniati");
    }

    // Update is called once per frame
    void Update()
    {
        //getting angle of view
        float angle = Camera.main.transform.eulerAngles.y;

        // move
        if (!lockInput && (Input.GetKeyDown(KeyCode.UpArrow) || leftActionValue.action.ReadValue<float>() > 0.9f || rightActionValue.action.ReadValue<float>() > 0.9f))
        {
            lockInput = true;
            List<string> obs = locData[new Vector2Int(currentX, currentY)].obstacles;
            int[] direcs = new int[4];
            direcs[0] = 0; direcs[1] = 0; direcs[2] = 0; direcs[3] = 0;

            foreach (string item in obs)
            {
                if(item == "u") direcs[0] = 1;
                if(item == "d") direcs[1] = 1;
                if(item == "l") direcs[2] = 1;
                if(item == "r") direcs[3] = 1;
            }

            if ((angle >= 25 && angle < 140) && direcs[0] == 0)
            {
                int initX = currentX;
                currentX--;
                while((locData.ContainsKey(new Vector2Int(currentX, currentY)) == false) && currentX > 0)
                    currentX--;
                if(currentX != 0){
                    if( JumpToLocation(currentX, currentY) == false){
                        Debug.Log("Jump to location failed");
                        currentX = initX;
                    }
                    Debug.Log("moved up "); Debug.Log(currentX); Debug.Log(currentY);
                }
            }
            else if( angle >= 140 && angle < 225 && direcs[3] == 0)
            {
                int initY = currentY;
                currentY++;
                // this while is not working as expected. Its always false
                while((locData.ContainsKey(new Vector2Int(currentX, currentY)) == false) && currentY < maxY)
                    currentY++;
                if(currentY != maxY){
                    if(JumpToLocation(currentX, currentY) == false){
                        Debug.Log("Jump to location failed");
                        currentY = initY;
                    }
                    Debug.Log("moved right "); Debug.Log(currentX); Debug.Log(currentY);
                }
            }
            else if( angle >= 225 && angle < 325 && direcs[1] == 0)
            {
                int initX = currentX;
                currentX++;
                while((locData.ContainsKey(new Vector2Int(currentX, currentY)) == false) && currentX < maxX)
                    currentX++;
                if(currentX != maxX){
                    if( JumpToLocation(currentX, currentY) == false){
                        Debug.Log("Jump to location failed");
                        currentX = initX;
                    }
                    Debug.Log("moved down "); Debug.Log(currentX); Debug.Log(currentY);
                }
            }
            else if( (angle < 25 || angle >= 325) && direcs[2] == 0)
            {
                int initY = currentY;
                currentY--;
                while((locData.ContainsKey(new Vector2Int(currentX, currentY)) == false) && currentY > 0)
                    currentY--;
                if(currentY != 0){
                    if(JumpToLocation(currentX, currentY) == false){
                        Debug.Log("Jump to location failed");
                        currentY = initY;
                    };
                    Debug.Log("moved left "); Debug.Log(currentX); Debug.Log(currentY);
                }
            }

        } else if(lockInput && leftActionValue.action.ReadValue<float>() < 0.1f && rightActionValue.action.ReadValue<float>() < 0.1f) {
            lockInput = false;
        }

    }
}