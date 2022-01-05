using UnityEngine;

public class Gun_Gravity : MonoBehaviour
{
    private bool shootable;

    [SerializeField] private GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Rigidbody hitTarget;

    [SerializeField] ParticleSystem ps2;
    [SerializeField] ParticleSystem ps3;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform barrel;

    [SerializeField] float strength = 1.5f;

    [SerializeField] float chargeTime = 1.0f;
    [SerializeField] float cooldown = 1.5f;

    private float charge;
    private float cool;

    private bool charging;
    private bool charged;
    private bool cooling;
    private bool clicking;

    private void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Gun"))
        {
            clicking = true;
        }
    }

    private void OnTriggerExit()
    {
        clicking = false;
    }

    private void Update()
    {
        if (cooling == false)
            {
                if (clicking)
                {
                    Shoot();
                    charging = true;
                    cool = 0f;
                }
                else
                {
                    laser.SetActive(false);
                }

                if (charged && !clicking)
                {
                    if (shootable)
                    {
                        Fly(hitTarget);
                    }
                    else
                    {
                        anim.Play("GravityDischarge");
                        charge = 0f;
                        cooling = true;
                    }
                    charged = false;
                }
            }
        
        Timers();
    }

    private void Timers()
    {
        if (charge >= chargeTime)
        {
            charging = false;
            charged = true;
        }

        if (charging == true)
        {
            charge += Time.deltaTime;
        }

        if (cool >= cooldown)
        {
            cooling = false;
        }

        if (cooling == true)
        {
            cool += Time.deltaTime;
        }
    }

    private void Shoot()
    {
        anim.Play("GravityCharge");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            ShowLaser(hit);

            if (hit.collider.gameObject.GetComponent<Rigidbody>())
            {
                hitTarget = hit.collider.gameObject.GetComponent<Rigidbody>();
                shootable = true;
            }
            else
            {
                shootable = false;
            }
        }
        else
        {
            laser.SetActive(false);
        }
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(barrel.position, hit.point, 0.5f);
        laserTransform.LookAt(hit.point);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }

    private void Fly(Rigidbody target)
    {
        target.useGravity = false;
        Vector3 force = new Vector3(Random.Range(-strength, strength), Random.Range(0, strength / 2f), Random.Range(-strength, strength));
        float multiplier = target.mass;
        target.AddForce(force * multiplier);
        target.AddTorque(force / 2 * multiplier);

        laser.SetActive(false);
        shootable = false;

        anim.Play("GravityShoot");
        ps2.Play();
        ps3.transform.position = target.gameObject.transform.position;
        ps3.Play();

        charge = 0f;
        cooling = true;
    }
}
