using System.Collections;
using UnityEngine;

public class Gun_Air : MonoBehaviour
{
    [SerializeField] private GameObject air;
    [SerializeField] private Transform barrel;
    [SerializeField] private Animator anim;

    private bool shot;
    private bool ready = true;
    [SerializeField] private float chargeSpeed = 10f;

    [SerializeField] private float speed = 1.0f;
    private float[] charges = new float[3];

    private GameObject[] airs = new GameObject[3];
    private Vector3[] forwards = new Vector3[3];

    private void Start()
    {
        for (int i = 0; i < charges.Length; i++)
        {
            charges[i] = 100f;
        }
    }

    private void Update()
    {
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] < 100f)
            {
                charges[i] += Time.deltaTime * chargeSpeed;
            }
        }

        anim.SetFloat("Charge1", charges[0] / 100f);
        anim.SetFloat("Charge2", charges[1] / 100f);
        anim.SetFloat("Charge3", charges[2] / 100f);

        AirControl();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Gun") & ready)
        {
            StartCoroutine("Delay");
        }
    }
    
    IEnumerator Delay()
    {
        ready = false;
        Shoot();
        yield return new WaitForSeconds(0.5f);
        ready = true;
    }

    private void Shoot()
    {
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] >= 100f)
            {
                charges[i] = 0f;
                GameObject airBubble = Instantiate(air, barrel.position, Quaternion.identity);
                airs[i] = airBubble;
                forwards[i] = new Vector3(barrel.forward.x, barrel.forward.y, barrel.forward.z);
                print(forwards[i]);
                Destroy(airBubble, 3.0f);
                break;
            }
        }
    }

    private void AirControl()
    {
        for (int i = 0; i < airs.Length; i++)
        {
            if (airs[i] != null)
            {
               airs[i].transform.Translate(forwards[i] * speed * Time.deltaTime); 
            }
        }       
    }
}

