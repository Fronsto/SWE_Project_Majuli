using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.SceneManagement;  
public class SceneChanger: MonoBehaviour {  
    public void Landing() {  
        SceneManager.LoadScene("Landing");  
    }  
    public void IntroVideo() {  
        SceneManager.LoadScene("IntroVideo");  
    }
}