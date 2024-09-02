using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Varjo.XR;

public class LaserPointer : MonoBehaviour
{

    public bool active = true;
    public Color color;
    public Color clickColor = Color.green;
    public GameObject controller_object;
    private Controller controller;
    public bool addRigidBody = false;
    public GameObject pointer;
    public float thickness = 0.0002f;

    Ray raycast;
    RaycastHit hit;
    Ray ray;

    public string collision;
    public bool button;

    private void Start()
    {
    
        ray = new Ray(transform.position, transform.right);
        controller = controller_object.GetComponent<Controller>();

        pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, 1000);
        pointer.transform.localPosition = pointer.transform.localPosition + new Vector3(0, 0, -2.8f);
    }

    private void FixedUpdate()
    {

        raycast = new Ray(transform.position, transform.forward);
        bool bHit = Physics.Raycast(raycast, out hit);

        button = controller.triggerButton; 

        float dist = 100f;

        if (bHit && hit.distance < 100f)
        {
            dist = hit.distance;
            pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, 1000);
            pointer.GetComponent<MeshRenderer>().material.color = clickColor;
            
            collision = hit.collider.gameObject.name;
            
        }

        Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward, Color.green);
    }

}
