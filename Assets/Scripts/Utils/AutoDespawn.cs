using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField] float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DespawnDelayed", lifetime);
    }

    void DespawnDelayed()
    {
        Destroy(gameObject);
    }
}
