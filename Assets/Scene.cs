using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public GameObject WorldSphere;
    public int currentX, currentY;
    public string currentSite;
    public GameObject[] arrows;
    public Material sphereBox;
    public GameObject Tablet;
    int maxX, maxY;
    Dictionary<String, SiteData> siteData;
    Dictionary<Vector2Int, LocationData> locData;
    Dictionary<Vector2Int, Texture> locTextures;

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
        public string tabImage;
    }
    [Serializable]
    public class SiteInfo {
        public string name;
        public SiteData data;
    }
    [Serializable]
    public class SitesWrapper {
        public SiteInfo[] Sites;
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
        public string tabImage;
        public TransitionData transition;
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
        TextAsset mytxtData=(TextAsset)Resources.Load(currentSite + "/locData");

        string jsonString=mytxtData.text;
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
    }
    IEnumerator PlayVideoAndJump(TransitionData transition) {
        var videoPlayer = WorldSphere.GetComponent<UnityEngine.Video.VideoPlayer>();
        while(videoPlayer.isPlaying) {
            yield return null;
        }
        videoPlayer.enabled = false;
        JumpToSite(transition.siteName, transition.initialLocation);
    }
    bool JumpToLocation(int x, int y) {
        currentX = x; currentY = y; 
        if(locData.ContainsKey(new Vector2Int(currentX, currentY)) == false){
            Debug.Log("No location grid data for " + currentX + " " + currentY + "");
            return false;
        }
        if(locData[new Vector2Int(currentX, currentY)].transition.siteName != null) {
            TransitionData transition = locData[new Vector2Int(currentX, currentY)].transition;
            // first disable all arrows
            foreach(GameObject arrow in arrows) arrow.SetActive(false);
            // grab the video object, start playing it
            var videoPlayer = WorldSphere.GetComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.enabled = true;
            videoPlayer.clip = Resources.Load<UnityEngine.Video.VideoClip>("Transitions/" + transition.transitionVideo);
            videoPlayer.Play();
            // then after the video finishes, jump to the new site
            StartCoroutine(PlayVideoAndJump(transition));
            return true;
        }

        // else
        if(locTextures.ContainsKey(new Vector2Int(currentX, currentY)) == false){
            Debug.Log("No texture for " + currentX + " " + currentY + "");
            return false;
        }
        sphereBox.SetTexture("_MainTex", locTextures[new Vector2Int(currentX, currentY)]);
        // set rotation of worldSphere based on z rotation of camera
        int rotation = locData[new Vector2Int(currentX, currentY)].rotation;
        // if(rotation == null) rotation = 0;
        WorldSphere.transform.rotation = Quaternion.Euler(0, rotation, 0);

        // set the arrows
        SetDirectionMarkers();
        // set the audio
        SetAndPlayAmbientAudio();

        // set the videoContent to be played on the tablet
        string videoContent = null;
        if(locData[new Vector2Int(currentX, currentY)].tabVideo != null) {
            videoContent = locData[new Vector2Int(currentX, currentY)].tabVideo;
        } else if(siteData[currentSite].tabVideo != null) {
            videoContent = siteData[currentSite].tabVideo;
        } 
        // call the tablet methods to provide the video and image content
        Tablet.GetComponent<Tab_Controller>().SetVideoContent(videoContent);

        return true;
    }

    ///////////////////////////////////////////////////////////////////////////
    // Jump to a site
    ///////////////////////////////////////////////////////////////////////////
    void JumpToSite(string siteName, Coordinates initialLocation = null) {
        currentSite = siteName;
        // First, we load the grid data from locData.json
        LoadGrid();
        // Next we load the textures from 360images folder
        LoadTextures();

        // jump to the initial location within this site
        if(initialLocation == null) {
            SiteData site = siteData[currentSite];
            if( JumpToLocation(site.initialLocation.x, site.initialLocation.y) == false){
                Debug.Log("Error in jumping to initial location");
            }
        } else {
            if( JumpToLocation(initialLocation.x, initialLocation.y) == false){
                Debug.Log("Error in jumping to initial location");
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        siteData = new Dictionary<String, SiteData>();
        // load site data
        TextAsset mytxtData=(TextAsset)Resources.Load("metaLocData");
        string jsonString=mytxtData.text;
        Debug.Log(jsonString);
        SitesWrapper sites_ = JsonUtility.FromJson<SitesWrapper>(jsonString);
        SiteInfo[] arrayOfSites = sites_.Sites;

        foreach (SiteInfo site in arrayOfSites) {
            siteData.Add(site.name, site.data);
        }

        // disable the video player on teh Gameobject WorldSphere
        var videoPlayer = WorldSphere.GetComponent<UnityEngine.Video.VideoPlayer>();
        // diable it
        videoPlayer.enabled = false;

        // JumpToSite("Auniati");
        // JumpToSite("Chawrekia");
        // JumpToSite("Baghor_Gaon");
        // JumpToSite("Madhya_Majuli_Satra");
        JumpToSite("Bali_Jokaibuwa_Gaon");
    }

    // Update is called once per frame
    void Update()
    {
        //getting angle of view
        float angle = Camera.main.transform.eulerAngles.y;

        // tablet
        if(Input.GetKeyDown(KeyCode.Space)) {
            // show the tablet if it is not shown
            if(Tablet.activeSelf == false)
            {
                Tablet.SetActive(true);
                Tablet.GetComponent<Tab_Controller>().PlayVideo();
            }
            else 
                Tablet.SetActive(false);
        }

        // move
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
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

        }

    }
}
