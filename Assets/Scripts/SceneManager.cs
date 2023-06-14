using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : ScriptableObject
{
    public static List<Scene> scenes = new();

    public static void LoadScene(int index) // i think index is the best for this?
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scenes[index].name);
    }
    public static void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
