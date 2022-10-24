using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickup : MonoBehaviour
{
    private LevelLight _levelLight;

    public float lightDuration = 2.0f;

    private void Awake()
    {
        _levelLight = GameObject.FindWithTag("LevelLight").GetComponent<LevelLight>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _levelLight.TurnOnFor(lightDuration);
            Destroy(this.gameObject);
        }
    }
}
