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
        Henchman,
        Structure_Shop,
        Structure_CapoHome,
        Structure_ShopFriendly,
        Structure_CapoHomeFriendly,
        Structure_PoliceStation,
    }

}

public static class Utils
{
    public static void print(string message)
    {
        Debug.Log(message);
    }
    
}

