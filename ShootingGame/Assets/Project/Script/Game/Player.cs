using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public Camera playerCamera;

    [Header("Gameplays")]
    public int initialHealth = 100;
    public int initialAmmo = 12;
    public float knockbackForce = 10;
    public float hurtDuration = 0.5f;

    public int health;
    public int Health { get { return health; } }

    private int ammo;
    public int Ammo { get { return ammo; } }

    private bool killed;
    public bool Killed { get { return killed; } }

    private bool isHurt;
    // Start is called before the first frame update
    void Start()
    {
        health = initialHealth;
        ammo = initialAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0)
            {
                ammo--;
                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true);
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
            }
        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.collider.name);
        if (hit.collider.GetComponent<AmmoCrate>() != null)
        {
            AmmoCrate ammoCrate = hit.collider.GetComponent<AmmoCrate>();
            ammo += ammoCrate.ammo;
            Destroy(ammoCrate.gameObject);
        }else if(hit.collider.GetComponent<HealthCrate>() != null)
        {
            HealthCrate healthCrate = hit.collider.GetComponent<HealthCrate>();
            health += healthCrate.health;
            Destroy(healthCrate.gameObject);
        }


        if (isHurt == false)
        {
            GameObject hazard = null;
            if (hit.collider.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                hazard = enemy.gameObject;
                health -= enemy.damage;
            }
            else if (hit.collider.GetComponent<Bullet>() != null)
            {
                Bullet bullet = hit.collider.GetComponent<Bullet>();
                if (bullet.ShotByPlayer == false)
                {
                    hazard = bullet.gameObject;
                    health -= bullet.damage;
                    bullet.gameObject.SetActive(false);
                }
            }
            if (hazard != null)
            {
     
                //Knockback effect
                isHurt = true;
                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
                GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);
                StartCoroutine(HurtRoutine());
            }

            if(health <= 0)
            {
                if(killed == false)
                {
                    killed = true;

                    OnKill();
                }
            }
        }
    }

        IEnumerator HurtRoutine()
        {
            yield return new WaitForSeconds(hurtDuration);

            isHurt = false;
        }

        private void OnKill()
    {
       
    }
    }
