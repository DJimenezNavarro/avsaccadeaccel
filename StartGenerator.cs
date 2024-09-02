using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
public class StartGenerator : MonoBehaviour
{

    public MisGenerator mis_generator;
    public StimuliGenerator stimuli_generator;
    public MisStaircase mis_staircase;
    public StaircaseCom staircase_com;
    public MisExamples mis_examples;

    public LatencyGenerator1 latency_generator1;
    public LatencyGenerator2 latency_generator2;

    
    // Start is called before the first frame update
    void Start()
    {
        try{
            mis_staircase = this.GetComponent<MisStaircase>();
        }
        catch{
            print("No MIS");
        } 

        try{
            stimuli_generator = this.GetComponent<StimuliGenerator>();
        }
        catch{
            print("No TEST");
        } 

        try{
            staircase_com = this.GetComponent<StaircaseCom>();
        }
        catch{
            print("No COM");
        } 

        try{
            mis_examples = this.GetComponent<MisExamples>();
        }
        catch{
            print("No MIS EXAMPLES");
        } 

        try{
            latency_generator1 = this.GetComponent<LatencyGenerator1>();
        }
        catch{
            print("No LATENCY");
        } 

        try{
            latency_generator2 = this.GetComponent<LatencyGenerator2>();
        }
        catch{
            print("No LATENCY2");
        } 

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Delete)){ 

            // start examples if any
            if(mis_examples != null){

                mis_examples.enabled = true;

            }else{

                Debug.Log("Error");
            }

            this.enabled = false; 

        }

        if(Input.GetKeyDown(KeyCode.Return)){

            // start main experiment and logs
            if(mis_staircase != null){

                mis_staircase.enabled = true;

            }else if(stimuli_generator != null){

                stimuli_generator.enabled = true;

            }else if(staircase_com != null){

                staircase_com.enabled = true;

            }else if(latency_generator2 != null){

                latency_generator2.enabled = true;

            }else{

                Debug.Log("Error");
            }

            this.enabled = false; 

        }
        
    }

    public bool getPinchDown()
    {
        return false;
        
    }
}