using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMinigame : MonoBehaviour
{
    [SerializeField] private GameObject storeUI;
    private GameObject canvas;
    private GameObject healthBar;
    private Image healthNum;
    private GameObject minigame;
    private Image storeOwner;
    private Image gameBar;
    private GameObject startButton;
    private GameObject complete;
    private GameObject wrong;

    private void Start()
    {
        canvas = Instantiate(storeUI, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
        canvas.transform.localPosition = new Vector3(0.525f, 0.1f, 0);
        canvas.transform.localRotation = Quaternion.Euler(0, 90, 0);
    }
    public void StartMinigame()
    {
        
    }
}
