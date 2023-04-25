using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletInfoIcons : MonoBehaviour
{
    private float thetaStep = Mathf.PI / 32f;
    [SerializeField]
    public float WaitBetweenWobbles = 0.1f;
    [SerializeField]
    private float theta = 0f;
    [SerializeField]
    public float Intensity = 500f;


   Quaternion _targetAngle;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeTarget", 0f, WaitBetweenWobbles);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetAngle, Time.deltaTime);
    }

    void ChangeTarget()
    {
        _targetAngle = Quaternion.Euler(Vector3.forward * Mathf.Sin(theta) * Intensity);
        if(theta >= Mathf.PI / 15f || theta <= -Mathf.PI / 15f){
            thetaStep = -thetaStep;
        }
        theta += thetaStep;
    }
}
