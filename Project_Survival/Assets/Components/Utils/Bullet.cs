using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //goes forward and stop at the first of anything.
    //anything that can get damagable or its a wall will get 
    float damage;
    GameObject parentReference;
     Vector3 dir;
    public void SetUpBullet(float damage,Vector3 dir,  GameObject parentReference)
    {
        this.damage = damage;
        this.parentReference = parentReference;
        this.dir = dir;
    }

    private void Update()
    {
        transform.position += dir * 15 * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == parentReference)
        {
            Debug.Log("its the same parent");
            return;
        }

        if(collision.gameObject.layer == 6)
        {
            Debug.Log("touched wall or floor");
            Destroy(gameObject);
            return;
        }

        Debug.Log("its got here");
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;

        Destroy(gameObject);
        damageable.TakeDamage(damage, transform, 1);
    }

}
