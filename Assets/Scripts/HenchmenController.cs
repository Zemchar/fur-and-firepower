using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HenchmenController : NetworkBehaviour
{
    public enum HenchmenState
    {
        None,
        Guarding,
        Moving,
        Attacking,
        Dead
    }
    
    private GameObject CurrentTarget;
    private GlobalVars.TargetType CurrentTargetType;
    [SerializeField] private int DAYNUMBER = 1;
    [FormerlySerializedAs("defaultAction")]
    [Header("Behavior Properties")]
    [Tooltip("Controls what the henchmen will default to when selecting an invalid target.")]
    [SerializeField] GlobalVars.TargetType defaultTargetType = GlobalVars.TargetType.None;
    [Tooltip("Controls the distance until the henchmen will start attacking.")]
    [SerializeField] private Vector3 min_TargetDistance; // When will the henchmen start attacking
    [SerializeField] private float LookAroundTime;
    public HenchmenState henchmenState;
    public float health = 100;
    public GlobalVars.TeamAlignment teamAlignment;
    public GameObject Owner;
    private NavMeshAgent agent;
    private int Ammo = 50;
    private bool isShooting = false;
    [FormerlySerializedAs("damage")]
    [Header("Bullet Properties")]
    [SerializeField] private float baseDamage;

    [SerializeField] int maxAmmo = 50;
    [SerializeField] float shootDelay;
    [SerializeField] float reloadDelay;
    [Tooltip("X is Min, Y is Max. Controls how many bullets are fired at once.")]
    [SerializeField] Vector2 minMaxBullets = new Vector2(1, 3);
    [SerializeField] private Vector3 MaxSpread;
    [SerializeField] Transform bulletSpawnPoint;
    [Header("UI/UX Properties")]
    [FormerlySerializedAs("bulletPrefab")] [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private Canvas SelectedInicator;
    [SerializeField] private Animation Bobber;
    [SerializeField] private BoxCollider TooCloseCollider;
    private bool isGuarding = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        SelectedInicator.enabled = false;
        Ammo = maxAmmo;
        TooCloseCollider.enabled = false;
        isGuarding = false;
    }
    // Update is called once per frame
    void Update()
    {
        void CheckAttack(GlobalVars.TargetType targetType)
        {
            switch (DAYNUMBER)
            {
                case 1:
                    if (Vector3.Distance(this.transform.position, CurrentTarget.transform.position) <=
                        min_TargetDistance.magnitude)
                    {
                        Debug.DrawLine(gameObject.transform.position, CurrentTarget.transform.position, Color.green);
                        agent.ResetPath();
                        henchmenState = HenchmenState.Attacking;
                    }
                    break;
                case 2:
                    if (Vector3.Distance(this.transform.position, CurrentTarget.transform.position) <=
                        min_TargetDistance.magnitude)
                    {
                        Debug.DrawLine(gameObject.transform.position, CurrentTarget.transform.position, Color.green);
                        agent.ResetPath();
                        henchmenState = targetType == GlobalVars.TargetType.Henchman ? HenchmenState.Attacking : HenchmenState.Guarding;
                    }
                    break;
                default:
                    if (Vector3.Distance(this.transform.position, CurrentTarget.transform.position) <=
                        min_TargetDistance.magnitude)
                    {
                        Debug.DrawLine(gameObject.transform.position, CurrentTarget.transform.position, Color.green);
                        agent.ResetPath();
                        henchmenState = HenchmenState.Attacking;
                    }
                    break;
            }
            
        }
        if (henchmenState == HenchmenState.None)
        {
            switch (CurrentTargetType)
            {
                case GlobalVars.TargetType.None:
                    TooCloseCollider.enabled = true;
                    henchmenState = HenchmenState.Guarding;
                    CurrentTarget = Owner; // Guard the owner
                    break;
                case GlobalVars.TargetType.Henchman: // Basic enemy type. Other henchmen and bosses. Most flexible.
                    agent.SetDestination(CurrentTarget.transform.position);
                    Debug.DrawLine(gameObject.transform.position, CurrentTarget.transform.position, Color.blue);
                    //check if target is in range
                    TooCloseCollider.enabled = false; // too close collider is disabled so that the henchmen can get close enough to attack and not get distracted
                    CheckAttack(GlobalVars.TargetType.Henchman);
                    break;
                case GlobalVars.TargetType.Structure_Shop:
                    agent.SetDestination(CurrentTarget.transform.position);
                    Debug.DrawLine(gameObject.transform.position, CurrentTarget.transform.position, Color.red);
                    TooCloseCollider.enabled = false; // too close collider enabled so they can find new targets. 
                    CheckAttack(GlobalVars.TargetType.Structure_Shop);
                    break;
                case GlobalVars.TargetType.Boss_OR_Capo: // currently will do the same thing as Henchman type. Its here so that we can change behavior later.
                    agent.SetDestination(CurrentTarget.transform.position);
                    Debug.DrawLine(gameObject.transform.position, CurrentTarget.transform.position, Color.blue);
                    //check if target is in range
                    TooCloseCollider.enabled = false; // too close collider is disabled so that the henchmen can get close enough to attack and not get distracted
                    CheckAttack(GlobalVars.TargetType.Henchman);
                    break;
                default:
                    TooCloseCollider.enabled = true;
                    henchmenState = HenchmenState.Guarding;
                    CurrentTarget = Owner; // Guard the owner
                    break;
            }
        }
        else
        {
            switch (henchmenState)
            {
                case HenchmenState.Attacking:
                    Debug.DrawLine(gameObject.transform.position, CurrentTarget.transform.position, Color.red);
                    if (!CurrentTarget.activeSelf)
                    {
                        henchmenState = HenchmenState.None;
                        break;
                    }
                    else if(!isShooting)
                        StartCoroutine(Shoot());
                    break;
                case HenchmenState.Guarding:
                    if(!isGuarding){ 
                        Collider guardianArea = CurrentTarget.GetComponent<Accessibleproperties>().GaurdianArea;
                        StartCoroutine(GuardWithinArea(guardianArea));
                    }
                    break;
            }
        }
    }

    private IEnumerator GuardWithinArea(Collider gaurdianArea)
    {
        isGuarding = true;
        Vector3 TargetPos = RandomNavSphere(gaurdianArea.transform.position, 1, -1);
        /* Sometimes the TargetPos is infinity. I Have no clue why.
         * If infinity is passed to the agent, it will break the agent permanently.
         * This is a temporary fix. */
        if (TargetPos.magnitude == Mathf.Infinity || TargetPos.magnitude == Mathf.NegativeInfinity)
        {
            //print("TargetPos is infinity");
            isGuarding = false;
            yield break; //Retry 
        }
        agent.SetDestination(TargetPos);
        while(Vector3.Distance(transform.position, TargetPos) > 1)
        {
            Debug.DrawLine(transform.position, TargetPos, Color.yellow);
            yield return null;
        }
        yield return new WaitForSeconds(LookAroundTime);
        isGuarding = false;
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Accessibleproperties>().TeamAlignment != teamAlignment && 
            (other.gameObject.GetComponentInParent<Accessibleproperties>().TargetType == GlobalVars.TargetType.Henchman || other.gameObject.GetComponentInParent<Accessibleproperties>().TargetType == GlobalVars.TargetType.Boss_OR_Capo))
        {
            SetTarget(other.gameObject.transform.parent.gameObject); // why is this so needlessly long.
            //Either way, insures SetTarget can get accessible properties.
        }
        else
        {
            return;
        }
    }
    
    private Vector3 CalculateFireDir()
    {
        transform.LookAt(CurrentTarget.transform);
        Vector3 dir = transform.forward;
        //add spread
        dir += new Vector3(Random.Range(-MaxSpread.x, MaxSpread.x), Random.Range(-MaxSpread.y, MaxSpread.y),
            Random.Range(-MaxSpread.z, MaxSpread.z));
        return dir.normalized;
    }
    /// <summary>
    /// Shoot function
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shoot()
    { 
        if (isShooting)
        {
            yield break; // dont shoot if already shooting
        }
        isShooting = true;
        while (Ammo > 0)
        {
            //shoot
            var bullets = UnityEngine.Random.Range((int)minMaxBullets.x, (int)minMaxBullets.y);
            //This uses linq to get the first object with the tag "BulletSpawnPoint" and gets its position in child objects.
            //the other function was not working. Linq is helpful
            for (var i = 0; i < bullets; i++)
            {
                if (Physics.Raycast(
                        bulletSpawnPoint.position,
                        CalculateFireDir(), out var hit))
                {
                    if (hit.collider.gameObject.GetComponentInParent<Accessibleproperties>().TeamAlignment != teamAlignment)
                    {
                        hit.collider.gameObject.SendMessage("InflictDamage", CalcDamage(baseDamage));
                    }
                    TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
                }
            }

            Ammo -= bullets;
            yield return new WaitForSeconds(shootDelay);

        }
        yield return new WaitForSeconds(reloadDelay);
        Ammo = maxAmmo;
        isShooting = false;
        yield break; // stop the coroutine
    }
    /// <summary>
    /// Bullet trail coroutine
    /// </summary>
    /// <param name="trail"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 start = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(start, hit.point, time);
            time += Time.deltaTime + trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal)); 
        Destroy(trail.gameObject, trail.time);
    }
   
    /// <summary>
    /// Redirects the henchmen to a new target.
    /// Stops ALL coroutines.
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(GameObject target)
    {
        StopAllCoroutines();
        isShooting = false;
        isGuarding = false;
        try{
            CurrentTargetType = target.GetComponent<Accessibleproperties>().TargetType;
            henchmenState = HenchmenState.None; // Required to redirect
            CurrentTarget = target;
            Bobber.Stop();
            SelectedInicator.enabled = false;
            //print($"New Target Set ({target.name}) of type {CurrentTargetType}");
        }
        catch{
            //Debug.LogError($"Target {target.name} does not have AccessibleProperties TargetType, defualting.");
            henchmenState = HenchmenState.None; // Required to redirect
            CurrentTargetType = defaultTargetType;
            CurrentTarget = null;
            Bobber.Stop();
            SelectedInicator.enabled = false;
        } }

    /// <summary>
    /// This is called by a player character to request to select this object.
    /// If the requester is on the same team, it will send a confirmation message.
    /// </summary>
    /// <param name="requester" description="The game object requesting the selection"></param>
    public void RequestSelect(GameObject requester)
    {
        //Debug.Log($"Request to select {this.gameObject.name} from {requester.name}");
        if (requester.GetComponent<Accessibleproperties>().TeamAlignment == teamAlignment)
        {
            SelectedInicator.enabled = true;
            Bobber.Play();
            requester.SendMessage("Select", this.gameObject);
        }
        //If not true, do not return confirmation message
    }
    /// <summary>
    /// Generates a random point on a sphere to aid with wandering
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="distance"></param>
    /// <param name="layermask"></param>
    /// <returns></returns>
    public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }
    /// <summary>
    /// Simple Damage Calculator
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageModifier"></param>
    /// <returns></returns>
    public float CalcDamage(float damage, float damageModifier = 1)
    {
        return damage * damageModifier;
    }
}
