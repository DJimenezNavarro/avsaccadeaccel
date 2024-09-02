using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

public class LatencyGenerator1 : MonoBehaviour
{

    private GameObject fix_cross; 
    private GameObject r1obj; 
    private GameObject l1obj; 
    private GameObject r2obj; 
    private GameObject l2obj; 
    private GameObject beep_r1; 
    private GameObject beep_r1p; 
    private GameObject beep_r1pp; 
    private GameObject beep_r1m; 
    private GameObject beep_r1mm; 
    private GameObject beep_l1; 
    private GameObject beep_l1p; 
    private GameObject beep_l1pp; 
    private GameObject beep_l1m; 
    private GameObject beep_l1mm; 
    private GameObject beep_r2; 
    private GameObject beep_r2p; 
    private GameObject beep_r2pp; 
    private GameObject beep_r2m; 
    private GameObject beep_r2mm; 
    private GameObject beep_l2; 
    private GameObject beep_l2p; 
    private GameObject beep_l2pp; 
    private GameObject beep_l2m; 
    private GameObject beep_l2mm; 

    // Cases control
    public string participant;
    public int session;
    public int[] cases;
    public int i = 0;
    public int next = 0;
    private bool current;
    private int newcase; 

    //SOA and delay
    public float soa;
    public float fixation_delay;

    private float onset_target;
    private string onset_frame;

    //Log
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

        //Shuffle cases if needed
        shuffle(cases);


        fix_cross = GameObject.Find("Fixation_cross"); 
        fix_cross.SetActive(true); 
        r1obj = GameObject.Find("R1"); 
        r1obj.SetActive(false); 
        l1obj = GameObject.Find("L1"); 
        l1obj.SetActive(false);  
        r2obj = GameObject.Find("R2"); 
        r2obj.SetActive(false); 
        l2obj = GameObject.Find("L2"); 
        l2obj.SetActive(false);  


        beep_r1 = GameObject.Find("Beep_R1"); 
        sound = beep_r1.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r1p = GameObject.Find("Beep_R1+"); 
        sound = beep_r1p.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r1pp = GameObject.Find("Beep_R1++"); 
        sound = beep_r1pp.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r1m = GameObject.Find("Beep_R1-"); 
        sound = beep_r1m.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r1mm = GameObject.Find("Beep_R1--"); 
        sound = beep_r1mm.GetComponent<AudioSource>();
        sound.mute = true;

        beep_l1 = GameObject.Find("Beep_L1"); 
        sound = beep_l1.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l1p = GameObject.Find("Beep_L1+"); 
        sound = beep_l1p.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l1pp = GameObject.Find("Beep_L1++"); 
        sound = beep_l1pp.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l1m = GameObject.Find("Beep_L1-"); 
        sound = beep_l1m.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l1mm = GameObject.Find("Beep_L1--"); 
        sound = beep_l1mm.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r2 = GameObject.Find("Beep_R2"); 
        sound = beep_r2.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r2p = GameObject.Find("Beep_R2+"); 
        sound = beep_r2p.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r2pp = GameObject.Find("Beep_R2++"); 
        sound = beep_r2pp.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r2m = GameObject.Find("Beep_R2-"); 
        sound = beep_r2m.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_r2mm = GameObject.Find("Beep_R2--"); 
        sound = beep_r2mm.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l2 = GameObject.Find("Beep_L2"); 
        sound = beep_l2.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l2p = GameObject.Find("Beep_L2+"); 
        sound = beep_l2p.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l2pp = GameObject.Find("Beep_L2++"); 
        sound = beep_l1pp.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l2m = GameObject.Find("Beep_L2-"); 
        sound = beep_l2m.GetComponent<AudioSource>();
        sound.mute = true; 

        beep_l2mm = GameObject.Find("Beep_L2--"); 
        sound = beep_l2mm.GetComponent<AudioSource>();
        sound.mute = true; 

        string folder_dir; 

        folder_dir = "Assets/Output/FinalLatenciesGT/" + participant + "/";

        Directory.CreateDirectory(folder_dir);

        path = folder_dir + participant + "_" + session; 
        
    }


    // FixedUpdate is called once per time provided 
    void FixedUpdate()
    {
        
        time2 = time2 + Time.deltaTime; 

        if (current == false){ 

            //Reset timers
            startTime = 0.0f;
            endTime = 0.0f;
            fixation_off = 0.0f;
            target_on = 0.0f;
            target_off = 0.0f;
            audio_off = 0.0f;
            audio_on = 0.0f;
            audio_flag = false;

        }else{ 

            trial(fixation_delay, audio_flag, soa, startTime, time2, next); 

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

                //Debug.Log("Waiting.");
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
        endTime = startTime + 4.0f; 

        switch(next) { 

            // FINAL CASES //

            case 1: 

                soa = 0.0f;
                fixation_delay = 0.0f;
                target = r1obj;
                audio_flag = false;


                break;

            case 2: 

                soa = 0.0f;
                fixation_delay = 0.0f;
                target = r2obj;
                audio_flag = false;


                break;

            case 3: 

                soa = 0.0f;
                fixation_delay = 0.0f;
                target = l1obj;
                audio_flag = false;


                break;

            case 4: 

                soa = 0.0f;
                fixation_delay = 0.0f;
                target = l2obj;
                audio_flag = false;


                break;
                
            case 5: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r1obj;
                beep = beep_r1;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();


                break;

            case 6: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r1obj;
                beep = beep_r1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 7: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r1obj;
                beep = beep_r1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 8: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r1obj;
                beep = beep_r1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 9: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r1obj;
                beep = beep_r1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 10: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r2obj;
                beep = beep_r2;
                audio_flag = true;
                sound = beep.GetComponent<AudioSource>();
                


                break;

            case 11: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r2obj;
                beep = beep_r2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 12: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r2obj;
                beep = beep_r2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 13: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r2obj;
                beep = beep_r2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 14: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = r2obj;
                beep = beep_r2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 15: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l1obj;
                beep = beep_l1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 16: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l1obj;
                beep = beep_l1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 17: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l1obj;
                beep = beep_l1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 18: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l1obj;
                beep = beep_l1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 19: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l1obj;
                beep = beep_l1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 20: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l2obj;
                beep = beep_l2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 21: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l2obj;
                beep = beep_l2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 22: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l2obj;
                beep = beep_l2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 23: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l2obj;
                beep = beep_l2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 24: 

                soa = 0.00f;
                fixation_delay = 0.0f;
                target = l2obj;
                beep = beep_l2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;
            
            case 25: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r1obj;
                beep = beep_r1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 26: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r1obj;
                beep = beep_r1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;
            
            case 27:

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r1obj;
                beep = beep_r1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 28: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r1obj;
                beep = beep_r1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 29: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r1obj;
                beep = beep_r1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 30: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r2obj;
                beep = beep_r2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 31: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r2obj;
                beep = beep_r2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;
            
            case 32: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r2obj;
                beep = beep_r2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 33:

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r2obj;
                beep = beep_r2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 34: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = r2obj;
                beep = beep_r2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 35: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l1obj;
                beep = beep_l1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 36: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l1obj;
                beep = beep_l1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 37: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l1obj;
                beep = beep_l1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 38: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l1obj;
                beep = beep_l1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 39: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l1obj;
                beep = beep_l1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 40: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l2obj;
                beep = beep_l2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 41: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l2obj;
                beep = beep_l2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 42: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l2obj;
                beep = beep_l2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 43: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l2obj;
                beep = beep_l2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 44: 

                soa = -0.100f;
                fixation_delay = -0.050f;
                target = l2obj;
                beep = beep_l2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 45: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r1obj;
                beep = beep_r1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 46:

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r1obj;
                beep = beep_r1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 47: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r1obj;
                beep = beep_r1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 48: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r1obj;
                beep = beep_r1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 49: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r1obj;
                beep = beep_r1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 50: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r2obj;
                beep = beep_r2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 51: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r2obj;
                beep = beep_r2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 52: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r2obj;
                beep = beep_r2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 53: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r2obj;
                beep = beep_r2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 54: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = r2obj;
                beep = beep_r2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 55: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l1obj;
                beep = beep_l1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 56: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l1obj;
                beep = beep_l1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 57: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l1obj;
                beep = beep_l1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 58: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l1obj;
                beep = beep_l1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 59: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l1obj;
                beep = beep_l1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 60: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l2obj;
                beep = beep_l2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 61: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l2obj;
                beep = beep_l2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 62: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l2obj;
                beep = beep_l2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 63: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l2obj;
                beep = beep_l2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 64: 

                soa = -0.200f;
                fixation_delay = -0.100f;
                target = l2obj;
                beep = beep_l2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 65: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r1obj;
                beep = beep_r1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 66: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r1obj;
                beep = beep_r1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 67:

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r1obj;
                beep = beep_r1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 68: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r1obj;
                beep = beep_r1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 69: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r1obj;
                beep = beep_r1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 70: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r2obj;
                beep = beep_r2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 71: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r2obj;
                beep = beep_r2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 72: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r2obj;
                beep = beep_r2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 73: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r2obj;
                beep = beep_r2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 74: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = r2obj;
                beep = beep_r2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 75: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l1obj;
                beep = beep_l1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 76: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l1obj;
                beep = beep_l1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 77:

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l1obj;
                beep = beep_l1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 78: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l1obj;
                beep = beep_l1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 79: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l1obj;
                beep = beep_l1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 80: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l2obj;
                beep = beep_l2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 81: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l2obj;
                beep = beep_l2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 82: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l2obj;
                beep = beep_l2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 83: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l2obj;
                beep = beep_l2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 84: 

                soa = -0.300f;
                fixation_delay = -0.150f;
                target = l2obj;
                beep = beep_l2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 85: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r1obj;
                beep = beep_r1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 86: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r1obj;
                beep = beep_r1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 87: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r1obj;
                beep = beep_r1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 88: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r1obj;
                beep = beep_r1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 89: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r1obj;
                beep = beep_r1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 90: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r2obj;
                beep = beep_r2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 91: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r2obj;
                beep = beep_r2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 92: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r2obj;
                beep = beep_r2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 93: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r2obj;
                beep = beep_r2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 94: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = r2obj;
                beep = beep_r2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 95: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l1obj;
                beep = beep_l1;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 96: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l1obj;
                beep = beep_l1p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 97: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l1obj;
                beep = beep_l1pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 98: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l1obj;
                beep = beep_l1m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 99: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l1obj;
                beep = beep_l1mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 100: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l2obj;
                beep = beep_l2;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 101: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l1obj;
                beep = beep_l2p;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 102: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l2obj;
                beep = beep_l2pp;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 103: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l2obj;
                beep = beep_l2m;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            case 104: 

                soa = -0.400f;
                fixation_delay = -0.200f;
                target = l2obj;
                beep = beep_l2mm;
                sound = beep.GetComponent<AudioSource>();
                audio_flag = true;


                break;

            default:
                
                Debug.Log("Unrecognized case...");

                break;

        }


    }

    void trial(float fixation_delay, bool audio_flag, float soa, float startTime, float currentTime, int next){ 

        if (start_new) 
        {
            start_new = false;

            end_sound = false;
            audio_playing = false;
            target_done = false;


            fixation_off = currentTime + UnityEngine.Random.Range(0.750f, 1.750f);


            target_on = fixation_off;
            fixation_off = fixation_off + fixation_delay; 

            target_off = target_on + 2.0f; 

            if(audio_flag){

                audio_on = target_on + soa; //SOA
                audio_off = target_off + 0.150f; 

            }else{

            }


        }else{ 

            if (currentTime >= fixation_off && fix_cross.activeSelf == true){ 
                
                fix_cross.SetActive(false); 
                
            }

            // Target appears   
            if (currentTime >= target_on && target.activeSelf == false  && target_done == false){ 
                
                target.SetActive(true); 

                onset_target = currentTime;
                onset_frame = Time.frameCount.ToString();
                                    
            }

            // Target disappears 
            if (currentTime >= target_off && target.activeSelf == true  && target_done == false){ 
                
                target.SetActive(false);  
                target_done = true;

            }

            if(audio_flag){

                // Beep starts
                if (currentTime >= audio_on && end_sound == false && audio_playing == false){

                    audio_playing = true;
                    sound.mute = false;

                    audio_off = currentTime + 0.150f;

                }

                // Beep ends
                if (currentTime >= audio_off && end_sound == false){ 

                    sound.mute = true;

                    audio_playing = false;
                    end_sound = true;

                }

            }

            // Write log
            if (currentTime >= endTime){ 
                
                current = false;

                fix_cross.SetActive(true);

                StreamWriter writer = new StreamWriter(path, true);

                if(target.name == "R1"){

                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  2);

                }else if(target.name == "L1"){

                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  1);

                }else if(target.name == "R2"){

                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  4);

                }else{
                    
                    writer.WriteLine(onset_target + " | " + onset_frame + " | " + next + " | " +  3);
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