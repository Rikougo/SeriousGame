using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelLight : MonoBehaviour
{
    [SerializeField] AnimationCurve lightStart;
    [SerializeField] float lightStartTime = 0.25f;
    [SerializeField] AnimationCurve lightStop;
    [SerializeField] float lightStopTime = 1;
    new Light2D light;
    float timer = -1;
    float paramsScale = 1;
    float fullLightTime = 0;

    static LevelLight _instance;
    public static LevelLight Instance => _instance;

    void Start()
    {
        light = GetComponent<Light2D>();
        light.enabled = false;
        _instance = this;
    }

    public void TurnOnFor(float seconds)
    {
        float minTime = lightStartTime + lightStopTime;
        float remaining = fullLightTime + paramsScale * minTime - timer;
        if(timer < 0 || seconds > remaining)
        {
            bool wasLit = timer > paramsScale * lightStartTime && timer < paramsScale * lightStartTime + fullLightTime;
            if(seconds < minTime) paramsScale = seconds / minTime;
            else paramsScale = 1;
            fullLightTime = seconds - paramsScale * minTime;
            timer = 0;
            if(wasLit)
            {
                float t = paramsScale * lightStartTime;
                timer += t;
                fullLightTime += t;
            }
        }
    }

    void Update()
    {
        if(timer >= 0)
        {
            light.enabled = true;
            float start_time = paramsScale * lightStartTime;
            timer += Time.deltaTime;
            if(timer < start_time)
            {
                light.intensity = lightStart.Evaluate(timer / start_time);
            }
            else if(timer >= fullLightTime + start_time)
            {
                float end_time = paramsScale * lightStopTime;
                float local_timer = timer - fullLightTime - start_time;
                if(local_timer < end_time)
                {
                    light.intensity = lightStop.Evaluate(local_timer / end_time);
                }
                else
                {
                    light.enabled = false;
                    timer = -1;
                }
            }
        }
    }
}