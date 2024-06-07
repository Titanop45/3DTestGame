using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [NonSerialized]public Vector3 position;
    public float sppeed = 30f;
    public int  damage = 20;
    private void Update()
    {
        float step = Time.deltaTime * sppeed;
        transform.position = Vector3.MoveTowards(transform.position, position, step);
        if (transform.position == position )
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Enemy"))
        {
            CarAttack attack = other.GetComponent<CarAttack>();
            attack.health -= damage;
            Transform healthBar =  other.transform.GetChild(0).transform;
            healthBar.localScale = new Vector3(
                healthBar.localScale.x - 0.3f,
                healthBar.localScale.y,
                healthBar.localScale.z);
            if(attack.health <= 0)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
