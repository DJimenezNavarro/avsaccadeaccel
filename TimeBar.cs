using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    void Start()
    {

        slider.value = slider.maxValue - 0.01f; 
        
    }


    void Update()
    {

        if(slider.value == slider.maxValue){

            Debug.Log("Time limit reached");

        }
        
    }

    public void setMaxTime(float time)
    {

        slider.maxValue = time;
        
    }

    public void setTime(float time)
    {

        slider.value = time;

        fill.color = gradient.Evaluate(slider.normalizedValue);

    }
}
