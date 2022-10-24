using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    private Vector3 initPos;
    private float cumulator;
    
    public bool Running { get; private set; }
    public float deltaY = 1.0f;
    public bool RunOnAwake = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        initPos = transform.position;

        Running = RunOnAwake;
        cumulator = 0.0f;
    }
    
    void FixedUpdate()
    {
        if (Running)
        {
            cumulator += Time.deltaTime;
            Vector3 l_pos = initPos;
            l_pos.y += Mathf.Sin(cumulator) * deltaY;

            transform.position = l_pos;
        }
    }
}
