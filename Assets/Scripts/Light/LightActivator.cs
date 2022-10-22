using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightActivator : MonoBehaviour
{
    [SerializeField] float duration = 3;
    public void Use()
    {
        LevelLight.Instance.TurnOnFor(duration);
    }
}
