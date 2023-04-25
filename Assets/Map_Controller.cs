using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Controller : MonoBehaviour
{
    [SerializeField] Scene sceneScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Auniati()
    {
        sceneScript.JumpToSite("Auniati");
    }
    public void Bhogpur()
    {
        sceneScript.JumpToSite("Bhogpur");
    }
    public void Madhya_Majuli_Satra()
    {
        sceneScript.JumpToSite("Madhya_Majuli_Satra");
    }
    public void Baghor_Gaon()
    {
        sceneScript.JumpToSite("Baghor_Gaon");
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
    public void Kamalabari_Chariali()
    {
        sceneScript.JumpToSite("Kamalabari_Chariali");
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
