using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public GameObject player;
    private int Health = 100;

    protected Animator Animator;
    protected StateContext _context;
    public float crosshairCooldown = 1f;
    private float lastCrosshairTime = -1f;
    public Slider healthBar;
    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite normalCrosshair;
    [SerializeField] private Sprite hitCrosshair;
    private bool wasHit = false;

    [SerializeField] private GameManager gameManager;
    protected bool isDead = false;
    private EnemyData enemyData;
    BehaviorGraphAgent behaviorGraphAgent;
    protected void Start()
    {

        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        _context = new StateContext();
        Animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        Health = gameManager.GetEnemyData().Health;
        healthBar.maxValue = Health;
        enemyData = new EnemyData();
        if (PlayerPrefs.GetInt("Continue", 0) == 1)
        {
            string json = PlayerPrefs.GetString(name);
            enemyData = JsonUtility.FromJson<EnemyData>(json);
            if (enemyData.isDead)
            {
                Destroy(gameObject);
            }
            Debug.Log(json);
            transform.SetPositionAndRotation(enemyData.position, enemyData.rotation);
            Health = enemyData.Health;
            healthBar.value = Health;
            // Debug.Log("Load enemy");
        }
        StartCoroutine(saveEnemyData());

    }


    private IEnumerator saveEnemyData()
    {
        while (true)
        {
            save();
            yield return new WaitForSeconds(5);
        }

    }

    void save()
    {
        enemyData.Health = Health;
        enemyData.position = transform.position;
        enemyData.rotation = transform.rotation;
        enemyData.isDead = isDead;
        string json = JsonUtility.ToJson(enemyData);
        PlayerPrefs.SetString(name, json);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (behaviorGraphAgent != null)
            behaviorGraphAgent.SetVariableValue<float>("Health", Health);
        if (Health > 0)
        {

            if (player.GetComponent<Player>().Health > 0)
            {

                // transform.LookAt(player.transform.position + Vector3.up);
                // if (Time.time >= lastShootTime + shootCooldown)
                // {
                //     Fire();
                // }
            }



        }
        if (Time.time >= lastCrosshairTime + crosshairCooldown && wasHit)
        {

            crosshair.sprite = normalCrosshair;
            wasHit = false;
        }
    }


    public void LoseHealth(int value)
    {
        if (isDead)
        {
            return;
        }
        if (Health > 0)
        {
            crosshair.sprite = hitCrosshair;
            lastCrosshairTime = Time.time;
            wasHit = true;
        }
        Health -= value;
        healthBar.value = Health;

        if (Health <= 0)
        {
            _context.SetState(new DeathState(Animator));
            Debug.Log("Datalt dead");
            GetComponent<Rigidbody>().freezeRotation = false;
            Destroy(gameObject, 10f);
            healthBar.gameObject.SetActive(false);
            gameManager.IncreaseScore();
            isDead = true;
            save();
        }
    }
}
