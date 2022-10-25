using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (p_collider.CompareTag("Player"))
        {
            GameObject.FindObjectOfType<GameManager>().ReloadScene();
        }
    }
}
