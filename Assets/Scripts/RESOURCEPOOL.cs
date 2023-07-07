using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Resources", menuName = "ScriptableObjects/RESOURCEPOOL", order = 1)]
public class RESOURCEPOOL : ScriptableObject
{
    /* LISTEN UP
     * THIS FILE. Is where you should put all resources you need to use in the game multiple times. (1+ prefabs or you suspect it will be necessary to use more than once)
     * Use this for stuff like changing skins of characters, spawning more characters etc.
     * PLEASE adhere to naming conventions in this file. This is like the only one I really care about
     * ALL of the variables must be prefixed with one of the things listed below, based on what they are meant for.
     * Update the prefix guide below as you add more stuff!
     * 
     * (oh also all variables need to be declared as public static)
     * ####################################### THE IMPORTANT BIT ##############################################
     * pc = A player controllable object. Stuff the player can directly influence like henchmen or the base bosses.
     * gcm = Game Critical Model. Stuff essential for the base game to work, such as road pieces or building prefabs.
     * gcv = Game Critical Values. Stuff that needs to be stored, like random gen seeds
     */
    [Header("Player Controllable Objects")]
    public GameObject pc_HenchmanBase; // Base henchman prefab
    
    [Header("Game Critical Models")]
    public GameObject[] gcm_RoadPieces; // Road pieces
    
    [Header("Game Critical Values")]
    public int gcv_RandomSeed; // Random Seed
    public int gcv_GridSize; // Grid Size
    [Tooltip("List of all the teams in the game and their properties")]
    public GlobalVars.TeamProperties[] gcv_teamProps; // Team Colors
}
