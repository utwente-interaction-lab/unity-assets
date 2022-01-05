using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private List<Light> lights = new List<Light>();
    
    [SerializeField] private float cooldown = 1f;
    private bool ready = true;
    
    private bool period;
    private float timer;
    private float seconds = 1f;

    private void OnTriggerEnter(Collider col)
    {
        if (ready && period)
        {
            StartCoroutine("Cool");
        }
    }

    private void Update()
    {
        if (!period)
        {
            timer += Time.deltaTime;
        }

        if (timer > seconds)
        {
            period = true;
        }
    }

    IEnumerator Cool()
    {
        ready = false;
        foreach (Light lamp in lights)
        {
            lamp.enabled = !lamp.enabled;
            yield return new WaitForSeconds(cooldown);
        }
        ready = true;
    }
}
