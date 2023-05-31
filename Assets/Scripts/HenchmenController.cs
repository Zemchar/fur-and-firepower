using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmenController : MonoBehaviour
{
    public GlobalVars.TeamAlignment teamAlignment;
    public GameObject Owner;
    private GlobalVars.TargetType defaultAction = GlobalVars.TargetType.None;
    private GameObject CurrentTarget;
    private GlobalVars.TargetType CurrentTargetType;
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
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
                throw new NotImplementedException();

        }
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
