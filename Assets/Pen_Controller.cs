using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Pen_Controller : MonoBehaviour
{
    public InputActionProperty activateAction; // Trigger
    public GameObject pen;
    public GameObject rightHand;

    // Start is called before the first frame update
    void Start()
    {
        pen.SetActive(false);
        rightHand.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(activateAction.action.ReadValue<float>() > 0.8f || Input.GetKeyDown(KeyCode.P)) {
            pen.SetActive(true);
            rightHand.SetActive(false);
        }
        else{
            pen.SetActive(false);
            rightHand.SetActive(true);
        }
    }
}
