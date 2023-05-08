using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class PauseMenuPresenter : MonoBehaviour
{
    void Awake() 
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement; 
        root.Q<Button>("CloseStatsBtn").clicked += () => root.visible = false;
        root.Q<Button>("QuitBtn").clicked += () => root.visible = false;
    }
}
