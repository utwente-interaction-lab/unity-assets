using System;
using System.Collections;
using UnityEngine;

public class Gun_Balls : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform barrel;
    private bool ready = true;
    private float heat;
    private float overheat = 5.0f;

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float lifeTime = 10.0f;
    [SerializeField] private float fireDelay = 0.1f;

    private void OnTriggerStay(Collider col)
    {
        if (heat < overheat)
        {
            if (!col.CompareTag("Gun") && ready)
            {
                StartCoroutine("AutoFire");
            }

            heat += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        heat = 0f;
    }

    IEnumerator AutoFire()
    {
        ready = false;
        Shoot();
        yield return new WaitForSeconds(fireDelay);
        ready = true;
    }
    
    private void Shoot()
    {
        GameObject b = Instantiate(ball, barrel.position, barrel.rotation);
        b.GetComponent<Rigidbody>().AddForce(barrel.forward * speed, ForceMode.Impulse);
        Destroy(b, lifeTime);
    }
}

