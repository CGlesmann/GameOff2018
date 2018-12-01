using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic_Tower", menuName = "Tower Assets/New Tower", order = 1)]
public class TowerStats : ScriptableObject {

    //Tower Enums
    public enum TowerTags { Bones, Mortal, Demon };
    public enum TowerAttackStyle { Melee, Range, Collision, NoAttack };

    [Header("Tower Info Vars")]
    public Sprite towerSprite;
    public string towerName;
    public string towerDescription;

    [Header("Tower Store Variables")]
    public float towerCost = 50f;

    [Header("Tower Movement Stats")]
    public Vector3 moveVector;

    public LayerMask enemyLayer;
    public bool stopsEnemies; // Implies that the tower also Comfronts them
    public int enemyCapacity;

    [Header("Tower Combat Stats")]
    public float towerHealth;
    public float towerDamage;
    public float towerAttackSpeed;
    public float towerKnockBackStrength = 0f;

    public TowerAttackStyle towerAStyle = TowerAttackStyle.Melee;
    public float towerRange;
    public bool canAttackWhileMoving = false;
    
    [Header("Tower Bullet Variables")]
    public GameObject rangeAttackPrefab = null;
    public Vector3 shootAngle = Vector2.right;
    public Vector3 bulletMoveVector;
    public Vector3 bulletPositionOffset = Vector3.zero;

    [Header("Tower Attack Sound(s)")]
    public AudioClip attackSound;
    public AudioClip damageSound;

    [Header("Tower Tags")]
    public TowerTags[] tags; 

}
