using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().LoseHealth(10);
        }
    }
}
