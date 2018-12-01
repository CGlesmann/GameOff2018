using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stat", menuName = "Enemy Assets/Create New Enemy", order = 1)]
public class EnemyStats : ScriptableObject {

    // Enemy Enums
    public enum EnemyAttackTags { Gun, Explosive, Holy }
    public enum EnemyAttackStyle { NoAttack, Melee, Range };
    public enum EnemyTarget { Towers, Enemies };

    [Header("Other Variables")]
    public int moneyReward;

    [Header("Movement Variables")]
    public LayerMask enemyLayer;
    public Vector3 moveSpeed;
    public bool canPassTowers;

    [Header("Enemy Stats")]
    public EnemyTarget eTarget = EnemyStats.EnemyTarget.Towers;
    public float maxHealth;
    public float enemyDamage;
    public float enemyAttackSpeed;

    [Header("Enemy Attacking Variables")]
    public bool canAttackWhileMoving = false;
    public EnemyAttackStyle enemyAttackStyle = EnemyStats.EnemyAttackStyle.Melee;
    public Vector3 shootAngle;
    public float enemyRange = 1f;
    public float splashRadius = 0f; // If equal 0f then no splash damage
    
    [Header("Enemy Range Variables")]
    public GameObject rangeAttackPrefab = null;
    public Vector3 bulletMoveVector = Vector3.zero;
    public Vector3 bulletPositionOffset = Vector3.zero;

    [Header("Enemy Sound References")]
    public AudioClip attackSound;

    [Header("Enemy Attack Tag(s)")]
    public EnemyAttackTags[] eAttackTags;
}
