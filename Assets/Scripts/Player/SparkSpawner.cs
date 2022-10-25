using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SparkSpawner : MonoBehaviour
{
    [SerializeField] GameObject sparkPrefab;
    [SerializeField] float sparkSpeed;
    characterGround ground;
    bool wasInAir = false;

    void Awake()
    {
        ground = GetComponent<characterGround>();
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = ground.GetOnGround();
        if(grounded && wasInAir)
        {
            SpawnSparks();
        }
        wasInAir = !grounded;
    }

    void SpawnSparks()
    {
        List<PlatformMarker> markers = GameObject.FindObjectsOfType<PlatformMarker>().ToList();
        int closest_id = markers.OrderBy(pm => (pm.transform.position - transform.position).sqrMagnitude).First().Id;
        foreach(Transform t in markers.OrderBy(pm => pm.Id).Where(pm => Mathf.Abs(pm.Id - closest_id) <= 1).Select(pm => pm.transform))
        {
            Instantiate(sparkPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity =
                (t.position - transform.position).normalized * sparkSpeed;
        }
    }
}
