using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jet_Trooper : MonoBehaviour {

    [Header("Jet Trooper Variables")]
    public EnemyStats eStats;

    [Header("Override Calls")]
    public readonly bool overrideMove = false;
    public readonly bool overrideAttack = false;
    public readonly bool overrideDeath = false;

    [Header("Jet Trooper Move Variables")]
    public bool isFlying = false;
    public bool isGrounded = true;
    public Vector3 radiusOffset;
    public Vector3 dropRadius;
    public float launchRange = 1.5f;
    public float flyingOffset;

    private float jumpCooldown = 0f;
    private float startY = 0f;

    [Header("Jet Pack Sounds")]
    public AudioClip takeOffClip;
    public AudioClip flyingClip;
    public AudioClip landingClip;

    private AudioSource aSource;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();

        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.moveOverride = overrideMove;
        enemy.attackOverride = overrideAttack;
        enemy.deathOverride = overrideDeath;
    }

    private void Update()
    {
        // Move the Jet Trooper
        if (jumpCooldown <= 0f)
            Jumping();
        else
        {
            jumpCooldown -= Time.deltaTime;
            if (!aSource.isPlaying)
            {
                PlaySound(flyingClip);
            }
        }

        if (isFlying)
        {
            transform.position = new Vector3(transform.position.x, startY + flyingOffset, transform.position.z);
        }

        gameObject.GetComponent<Enemy>().canAttack = isFlying;
        return;
    }

    private void Jumping()
    {
        // Checking to land if flying
        if (isFlying)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, dropRadius, 0f, Vector2.zero, 0f, eStats.enemyLayer);
            if (!hit)
            {
                isFlying = false;
                isGrounded = true;

                PlaySound(landingClip);

                transform.position = new Vector3(transform.position.x, transform.position.y - flyingOffset, transform.position.z);

                jumpCooldown = 0.5f;
            }
        }

        if (isGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, launchRange, eStats.enemyLayer);
            if (hit)
            {
                PlaySound(takeOffClip);

                startY = transform.position.y;

                isFlying = true;
                isGrounded = false;

                jumpCooldown = 0.5f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position + radiusOffset, dropRadius);
        Gizmos.DrawLine(transform.position + eStats.bulletPositionOffset, transform.position + eStats.bulletPositionOffset + eStats.shootAngle);
    }

    private void PlaySound(AudioClip aClip, float delay = 0f)
    {
        aSource.clip = aClip;
        aSource.PlayDelayed(delay);
    }

}
