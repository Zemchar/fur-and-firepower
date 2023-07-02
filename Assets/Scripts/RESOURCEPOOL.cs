using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Resources", menuName = "ScriptableObjects/RESOURCEPOOL", order = 1)]
public class RESOURCEPOOL : ScriptableObject
{
    /* LISTEN UP
     * THIS FILE. Is where you should put all resources you need to use in the game multiple times.
     * Use this for stuff like changing skins of characters, spawning more characters etc.
     * PLEASE adhere to naming conventions in this file. This is like the only one I really care about
     * ALL of the variables must be prefixed with one of the things listed below, based on what they are meant for.
     * Update the prefix guide below as you add more stuff!
     *
     * (oh also all variables need to be declared as public static)
     * ####################################### THE IMPORTANT BIT ##############################################
     * pc = A player controllable object. Stuff the player can directly influence like henchmen or the base bosses.
     * 
     */
    public GameObject pc_HenchmanBase; // Base henchman prefab
}
