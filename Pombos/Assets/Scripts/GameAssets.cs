using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public GameObject damagePopup;
    public AmmoIndicator ammoIndicator;

    private static GameAssets _instance;
    public static GameAssets instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        ammoIndicator = gameObject.GetComponent<AmmoIndicator>();
    }
}
