using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    private CinemachineBrain mainCamera;

    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineBrain>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Vector3 newCamPos = transform.position;
            newCamPos.z = (mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position.z;

            (mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).GetComponent<CameraController>().MoveToNewTabGrid(newCamPos);
        }
    }


    
}
