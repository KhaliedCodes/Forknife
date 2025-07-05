
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public GameObject bulletHolePrefab;


    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.gameObject.tag);
        // if (collision.collider.gameObject.CompareTag("Player"))
        // {
        //     collision.gameObject.GetComponent<Player>().LoseHealth(Damage);
        // }
        // else
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {

            collision.gameObject.GetComponent<Enemy>().LoseHealth(Damage);
        }
        else
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, collision.GetContact(0).point + collision.GetContact(0).normal * 0.01f, Quaternion.LookRotation(collision.GetContact(0).normal));

            Destroy(bulletHole, 5f);
        }


        Destroy(gameObject);
    }
}
