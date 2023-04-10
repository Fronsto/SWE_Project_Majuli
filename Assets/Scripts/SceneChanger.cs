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
    public void Main() {
        SceneManager.LoadScene("Main");
    }
    public void ExitGame() {
        Application.Quit();
        Debug.Log("Exit App");
    }  
}