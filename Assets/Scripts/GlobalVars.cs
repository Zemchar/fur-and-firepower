using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    
    [Serializable]
    public class TeamProperties
    {
        [Tooltip("The team this family belongs to")]
        public TeamAlignment Alignment;
        [Tooltip("Default color value for anything related to this family")]
        public Color Color;
        [Tooltip("Name of the family. This will be displayed in the UI and in fly-in intro cutscenes")]
        public string DisplayName;
        [Tooltip("Small, recognizable icon for the family")]
        public Image Icon;
        //Stats Here
    }

    
    
    public enum TargetType
    {
        None,
        Henchman,
        Boss_OR_Capo,
        Structure_Shop,
        Structure_CapoHome,
        Structure_ShopFriendly,
        Structure_CapoHomeFriendly,
        Structure_PoliceStation,
    }

    public enum StreetState
    {
        UnControlled,
        Unrest,
        Controlled, 
        
    }

    public enum Minigame
    {
        StardewCharmer,
    }

}

public enum GameMode
{
    single,
    multi
}

public static class Utils
{
    public static void print(string message)
    {
        Debug.Log(message);
    }
    public static GlobalVars.TeamProperties GetTeam(GlobalVars.TeamAlignment team, GlobalVars.TeamProperties[] arr)
    {
        foreach (GlobalVars.TeamProperties teamProp in arr)
        {
            if (teamProp.Alignment == team)
            {
                return teamProp;
            }
        }
        return null;
    }
    
}

