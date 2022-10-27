using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] ParticleSystem shieldBurst;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Function that activates the shield burst
    public void StartShieldBurst()
    {
        shieldBurst.Play();
    }
}
