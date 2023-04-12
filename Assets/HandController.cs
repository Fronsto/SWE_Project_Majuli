using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    public InputActionProperty activateAction;
    public InputActionProperty selectAction;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Trigger", activateAction.action.ReadValue<float>());
        animator.SetFloat("Grip", selectAction.action.ReadValue<float>());
    }
}
