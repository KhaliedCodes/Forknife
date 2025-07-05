using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{

    public WeaponData weaponData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject bulletPrefab;
    public GameObject FireParticlePrefab;
    [SerializeField] public float bulletSpeed = 100f;
    public float shootCooldown = 1f; // Time in seconds between shots
    private float lastShootTime = -1f; // Time when the last shot was fired

    private Vector3 firePoint;
    public Transform MuzzlePosition;
    Ray ray;
    RaycastHit hit;
    public GameObject WeaponHolder;

    public int Damage = 50;

    [SerializeField] private AudioManager audioManager;


    void Start()
    {
        audioManager = AudioManager.Instance;
    }

    void Update()
    {
        ray.origin = WeaponHolder.transform.position;
        ray.direction = WeaponHolder.transform.forward;
        if (Physics.Raycast(ray, out hit, 100f, ~0, QueryTriggerInteraction.Ignore))
        {
            firePoint = hit.point;
        }
        else
        {
            firePoint = ray.GetPoint(100);
        }
    }

    public void Fire()
    {
        if (Time.time >= lastShootTime + shootCooldown)
        {
            audioManager.PlayAudioClip("SFX", "Gunshot");
            GameObject bullet = Instantiate(bulletPrefab, MuzzlePosition.position, MuzzlePosition.rotation);
            bullet.GetComponent<Bullet>().Damage = Damage;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            Vector3 direction = (firePoint - MuzzlePosition.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
            // FireParticle.SetActive(true);
            Instantiate(FireParticlePrefab, MuzzlePosition.position, MuzzlePosition.rotation);
            // fireParticle.transform.parent = transform;
            lastShootTime = Time.time;


        }
    }

}
