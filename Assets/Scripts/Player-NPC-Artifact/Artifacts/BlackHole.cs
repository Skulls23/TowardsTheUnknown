using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Artifact, IArtifact
{
    public BlackHole()
    {
        cost = 2;

        distanceMin = 3;
        distanceMax = 5;

        areaOfEffectMin = 2;
        areaOfEffectMax = 2;

        damageMin = 100;
        damageMax = 100;

        maximumUsePerTurn = 1;
        cooldown = 2;
        lootRate = 0.01f;

        sizeX = 2;
        sizeY = 2;
    }

    private void Awake()
    {
        Prefab = (GameObject)Resources.Load("VFX/BlackHole/BlackHole", typeof(GameObject));
    }

    void IArtifact.Launch(RaycastHit hitTerrain, Animator animator)
    {

        //TODO We must know by the artifact if it's a Tile artefact or an AOE artifact then decide which mask to take

        RaycastHit hitAbove;
        if (Physics.Raycast(hitTerrain.transform.position, Vector3.up, out hitAbove, 1 << LayerMask.GetMask("Player", "Enemy")))
        {
            GameObject enemy = hitAbove.collider.gameObject;

            Vector3 position = hitTerrain.transform.position;
            position.y += 2;
            animator.SetTrigger("attacking");

            //StartCoroutine(LaunchFXAndAnim(animator, position));
            Instantiate(this.Prefab, position, Quaternion.identity);

            int damage = Random.Range(damageMin, damageMax+1);
            enemy.GetComponentInParent<Enemy>().LowerHealth(damage);
        }
    }

    /*public IEnumerator LaunchFXAndAnim(Animator animator, Vector3 position)
    {
        while(!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            yield return null;

        Instantiate(this.Prefab, position, Quaternion.identity);
    }*/

    bool IArtifact.IsRaycastHitAccepted(RaycastHit hitTerrain)
    {
        if (Physics.Raycast(hitTerrain.transform.position, Vector3.up, 1 << LayerMask.GetMask("Player", "Enemy")))
            return true;
        else 
            return false;
    }

    int IArtifact.GetMaxDistance()
    {
        return distanceMax;
    }

    int IArtifact.GetMinDistance()
    {
        return distanceMin;
    }
}
