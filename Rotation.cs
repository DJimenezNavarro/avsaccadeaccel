using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //Quaternion q = new Quaternion(0.016f, 0.134f , -0.003f, 0.991f);
        //Debug.Log(q.Euler.ToString("F3"));
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(transform.rotation.eulerAngles.ToString("F3"));
        Debug.Log(transform.localRotation.eulerAngles.ToString("F3"));
        //Debug.Log(transform.rotation.eulerAngles.ToString("F3"));
        //Debug.Log(transform.rotation.eulerAngles.ToString("F3"));
        
    }
}
