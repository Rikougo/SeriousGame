using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffTheLight : MonoBehaviour
{

    public GameObject levelLight;
    LevelLight l;
    // Start is called before the first frame update
    void Start()
    {
        l = levelLight.GetComponent<LevelLight>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            l.enabled = true;
            print("ok");
        }
    }
}
