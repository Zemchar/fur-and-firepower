using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    
    private GlobalVars.TargetType defaultAction = GlobalVars.TargetType.None;
    private GameObject CurrentTarget;
    private GlobalVars.TargetType CurrentTargetType;
    [Header("Behavior Properties")]
    [Tooltip("Controls the distance until the henchmen will start attacking.")] [SerializeField]
    private Vector3 min_TargetDistance; // When will the henchmen start attacking
    public HenchmenState henchmenState;
    public GlobalVars.TeamAlignment teamAlignment;
    public GameObject Owner;
    
    private NavMeshAgent agent;
    private int Ammo = 50;
    private bool isShooting = false;
    [Header("Bullet Properties")]
    [SerializeField] int maxAmmo = 50;
    [SerializeField] float shootDelay;
    [Tooltip("X is Min, Y is Max. Controls how many bullets are fired at once.")]
    [SerializeField] Vector2 minMaxBullets = new Vector2(1, 3);
    [SerializeField] private Vector3 MaxSpread;
    [SerializeField] Transform bulletSpawnPoint;
    [Header("UI/UX Properties")]
    [FormerlySerializedAs("bulletPrefab")] [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private Canvas SelectedInicator;
    [SerializeField] private Animation Bobber;
    

    // Start is called before the first frame update
    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        SelectedInicator.enabled = false;
        Ammo = maxAmmo;
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
                    // throw new NotImplementedException();
                    break;
                case GlobalVars.TargetType.Enemy:
                    agent.SetDestination(CurrentTarget.transform.position);
                    Debug.DrawLine(gameObject.transform.position, CurrentTarget.transform.position, Color.blue);
                    //check if target is in range
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
            }
        }
    }
    
    private Vector3 CalculateFireDir()
    {
        Vector3 dir = transform.forward;
        //add spread
        dir += new Vector3(Random.Range(-MaxSpread.x, MaxSpread.x), Random.Range(-MaxSpread.y, MaxSpread.y),
            Random.Range(-MaxSpread.z, MaxSpread.z));
        return dir.normalized;
    }

    private IEnumerator Shoot()
    {
        print("Shooting");
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
                    TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
                }
            }

            Ammo -= bullets;
            yield return new WaitForSeconds(shootDelay);

        }
    }

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
   
    public void SetTarget(GameObject target)
    {
        try{
            CurrentTargetType = target.GetComponent<Accessibleproperties>().TargetType;
            CurrentTarget = target;
            Bobber.Stop();
            SelectedInicator.enabled = false;
        }
        catch{
            Debug.LogError($"Target {target.name} does not have AccessibleProperties TargetType, defualting.");
            CurrentTargetType = defaultAction; 
        } }

    public void RequestSelect(GameObject requester)
    {
        Debug.Log($"Request to select {this.gameObject.name} from {requester.name}");
        if (requester.GetComponent<Accessibleproperties>().TeamAlignment == teamAlignment)
        {
            SelectedInicator.enabled = true;
            Bobber.Play();
            requester.SendMessage("Select", this.gameObject);
        }
        //If not true, do not return confirmation message

    }

}
