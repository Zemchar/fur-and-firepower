using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class HenchmenDirector : MonoBehaviour
{
    public static HenchmenDirector Singleton { get; private set; }

    private Dictionary<GameObject, GlobalVars.TeamAlignment> henchmenDict =
        new Dictionary<GameObject, GlobalVars.TeamAlignment>();
    // Start is called before the first frame update
    
    private void Start()
    {
        Singleton ??= this;
    }
    public void RequestSpawnHenchmen(int count, GameObject owner, GlobalVars.TeamAlignment teamAlignment)
    {
        for (var i = 0; i < count; i++)
        {
            var henchmen = Instantiate(RPSingleton.Access.rp.pc_HenchmanBase, this.transform.position + new Vector3(Random.Range(0f, 10f), 0, Random.Range(0f, 10f)), Quaternion.identity);
            henchmen.GetComponent<HenchmenController>().teamAlignment = teamAlignment;
            henchmen.GetComponent<HenchmenController>().Owner = owner;
            henchmenDict.Append(new KeyValuePair<GameObject, GlobalVars.TeamAlignment>(henchmen, teamAlignment));
        }
    }
    public void RedirectHenchmen(object[] input)
    {
        //Debug.Log("Message Received. Redirecting Henchmen");
        var targets = (Dictionary<GameObject, GameObject>)input[0];
        var requester = (GameObject)input[1];
        foreach (var henchmen in targets)
        {
            Debug.Log("Redirecting " + henchmen.Key.name + " to " + henchmen.Value.name);
            //loops thru dictionary and sets all henchmen to their targets
            henchmen.Key.SendMessage("SetTarget", henchmen.Value);
            Debug.Log("Redirected " + henchmen.Key.name + " to " + henchmen.Value.name);
        }
        
        
    }
}
