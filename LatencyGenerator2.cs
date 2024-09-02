using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;


public class LatencyGenerator2 : MonoBehaviour
{

    private GameObject fix_cross; 

    private GameObject r5obj; 
    private GameObject l5obj; 
    private GameObject r12obj; 
    private GameObject l12obj; 
    private GameObject r20obj; 
    private GameObject l20obj; 
    
    private GameObject beep_r5; 
    private GameObject beep_l5; 
    private GameObject beep_r12; 
    private GameObject beep_l12; 
    private GameObject beep_r20; 
    private GameObject beep_l20; 

    // Cases control
    public string participant;
    public int session;
    public int[] cases;
    public int i = 0;
    public int next = 0;
    private bool current;
    private int newcase; 

    //Soa and Delay
    public float soa;
    public float fixation_delay;

    //Target on
    public float target_onset;

    //Results
    private float onset_target;
    private string onset_frame;

    //Logs
    private string path;

    private float time; 
    private float time2;
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

    //Beep & Target
    public GameObject beep;
    public AudioSource sound;
    public GameObject target;


    // Start is called before the first frame update
    void Start()
    {

        shuffle(cases);


        fix_cross = GameObject.Find("Fixation_cross"); 
        fix_cross.SetActive(true); 
        r5obj = GameObject.Find("R5"); 
        r5obj.SetActive(false); 
        l5obj = GameObject.Find("L5"); 
        l5obj.SetActive(false); 
        r12obj = GameObject.Find("R12"); 
        r12obj.SetActive(false); 
        l12obj = GameObject.Find("L12"); 
        l12obj.SetActive(false); 
        r20obj = GameObject.Find("R20"); 
        r20obj.SetActive(false); 
        l20obj = GameObject.Find("L20"); 
        l20obj.SetActive(false); 

        //All sounds off
        beep_r5 = GameObject.Find("Beep_R5"); 
        sound = beep_r5.GetComponent<AudioSource>();
        sound.mute = true;

        beep_l5 = GameObject.Find("Beep_L5"); 
        sound = beep_l5.GetComponent<AudioSource>();
        sound.mute = true;

        beep_r12 = GameObject.Find("Beep_R12"); 
        sound = beep_r12.GetComponent<AudioSource>();
        sound.mute = true;

        beep_l12 = GameObject.Find("Beep_L12"); 
        sound = beep_l12.GetComponent<AudioSource>();
        sound.mute = true;

        beep_r20 = GameObject.Find("Beep_R20"); 
        sound = beep_r20.GetComponent<AudioSource>();
        sound.mute = true;

        beep_l20 = GameObject.Find("Beep_L20"); 
        sound = beep_l20.GetComponent<AudioSource>();
        sound.mute = true;

        string folder_dir;

        folder_dir = "Assets/Output/FinalLatencies2GT/" + participant + "/";

        Directory.CreateDirectory(folder_dir);

        path = folder_dir + participant + "_" + session;
          
    }

    void FixedUpdate()
    {
    
        time2 = time2 + Time.deltaTime; 

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

    // Update is called once per frame
    void Update()
    {

        time = time + Time.deltaTime;

        try{

            if(Input.GetKeyDown("space") && fix_cross.activeSelf == true){ 

                next = cases[i];

                //Start next trial
                newTrial(next, time2);


                current = true;
                i = i + 1;

            }else{

            }

        } catch {

            Debug.Log("No more cases, ending...");
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;

        }
        
    }

    void newTrial(int next, float start){


        start_new = true;

        startTime = start;
        endTime = startTime + 7.0f; 

        switch(next) { 

            // Final Cases //

            case 1: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r5obj;
                beep = beep_r5;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.0f; 


                break;

            case 2: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l5obj;
                beep = beep_l5;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;
                target_onset = 2.00f; 


                break;

            case 3: 

                soa = -0.075f;
                fixation_delay = -0.038f;
                target = r5obj;
                beep = beep_r5;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.02f; 


                break;

            case 4: 

                soa = -0.075f;
                fixation_delay = -0.038f;
                target = l5obj;
                beep = beep_l5;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.02f; 


                break;

            case 5:

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r12obj;
                beep = beep_r12;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.0f; 


                break;

            case 6: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l12obj;
                beep = beep_l12;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;
                target_onset = 2.00f; 


                break;

            case 7: 

                soa = -0.145f;
                fixation_delay = -0.073f;
                target = r12obj;
                beep = beep_r12;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.04f; 


                break;

            case 8: 

                soa = -0.145f;
                fixation_delay = -0.073f;
                target = l12obj;
                beep = beep_l12;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.04f; 


                break;

            case 9: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r20obj;
                beep = beep_r20;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 2.0f; 


                break;

            case 10: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l20obj;
                beep = beep_l20;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;
                target_onset = 2.00f;


                break;

            case 11: 

                soa = -0.135f;
                fixation_delay = -0.068f;
                target = r20obj;
                beep = beep_r20;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 3.06f; 


                break;

            case 12: 

                soa = -0.135f;
                fixation_delay = -0.068f;
                target = l20obj;
                beep = beep_l20;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                target_onset = 3.06f; 


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

            target_off = target_on + 2.0f; 

            if(audio_flag){

                audio_on = target_on + soa; 
                audio_off = target_off + 0.150f; 

                Debug.Log("Case " + next + " : Set Target ON at " + target_on + ", OFF at " + target_off + ", FIX OFF at " + fixation_off + ", AUD ON at " + audio_on + ", AUD OFF at " + audio_off);

            }else{

            }


        }else{ 

            if (currentTime >= fixation_off && fix_cross.activeSelf == true){ 
                
                fix_cross.SetActive(false); 
                
            }

            if (currentTime >= target_on && target.activeSelf == false  && target_done == false){ 
                
                target.SetActive(true); 
                Debug.Log("Target ON at " + currentTime); 
                onset_target = currentTime;
                onset_frame = Time.frameCount.ToString();
                                    
            }

            if (currentTime >= target_off && target.activeSelf == true  && target_done == false){ 
                
                target.SetActive(false);  
                target_done = true;
                Debug.Log("Target OFF at " + currentTime); 

            }

            if(audio_flag){

                if (currentTime >= audio_on && end_sound == false && audio_playing == false){

                    audio_playing = true;
                    sound.mute = false;

                    audio_off = currentTime + 0.150f;

                    Debug.Log("Audio ON at " + currentTime);

                }


                if (currentTime >= audio_off && end_sound == false){ 

                    sound.mute = true; 

                    audio_playing = false;
                    end_sound = true;

                    Debug.Log("Audio OFF at " + currentTime);

                }

            }

            if (currentTime >= endTime){ 
                
                current = false;

                fix_cross.SetActive(true); 


                StreamWriter writer = new StreamWriter(path, true);

                if(target.name == "R5"){

                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  2);

                }else if(target.name == "L5"){

                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  1);

                }else if(target.name == "R12"){

                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  4);

                }else if(target.name == "L12"){

                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  3);

                }else if(target.name == "L20"){

                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  6);

                }else{
                    
                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  5);
                }

                writer.Close();

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
