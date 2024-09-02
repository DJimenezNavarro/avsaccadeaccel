using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

public class ExampleCases : MonoBehaviour
{
    public GameCases game_cases;
    public FixationControl fixation_control;

    private GameObject fix_chicken; 

    private GameObject r10obj; 
    private GameObject l10obj; 
    private GameObject r12obj;
    private GameObject l12obj; 
    private GameObject r14obj; 
    private GameObject l14obj; 
    
    private GameObject beep_r10; 
    private GameObject beep_l10; 
    private GameObject beep_r12; 
    private GameObject beep_l12; 
    private GameObject beep_r14; 
    private GameObject beep_l14; 


    public int[] cases;
    public int i = 0;
    public int next = 0;
    private bool current;
    private int newcase; 


    public float soa;
    public float fixation_delay;


    public float target_onset;
    public float network_delay;


    private float onset_target;
    private string onset_frame;


    private string path;

    public float time; 
    public float time2;
    public float startTime;
    public float endTime;
    public float target_on;
    public float target_off;
    public float fixation_off;
    public bool audio_flag;
    public float audio_on;
    public float audio_off; 

    private bool start_new;
    private bool end_sound;
    private bool audio_playing;
    private bool target_done;


    public GameObject beep;
    public AudioSource sound;
    public GameObject target;


    public GameObject controller_object;
    public bool button;
    public bool startbutton;
    private Controller controller;
    private LaserPointer pointer;
    public string object_found;

    public float new_case_timer = 2.0f;


    void Start()
    {
        Debug.Log("Started Examples!");
        controller = controller_object.GetComponent<Controller>();
        pointer = controller_object.GetComponent<LaserPointer>();



        fix_chicken = GameObject.Find("Chicken"); 
        fix_chicken.SetActive(true); 
        r10obj = GameObject.Find("R1"); 
        r10obj.SetActive(false); 
        l10obj = GameObject.Find("L1"); 
        l10obj.SetActive(false); 
        r12obj = GameObject.Find("R2"); 
        r12obj.SetActive(false); 
        l12obj = GameObject.Find("L2"); 
        l12obj.SetActive(false);  
        r14obj = GameObject.Find("R3"); 
        r14obj.SetActive(false); 
        l14obj = GameObject.Find("L3"); 
        l14obj.SetActive(false);


        beep_r10 = GameObject.Find("R1_sound"); 
        sound = beep_r10.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l10 = GameObject.Find("L1_sound"); 
        sound = beep_l10.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r12 = GameObject.Find("R2_sound"); 
        sound = beep_r12.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l12 = GameObject.Find("L2_sound"); 
        sound = beep_l12.GetComponent<AudioSource>();
        sound.mute = true;

        beep_r14 = GameObject.Find("R3_sound"); 
        sound = beep_r14.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l14 = GameObject.Find("L3_sound"); 
        sound = beep_l14.GetComponent<AudioSource>();
        sound.mute = true; 


        try{

            game_cases = this.GetComponent<GameCases>();
            fixation_control = this.GetComponent<FixationControl>();

        }
        catch{
            print("No Game Cases");
        } 
    }

    void FixedUpdate()
    {
        
        time2 = time2 + Time.deltaTime; 
        
        Debug.Log("TimeFix: " + time2);

        if (current == false){ 

            startTime = 0.0f;
            endTime = 0.0f;
            fixation_off = 0.0f;
            target_on = 0.0f;
            target_off = 0.0f;
            audio_off = 0.0f;
            audio_on = 0.0f;
            audio_flag = false;

        }else{ 

            trial(fixation_delay, audio_flag, soa, startTime, time2, next, target_onset); 

        }


    }


    void Update()
    {

        time = time + Time.deltaTime; 
        Debug.Log("TimeUpdate: " + time);
        button = controller.triggerButton; 
        object_found = pointer.collision; 

        startbutton = controller.primary2DAxisClick; 

        try{

            if(time > new_case_timer  && fix_chicken.activeSelf == true){ 

                new_case_timer = 9999; 
                next = cases[i];

                newTrial(next, time2);


                current = true;
                i = i + 1;


            }else{


            }

            if(startbutton){

                if(game_cases != null){

                    fix_chicken.SetActive(true); 
                    r10obj.SetActive(true); 
                    l10obj.SetActive(true);  
                    r12obj.SetActive(true); 
                    l12obj.SetActive(true);
                    r14obj.SetActive(true); 
                    l14obj.SetActive(true);

                    game_cases.enabled = true;
                    fixation_control.enabled = true; 


                }else{

                    Debug.Log("Error");
                }

                this.enabled = false; 
            }

        } catch {

            if(game_cases != null){

                game_cases.enabled = true;
                fixation_control.enabled = true; 

            }else{

                Debug.Log("No more cases, ending...");
                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
            }

            this.enabled = false; 

        }
        
    }


    void newTrial(int next, float start){


        start_new = true;
        startTime = start;


        endTime = startTime + 20.0f; 

        switch(next) { 

            // Example Cases //

            case 1: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r10obj;
                beep = beep_r10;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.0f; 


                break;

            case 2: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l10obj;
                beep = beep_l10;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;
                target_onset = 2.02f; 


                break;

            case 3: 

                soa = -0.145f;
                fixation_delay = -0.073f;
                target = r12obj;
                beep = beep_r12;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.04f; 


                break;

            case 4: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l12obj;
                beep = beep_l12;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;
                target_onset = 2.04f; 


                break;

            case 5: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r14obj;
                beep = beep_r14;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.0f; 


                break;

            case 6: 

                soa = -0.135f;
                fixation_delay = -0.068f;
                target = l14obj;
                beep = beep_l14;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.06f; 


                break;

            default:
                
                Debug.Log("Unrecognized case...");

                break;

        }


    } 


    void trial(float fixation_delay, bool audio_flag, float soa, float startTime, float currentTime, int next, float targetspawn){ 

        if (start_new) 
        {
            start_new = false;

            end_sound = false;
            audio_playing = false;
            target_done = false;
            
            target_on = currentTime + targetspawn;

            fixation_off = target_on;
            fixation_off = fixation_off + fixation_delay; 


            target_off = target_on + 20.0f; 

            if(audio_flag){

                audio_on = target_on + soa; 
                audio_off = audio_on + 0.150f; 

            }else{

            }


        }else{ 

            if (currentTime >= fixation_off && fix_chicken.activeSelf == true){ 
                
                fix_chicken.SetActive(false); 
                
            }

 
            if (currentTime >= target_on && target.activeSelf == false  && target_done == false){ 
                
                target.SetActive(true); 

                onset_target = currentTime;
                onset_frame = Time.frameCount.ToString();
                                    
            }


            if ((object_found == "CL1" ||  object_found == "CL2" ||  object_found == "CL3" || object_found == "CR1" || object_found == "CR2" || object_found == "CR3") && button && target.activeSelf == true  && target_done == false){    
                
                target.SetActive(false);  
                target_done = true;

                endTime = currentTime;

            }

            if(audio_flag){

                // Beep starts
                if (currentTime >= audio_on && end_sound == false && audio_playing == false){ 

                    audio_playing = true;

                    sound.mute = false;

                    audio_off = currentTime + 0.150f;

                }

                if (currentTime >= audio_off && end_sound == false){ 

                    sound.mute = true;

                    
                    audio_playing = false;
                    end_sound = true;

                }

            }


            if (currentTime >= endTime){ 
                
                current = false;
                
                new_case_timer = currentTime + 2.0f;

                fix_chicken.SetActive(true); 

            }

        }

    }


    void OnApplicationQuit(){
        
        Debug.Log("Application ending after " + Time.time + " seconds");
    }



    public void shuffle(int[] cases){


        for(int i = 0; i < cases.Length; i++){

            int randomindex = UnityEngine.Random.Range(0, cases.Length);
            
            int actual  = cases[i];

            cases[i] = cases[randomindex];
            cases[randomindex] = actual;
        }
    }
}
