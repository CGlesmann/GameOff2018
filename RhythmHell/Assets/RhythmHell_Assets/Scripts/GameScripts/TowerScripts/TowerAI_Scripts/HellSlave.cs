using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
[RequireComponent(typeof(LineRenderer))]
public class HellSlave : MonoBehaviour {

    [Header("Hell Slave Variables")]
    public bool attached;
    public SlaveMaster master = null;

    private GameObject attachedTarget = null;
    private Tower hTower;
    private LineRenderer lRend;

    private void Awake()
    {
        hTower = GetComponent<Tower>();
        lRend = GetComponent<LineRenderer>();

        attachedTarget = null;
    }

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            // Toggling movement based on attachment
            hTower.canMove = (!attached);

            if (attached)
            {
                if (master == null)
                {
                    Debug.Log("Master Not Getting Assigned");
                    return;
                }

                // Draw Line to Slave Master
                Vector3 pos0 = new Vector3(transform.position.x, transform.position.y, -5f);
                Vector3 pos1 = new Vector3(master.gameObject.transform.position.x, master.gameObject.transform.position.y, -5f);

                lRend.SetPosition(0, pos0);
                lRend.SetPosition(1, pos1);

                // Attached Movement Script Down Here
                if (attachedTarget == null)
                {
                    RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 5f, 0f, Vector2.zero, 0f, hTower.tStats.enemyLayer);
                    if (hit)
                    {
                        Debug.Log("Slave is locked in");
                        attachedTarget = hit.collider.gameObject;
                    }
                }
                else
                {
                    Vector3 targetPos = attachedTarget.transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector3(targetPos.x - 0.75f, targetPos.y, targetPos.z), 1.5f * Time.deltaTime);
                }
            }
        }
    }

    public void AttachToMaster(SlaveMaster sm)
    {
        lRend.enabled = true;
        attached = true;

        master = sm;
    }

    public void DeAttachFromMaster()
    {
        lRend.enabled = false;
        attached = false;

        master = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5f);
    }

}
