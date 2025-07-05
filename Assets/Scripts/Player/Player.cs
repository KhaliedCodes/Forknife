using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] private List<GameObject> Weapons;
    [SerializeField] private GameObject ActiveWeapon;
    private int currentWeaponIndex = 0;

    Ray ray;
    RaycastHit hit;
    [SerializeField] public int Health;
    public Slider healthBar;
    public Image hitFilter;

    [SerializeField] private GameObject MinimapCamera;

    private GameObject HighlightedWeapon;
    private PlayerData playerData;
    public WeaponManager weaponManager;
    [SerializeField] GameObject mainCamera;
    // Update is called once per frame
    void Start()
    {
        weaponManager = WeaponManager.Instance;
        playerData = new PlayerData();
        if (PlayerPrefs.GetInt("Continue", 0) == 1)
        {
            Debug.Log("Continuing");
            string json = PlayerPrefs.GetString(name);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Health = PlayerPrefs.GetInt("Health", 100);
            transform.SetPositionAndRotation(playerData.position, playerData.rotation);
            healthBar.value = Health;
            if (playerData.weapons.Count > 0)
            {
                foreach (var weapon in playerData.weapons)
                {
                    var weaponObject = weaponManager.GetWeapon(weapon);
                    if (weaponObject != null)
                        pickupWeapon(weaponObject);
                }
            }
        }
        StartCoroutine(savePlayer());
    }

    IEnumerator savePlayer()
    {
        while (true)
        {
            playerData.Health = Health;
            playerData.position = transform.position;
            playerData.rotation = transform.rotation;
            playerData.weapons = Weapons.Select(weapon => weapon.name).ToList();
            PlayerPrefs.SetInt("Health", Health);
            string json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString(name, json);
            PlayerPrefs.Save();
            yield return new WaitForSeconds(5);
        }
    }
    void Update()
    {
        if (Health > 0)
        {
            ray.origin = mainCamera.transform.position;
            ray.direction = mainCamera.transform.forward;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Weapon"))
                {
                    HighlightedWeapon = hit.collider.gameObject;
                    HighlightedWeapon.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    if (HighlightedWeapon != null)
                    {
                        HighlightedWeapon.GetComponent<Outline>().enabled = false;
                    }
                }
            }

            var alpha = Mathf.MoveTowards(hitFilter.color.a, 0, Time.deltaTime);

            if (alpha == 0)
            {
                hitFilter.gameObject.SetActive(false);
            }
            else
            {
                hitFilter.color = new Vector4(1, 0, 0, alpha);
            }
        }
    }

    void FixedUpdate()
    {

        MinimapCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);
    }

    void OnAttack()
    {
        if (Health > 0)
            ActiveWeapon.GetComponent<IWeapon>().Fire();
    }

    void OnInteract()
    {
        Debug.Log(hit.point);
        if (hit.transform.CompareTag("Weapon"))
        {

            pickupWeapon(hit.transform.gameObject);
        }
    }


    void pickupWeapon(GameObject weapon)
    {
        ActiveWeapon.SetActive(false);
        Weapons.Add(weapon);
        currentWeaponIndex++;
        ActiveWeapon = weapon;
        ActiveWeapon.transform.parent = mainCamera.transform;
        weapon.transform.SetLocalPositionAndRotation(weapon.GetComponent<Weapon>().weaponData.Position, weapon.GetComponent<Weapon>().weaponData.Rotation);

        ActiveWeapon.GetComponent<Rigidbody>().isKinematic = true;
        ActiveWeapon.GetComponent<BoxCollider>().enabled = false;
        ActiveWeapon.SetActive(true);
        ActiveWeapon.layer = 6;
    }


    void OnNext()
    {
        if (Health > 0)
        {
            ActiveWeapon.SetActive(false);
            if (currentWeaponIndex + 1 >= Weapons.Count) currentWeaponIndex = 0; else currentWeaponIndex++;

            ActiveWeapon = Weapons[currentWeaponIndex];
            ActiveWeapon.SetActive(true);

        }

    }


    void OnPrevious()
    {
        if (Health > 0)
        {
            ActiveWeapon.SetActive(false);
            if (currentWeaponIndex - 1 < 0) currentWeaponIndex = Weapons.Count - 1; else currentWeaponIndex--;

            ActiveWeapon = Weapons[currentWeaponIndex];
            ActiveWeapon.SetActive(true);
        }
    }
    public void LoseHealth(int value)
    {
        Health -= value;
        healthBar.value = Health;
        hitFilter.gameObject.SetActive(true);
        hitFilter.color = new Vector4(1, 0, 0, 0.5f);
        PlayerPrefs.SetInt("Health", Health);
        PlayerPrefs.Save();
        if (Health <= 0)
        {
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<PlayerInput>().enabled = false;
        }
    }

}
