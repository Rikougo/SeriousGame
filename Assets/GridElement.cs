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
            (mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position = newCamPos;
            // StopCoroutine ( MoveScreen() );
            // StartCoroutine ( MoveScreen() );
        }
    }


    IEnumerator MoveScreen() {

        Vector3 nextPos = transform.position;

        nextPos.z =  (mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position.z;
        (mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position = nextPos;

        yield return null;

        // float smoothTime = 0.7F;
        // Vector3 velocity = Vector3.zero;

        // while((mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position  != nextPos) {
        //     nextPos = transform.position;
        //     nextPos.z =  (mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position.z;
        //     (mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position = Vector3.SmoothDamp((mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position, nextPos, ref velocity, smoothTime);
            
        //     yield return null;
        // }
    }
    
}
