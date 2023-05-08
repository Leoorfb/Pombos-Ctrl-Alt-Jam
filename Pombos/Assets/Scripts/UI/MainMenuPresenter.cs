using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuPresenter : MonoBehaviour
{
    private void Start() {
        
    }
    private void Awake() 
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement; 
        root.Q<Button>("Start").clicked += () => SceneManager.LoadScene("SampleScene");
        root.Q<Button>("About").clicked += () => SceneManager.LoadScene("StatsMenu");
        root.Q<Button>("Quit").clicked += () => Application.Quit();
    }
}
