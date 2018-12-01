using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Tower : MonoBehaviour {

    [Header("Rhythm Game Manager Ref")]
    public RhythmGameManager rManager;

	[Header("Tower Variables")]
    public TowerStats tStats;
    public bool canAttack = true;
    public int enemyCount = 0;
    public int towerLane;

    [Header("Tower Health Variables")]
    public float towerHealth;
    public float towerMaxHealth;

    [Header("Control Variables")]
    [Tooltip("To be set to true by tower AI script if OnDeathEvent")] public bool overRideDeathCall = false;
    public bool canMove = true;
    [HideInInspector] public float attackTimer = 0f;

    [Header("GUI References")]
    public GameObject healthBarCanvas;
    public Image healthBar;

    [Header("Sound References")]
    public AudioClip spawnClip;

    private AudioSource aSource;

    private void Awake()
    {
        rManager = GameObject.Find("GameManager").GetComponent<GameManager>().rgManager;
        healthBarCanvas = transform.GetChild(0).gameObject;
        healthBar = healthBarCanvas.transform.GetChild(1).GetComponent<Image>();

        attackTimer = 0f;
        enemyCount = 0;

        towerHealth = towerMaxHealth = tStats.towerHealth;
    }

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            // Updating the GUI
            UpdateGUI();

            // Moving the Tower
            TowerInteractions();

            // Checking for Death
            if (!overRideDeathCall)
            {
                if (towerHealth <= 0f)
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        towerHealth -= damage;

        // Playing the Damage Sound
        if (tStats.damageSound != null && GetComponent<AudioSource>() != null)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().PlaySoundFX(tStats.damageSound);
            /*
            GetComponent<AudioSource>().clip = tStats.damageSound;
            GetComponent<AudioSource>().Play();
            */
        }
    }

    private void TowerInteractions()
    {
        // Check for Attacking and Moving
        if (CanAttack())
        {
            TowerAttack();

            if (tStats.canAttackWhileMoving)
                MoveTower();
            else
                return;
        } else {
            MoveTower();
        }
    }

    private void MoveTower()
    {
        if (tStats.stopsEnemies)
        {
            Debug.DrawRay(transform.position, Vector3.right * tStats.moveVector.x, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(tStats.moveVector.x), 0.5f, tStats.enemyLayer);
            if (!hit)
            {
                if (canMove)
                    transform.position += (tStats.moveVector * Time.deltaTime);
            } else
            {
                if (hit.collider.gameObject.GetComponent<Enemy>().enemyLane != towerLane)
                    transform.position += (tStats.moveVector * Time.deltaTime);
            }
        } else {
            if (canMove)
                transform.position += (tStats.moveVector * Time.deltaTime);
        }
    }

    private void TowerAttack()
    {
        // Starting the Animation
        if (tStats.towerAStyle != TowerStats.TowerAttackStyle.NoAttack && tStats.towerAStyle != TowerStats.TowerAttackStyle.Collision)
            if (GetComponent<Animator>() != null)
                GetComponent<Animator>().SetBool("isAttacking", true);

        return;
    }

    private void UpdateGUI()
    {
        // Updating the Healthbar
        healthBar.fillAmount = (towerHealth / towerMaxHealth);
    }

    private bool CanAttack()
    {
        if (attackTimer <= 0f && canAttack)
        {
            float range = tStats.towerRange;
            if (tStats.towerAStyle == TowerStats.TowerAttackStyle.Melee)
                range = 1f;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, tStats.shootAngle, range, tStats.enemyLayer);
            return (hit && hit.collider.GetComponent<Enemy>().enemyLane == towerLane);
        } else
        {
            attackTimer -= Time.deltaTime;
            return false;
        }
    }

    public void DamageEnemy()
    {
        GetComponent<Animator>().SetBool("isAttacking", false);

        if (tStats.towerAStyle == TowerStats.TowerAttackStyle.Range)
        {
            GameObject newAttack = Instantiate(tStats.rangeAttackPrefab);
            newAttack.transform.position = transform.position;

            newAttack.GetComponent<PlayerBullet>().attackMult = rManager.rgMult;
            newAttack.GetComponent<PlayerBullet>().bulletLane = towerLane;
            newAttack.GetComponent<PlayerBullet>().tStats = tStats;

            // Play the Attack Sound
            if (tStats.attackSound != null)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().PlaySoundFX(tStats.attackSound);
                /*
                GetComponent<AudioSource>().clip = tStats.attackSound;
                GetComponent<AudioSource>().Play();
                */
            }

            return;
        }

        if (tStats.towerAStyle == TowerStats.TowerAttackStyle.Melee)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, tStats.towerRange, tStats.enemyLayer);
            if (hit)
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null && enemy.enemyLane == towerLane)
                {
                    enemy.TakeDamage(tStats.towerDamage * rManager.rgMult);
                    enemy.transform.position = new Vector3(enemy.transform.position.x + tStats.towerKnockBackStrength, enemy.transform.position.y, enemy.transform.position.z);
                    if (tStats.towerKnockBackStrength > 0f)
                    {
                        enemy.canMove = true;
                        enemy.canAttack = true;
                    }

                    // Play the Attack Sound
                    if (tStats.attackSound != null)
                    {
                        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySoundFX(tStats.attackSound);
                        /*
                        GetComponent<AudioSource>().clip = tStats.attackSound;
                        GetComponent<AudioSource>().Play();
                        */
                    }
                }

                return;
            }
        }

        attackTimer = tStats.towerAttackSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Debug.DrawRay(transform.position, tStats.shootAngle * tStats.towerRange);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exiting a Trigger");
        // This Death Call Ignores Any Death Call Overrides
        GameObject obj = collision.gameObject;
        if (obj.tag == "StartLine")
        {
            GameObject.Destroy(gameObject);
        }

    }
}
