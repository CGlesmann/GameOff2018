using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveMaster : MonoBehaviour {

    [Header("SlaveMaster Variables")]
    public LayerMask slaveLayer;
    public string slaveTag;
    public float attachRadius = 3f;

    public int maxAttachedSlaves = 5;
    public GameObject[] attachedSlaves;
    private int slaveIndex = 0;

    private void Start()
    {
        attachedSlaves = new GameObject[maxAttachedSlaves];
        slaveIndex = 0;
    }

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            if (slaveIndex < attachedSlaves.Length - 0)
            {
                // Getting an Array of Nearby Hell Slaves
                RaycastHit2D[] slaves = Physics2D.BoxCastAll(transform.position, Vector2.one * attachRadius, 0, Vector2.zero, 0f, slaveLayer);
                foreach (RaycastHit2D hit in slaves)
                {
                    GameObject slaveObj = hit.collider.gameObject;
                    if (slaveObj.tag == slaveTag && !SlaveAlreadyChained(slaveObj))
                    {
                        attachedSlaves[slaveIndex++] = slaveObj;

                        HellSlave slave = slaveObj.GetComponent<HellSlave>();
                        slave.AttachToMaster(this);

                        // Playing the Whip Sound
                        GetComponent<AudioSource>().clip = GetComponent<Tower>().tStats.attackSound;
                        GetComponent<AudioSource>().Play();

                        // Play the Whip Animation
                        GetComponent<Animator>().SetBool("isAttacking", true);
                    }
                }
            }
        }
    }

    private bool SlaveAlreadyChained(GameObject slaveObj)
    {
        HellSlave hSlave = slaveObj.GetComponent<HellSlave>();

        return (hSlave.attached);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, Vector3.one * attachRadius);
    }

}
