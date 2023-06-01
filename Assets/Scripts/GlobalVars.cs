using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : ScriptableObject
{
    public enum TeamAlignment
    {
        None,
        Snazz,
        Sting,
        Moxie,
        Whack
    }
    public enum TargetType
    {
        None,
        Enemy,
        Structure_Shop,
        Structure_CapoHome,
        Structure_ShopFriendly,
        Structure_CapoHomeFriendly,
        Structure_PoliceStation,
    }
    
}
