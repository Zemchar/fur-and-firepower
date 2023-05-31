using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HenchmenController : MonoBehaviour
{
    public enum HenchmenState
    {
        None,
        Idle,
        Moving,
        Attacking,
        Dead
    }
    public HenchmenState henchmenState;
    public GlobalVars.TeamAlignment teamAlignment;
    public GameObject Owner;
    private GlobalVars.TargetType defaultAction = GlobalVars.TargetType.None;
    private GameObject CurrentTarget;
    private GlobalVars.TargetType CurrentTargetType;
    [Tooltip("Controls the distance until the henchmen will start attacking.")]
    [SerializeField] private Vector3 min_TargetDistance; // When will the henchmen start attacking
    private NavMeshAgent agent;
    private int Ammo;
    private float shootDelay;
    private bool isShooting = false;
    [Tooltip("X is Min, Y is Max")]
    [SerializeField] Vector2 minMaxBullets = new Vector2(1, 3);
    // Start is called before the first frame update
    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (henchmenState == HenchmenState.None)
        {
            switch (CurrentTargetType)
            {
                case GlobalVars.TargetType.None:
                    /*This one is going to be the hardest to implement, so I'm leaving it for last.\\
                     * It will be used for things like protecting the player, or just standing around.\\
                     */
                    throw new NotImplementedException();
                    break;
                case GlobalVars.TargetType.Enemy:
                    agent.SetDestination(CurrentTarget.transform.position);
                    //check if target is in range
                    if (Vector3.Distance(this.transform.position, CurrentTarget.transform.position) <
                        min_TargetDistance.magnitude)
                    {
                        agent.ResetPath();
                        henchmenState = HenchmenState.Attacking;
                    }

                    break;
            }
        }
        else
        {
            switch (henchmenState)
            {
                case HenchmenState.Attacking:
                    if(!isShooting)
                        StartCoroutine((string)Shoot());
                    break;
            }
        }
    }

    private IEnumerable Shoot()
    {
        if (isShooting)
        {
            yield break;
        }
        isShooting = true;
        while (Ammo > 0)
        {
            //shoot
            var bullets = UnityEngine.Random.Range((int)minMaxBullets.x, (int)minMaxBullets.y);
            for (var i = 0; i < bullets; i++)
            {
                //spawn bullet
                
            }
            yield return new WaitForSeconds(shootDelay);
        }
        isShooting = false;
    }
    private void Brain(GlobalVars.TargetType currentType)
    {
        
    }
    public void SetTarget(GameObject target)
    {
        try{
            CurrentTargetType = target.GetComponent<AccessableProperties>().TargetType;
            CurrentTarget = target;
        }
        catch{
            Debug.LogError("Target does not have AccessableProperties, defualting.");
            CurrentTargetType = defaultAction;
        } }

}
