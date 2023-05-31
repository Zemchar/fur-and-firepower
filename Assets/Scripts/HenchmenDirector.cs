using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HenchmenDirector : MonoBehaviour
{

    private Dictionary<GameObject, GlobalVars.TeamAlignment> henchmenDict =
        new Dictionary<GameObject, GlobalVars.TeamAlignment>();
    // Start is called before the first frame update
    
    public void RequestSpawnHenchmen(int count, GameObject Owner, GlobalVars.TeamAlignment teamAlignment)
    {
        for (var i = 0; i < count; i++)
        {
            var henchmen = Instantiate(Resources.Load<GameObject>("Prefabs/Henchman"), this.transform.position + new Vector3(Random.Range(0f, 10f), 0, Random.Range(0f, 10f)), Quaternion.identity);
            henchmen.GetComponent<HenchmenController>().teamAlignment = teamAlignment;
            henchmen.GetComponent<HenchmenController>().Owner = Owner;
            henchmenDict.Append(new KeyValuePair<GameObject, GlobalVars.TeamAlignment>(henchmen, teamAlignment));
        }
    }
    public void RedirectHenchmen(Dictionary<GameObject, GameObject> targets, GameObject Requester)
    {
        foreach (var henchmen in targets)
        {
            if (henchmenDict[henchmen.Key] != henchmen.Value.GetComponent<HenchmenController>().teamAlignment)
            {
                Debug.Log("Henchmen " + henchmen.Key.name + " is not on the same team as " + Requester.gameObject.name + "and cannot be redirected.");
                continue;
            }
            //loops thru dictionary and sets all henchmen to their targets
            henchmen.Key.GetComponent<HenchmenController>().SetTarget(henchmen.Value);
        }
    }
}
