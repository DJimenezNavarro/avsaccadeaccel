using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixationControl : MonoBehaviour
{


    public TimeBar timeBar;
    public EyeTracking eyeData;

    public float max_time = 2.0f;
    public float x_lim = 0.2f;
    public float y_lim = 0.2f;

    public Vector3 gaze;

    private GameObject fix_chicken; 

    public float timer;

    void Awake(){


        fix_chicken = GameObject.Find("Chicken"); 
        timeBar.setMaxTime(max_time);

    }
    
    
    void Start()
    {

        timeBar.setTime(0.0f);
        
    }


    void Update()
    {

        gaze = eyeData.gazeforward; 

        if(fix_chicken.activeSelf == true){ 

            timer = timer + Time.deltaTime; 

            if((gaze.x > 0.0f && gaze.x < x_lim) ||( gaze.x < 0.0f && gaze.x > -x_lim)){ 


                if((gaze.y > 0.0f && gaze.y < y_lim) ||( gaze.y < 0.0f && gaze.y > -y_lim)){ 

                    timer = 0.0f; 
                }else{

                }

                
            }else{

            }


        }else{
            
            if(timer != 0.0f){

                timer = 0.0f; 
                
            }
            

        }

        timeBar.setTime(timer); 

        
    }

}
