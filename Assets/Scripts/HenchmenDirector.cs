using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HenchmenDirector : MonoBehaviour
{
    [SerializeField] int initalHenchmenCount = 10;
    [SerializeField] GlobalVars.TeamAlignment teamAlignment;

    private List<GameObject> henchmenList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < initalHenchmenCount; i++)
        {
            var henchmen = Instantiate(Resources.Load<GameObject>("Prefabs/Henchman"), this.transform.position + new Vector3(Random.Range(0f, 10f), 0, Random.Range(0f, 10f)), Quaternion.identity);
            henchmen.GetComponent<HenchmenController>().teamAlignment = teamAlignment;
            henchmenList.Append(henchmen);
        }
    }
    
    public void RedirectHenchmen(Dictionary<GameObject, GameObject> targets)
    {
        foreach (var henchmen in targets)
        {
            //loops thru dictionary and sets all henchmen to their targets
            henchmen.Key.GetComponent<HenchmenController>().SetTarget(henchmen.Value);
        }
    }
}