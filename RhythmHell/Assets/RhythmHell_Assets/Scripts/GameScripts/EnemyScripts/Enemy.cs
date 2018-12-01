using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour {

    [Header("Enemy Variables")]
    public EnemyStats eStats; // Reference to EnemyStats for enemy
    public bool canAttack = true;
    public bool canMove = true;
    public int enemyLane;

    [Header("Enemy Health Variables")]
    public float enemyMaxHealth; // Max Health (Readonly)
    public float enemyHealth; // Tracks current Health

    private float attackTimer; // Tracks Attack Cooldown

    [Header("GUI References")]
    public GameObject healthBarCanvas;
    public Image healthBar;

    [Header("Override Variables")]
    public bool moveOverride = false;
    public bool attackOverride = false;
    public bool deathOverride = false;

    [Header("Stun Variables")]
    public bool stunned = false;
    public float stunTimer = 0f;

    public Color stunnedColor;
    public Color baseColor;

    [HideInInspector] public AudioSource aSource;

    // Called at Instatiation, Initializes Tracking Variables
    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = baseColor;
        aSource = GetComponent<AudioSource>();

        healthBarCanvas = transform.GetChild(0).gameObject;
        healthBar = healthBarCanvas.transform.GetChild(1).GetComponent<Image>();

        attackTimer = 0f;

        enemyMaxHealth = eStats.maxHealth;
        enemyHealth = enemyMaxHealth;
    }

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            // Updating The GUI
            UpdateGUI();

            // Check for enemy Death
            if (!deathOverride)
                CheckForDeath();

            bool tryAttack = false;
            if (eStats.enemyAttackStyle != EnemyStats.EnemyAttackStyle.NoAttack)
            {
                TryEnemyAttack();

                // Execute Enemy Attack
                tryAttack = TowerInRange();
            }

            if (!moveOverride && canMove && !stunned)
            {
                if (!eStats.canAttackWhileMoving)
                {
                    if (!tryAttack)
                    {
                        // Enemy Movement
                        if (!eStats.canPassTowers)
                        {
                            // If The Enemy Tower Can't pass through playerTowers then check for tower collision
                            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(eStats.moveSpeed.x), 0.5f, eStats.enemyLayer);
                            if (!hit)
                            {
                                // If there is no tower in front, move foward
                                transform.position += (eStats.moveSpeed * Time.deltaTime);
                            }
                            else
                            {
                                Tower t = hit.collider.gameObject.GetComponent<Tower>();
                                if (t == null || t.towerLane != enemyLane)
                                {
                                    // If there is no tower in front, move foward
                                    transform.position += (eStats.moveSpeed * Time.deltaTime);
                                }
                            }
                        }
                        else
                        {
                            // If can pass through towers, continue moving forward
                            transform.position += (eStats.moveSpeed * Time.deltaTime);
                        }
                    }
                }
                else
                {
                    // Enemy Movement
                    if (!eStats.canPassTowers)
                    {
                        // If The Enemy Tower Can't pass through playerTowers then check for tower collision
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(eStats.moveSpeed.x), 0.5f, eStats.enemyLayer);
                        if (!hit)
                        {
                            // If there is no tower in front, move foward
                            transform.position += (eStats.moveSpeed * Time.deltaTime);
                        }
                        else
                        {
                            if (hit.collider.gameObject.GetComponent<Tower>().towerLane != enemyLane)
                            {
                                // If there is no tower in front, move foward
                                transform.position += (eStats.moveSpeed * Time.deltaTime);
                            }
                        }
                    }
                    else
                    {
                        // If can pass through towers, continue moving forward
                        transform.position += (eStats.moveSpeed * Time.deltaTime);
                    }
                }
            }
        }

        if (stunTimer > 0f)
        {
            GetComponent<SpriteRenderer>().color = stunnedColor;
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                stunned = false;
                GetComponent<SpriteRenderer>().color = baseColor;
            }
        }
    }

    private void UpdateGUI()
    {
        // Updating the Healthbar
        healthBar.fillAmount = (enemyHealth / enemyMaxHealth);
    }

    private bool TowerInRange()
    {
        Debug.DrawRay(transform.position, Vector2.left * eStats.enemyRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, eStats.shootAngle, eStats.enemyRange * 0.99f, eStats.enemyLayer);

        return (hit && canAttack && hit.collider.gameObject.GetComponent<Tower>().towerLane == enemyLane);
    }

    public void Stun(float time)
    {
        stunned = true;
        stunTimer = time;
    }

    // Called When The Enemy Attacks
    private void TryEnemyAttack()
    {
        // If the Timer is completed then attack
        if (attackTimer <= 0f && canAttack && !stunned)
        {
            // Getting a reference to playerTower collision
            if (TowerInRange())
            {
                if (eStats.enemyAttackStyle == EnemyStats.EnemyAttackStyle.Melee)
                {
                    // Getting the Raycast Hit
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, eStats.enemyRange, eStats.enemyLayer);

                    // Sending Damage Message To Collided Tower
                    Tower tower = hit.collider.gameObject.GetComponent<Tower>();
                    tower.TakeDamage(eStats.enemyDamage);

                    if (eStats.attackSound != null)
                        PlaySound(eStats.attackSound);

                    // Resetting the Attack Timer
                    attackTimer = eStats.enemyAttackSpeed;
                }
                
                if (eStats.enemyAttackStyle == EnemyStats.EnemyAttackStyle.Range)
                {
                    // Instatiate Range Attack Prefab
                    GameObject newBullet = Instantiate(eStats.rangeAttackPrefab);
                    newBullet.GetComponent<EnemyBullet>().eStats = eStats;
                    newBullet.GetComponent<EnemyBullet>().bulletLane = enemyLane;
                    newBullet.transform.position = transform.position + newBullet.GetComponent<EnemyBullet>().eStats.bulletPositionOffset;

                    if (eStats.attackSound != null)
                        PlaySound(eStats.attackSound);

                    // Resetting the Attack Timer
                    attackTimer = eStats.enemyAttackSpeed;
                }
            }
        } else {
            // If the timer is going, then decrement
            attackTimer -= Time.deltaTime;
        }
    }

    // Checking for Death
    private void CheckForDeath()
    {
        if (enemyHealth <= 0f)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().money += eStats.moneyReward;
            GameObject.Destroy(gameObject);
        }
    }

    // Checking for when the enemy passes by the EndLine
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EndLine")
        {
            // Decrementing the Life Counter
            GameObject.Find("GameManager").GetComponent<GameManager>().playerLives--;
            // Destroying The Enemy
            GameObject.Destroy(gameObject);
        }
    }

    // Taking Damage Method
    public void TakeDamage(float damage)
    {
        Debug.Log(name + ": " + damage.ToString());
        enemyHealth -= damage;
        return;
    }

    // Healing Method
    public void Heal(float healAmount)
    {
        enemyHealth = Mathf.Clamp(enemyHealth + healAmount, 0f, enemyMaxHealth);
        return;
    }

    private void PlaySound(AudioClip aClip, float delay = 0f)
    {
        aSource.clip = aClip;
        aSource.PlayDelayed(delay);
    }
}
