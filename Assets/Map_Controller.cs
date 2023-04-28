using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Map_Controller : MonoBehaviour
{
    [SerializeField] Scene sceneScript;

    // Start is called before the first frame update
    void Start()
    {
        // initially we iterate through the children of children and set their text value to be the id of the children
        foreach(Transform child in this.transform){
            if(child.gameObject.name == "MapImage") continue;
            foreach(Transform grandChild in child){
                if(grandChild.gameObject.name == "Auto"){
                    grandChild.gameObject.GetComponent<TextMeshProUGUI>().text = child.gameObject.name;
                }
            }
        }
        
    }

    public void SetButtonsToShow(string[] sites){
        // access all child of this gameobject
        foreach(Transform child in this.transform){
            if(child.gameObject.name == "MapImage"){
                continue;
            }
            bool found = false;
            foreach(string site in sites){
                if(child.gameObject.name == site){
                    found = true;
                    break;
                }
            }
            if(found){
                child.gameObject.SetActive(true);
            }
            else{
                child.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMap(string siteName){
        // access all child of this gameobject

        foreach(Transform child in this.transform){
            if(child.gameObject.name == "MapImage"){
                continue;
            }
            if(child.gameObject.name == siteName){
                // change color of this gameobject to blue
                // change normal color of other gameobjects to white
                var col = child.gameObject.GetComponent<Button>().colors;
                col.normalColor = Color.blue;
                child.gameObject.GetComponent<Button>().colors = col;
            }
            else{
                var col = child.gameObject.GetComponent<Button>().colors;
                col.normalColor = Color.green;
                child.gameObject.GetComponent<Button>().colors = col;
            }
        }


    }

    public void Auniati()
    {
        sceneScript.JumpToSite("Auniati");
    }
    public void Baghor_Gaon()
    {
        sceneScript.JumpToSite("Baghor_Gaon");
    }
    public void Bahgora_ghat_Dhunaguri()
    {
        sceneScript.JumpToSite("Bahgora_ghat_Dhunaguri");
    }
    public void Bali_Jokaibuwa_Gaon()
    {
        sceneScript.JumpToSite("Bali_Jokaibuwa_Gaon");
    }
    public void Bhogpur()
    {
        sceneScript.JumpToSite("Bhogpur");
    }
    public void Chawrekia()
    {
        sceneScript.JumpToSite("Chawrekia");
    }
    public void Chotaipur_Gaon()
    {
        sceneScript.JumpToSite("Chotaipur_Gaon");
    }
    public void Gormur()
    {
        sceneScript.JumpToSite("Gormur");
    }
    public void JengraiMukh()
    {
        sceneScript.JumpToSite("JengraiMukh");
    }
    public void Kamalabari_Chariali()
    {
        sceneScript.JumpToSite("Kamalabari_Chariali");
    }
    public void Madhya_Majuli_Satra()
    {
        sceneScript.JumpToSite("Madhya_Majuli_Satra");
    }
    public void Namoni_Jokaibuwa()
    {
        sceneScript.JumpToSite("Namoni_Jokaibuwa");
    }
    public void Nayabazar()
    {
        sceneScript.JumpToSite("Nayabazar");
    }
    public void Rawanapar()
    {
        sceneScript.JumpToSite("Rawanapar");
    }
}
