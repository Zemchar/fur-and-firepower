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
    public void RedirectHenchmen(object[] input)
    {
        Debug.Log("Message Received. Redirecting Henchmen");
        var targets = (Dictionary<GameObject, GameObject>)input[0];
        var requester = (GameObject)input[1];
        foreach (var henchmen in targets)
        {
            Debug.Log("Redirecting " + henchmen.Key.name + " to " + henchmen.Value.name);
                // if (henchmen.Key.gameObject.GetComponent<Accessibleproperties>().TeamAlignment != requester.GetComponent<Accessibleproperties>().TeamAlignment)
                // {
                //     Debug.Log("Henchmen " + henchmen.Key.name + " is not on the same team as " + requester.gameObject.name + "and cannot be redirected.");
                //     continue;
                // } Likely not needed as this is checked in the HenchmenController
            //loops thru dictionary and sets all henchmen to their targets
            henchmen.Key.SendMessage("SetTarget", henchmen.Value);
            Debug.Log("Redirected " + henchmen.Key.name + " to " + henchmen.Value.name);
        }
    }
}
