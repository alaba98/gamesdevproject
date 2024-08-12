using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
   // [SerializeField] int bulletDamage = 20;
    [SerializeField] float destructionTime;
    [HideInInspector] public weaponManager weapon;
    float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= destructionTime)
            Destroy(this.gameObject);
        //destroy the bullet object after a set amount of time
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(weapon.damage);
            Destroy(gameObject);
        }

        Destroy(this.gameObject);//destroy on collision


    }
}

