using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

using UnityEngine;
public class PauseManager : MonoBehaviour
{   
    private bool gameIsPaused = false;
     private void Start() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;
    }

    public void PauseGame()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Time.timeScale = 0;
        root.style.display = DisplayStyle.Flex;
        this.gameIsPaused = true;
    }

    public void UnPauseGame()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Time.timeScale = 1;
        root.style.display = DisplayStyle.None;
        this.gameIsPaused =  false;
    }

    public void Update() {
        if(Input.GetKeyDown("escape")) {
            if(gameIsPaused) {
                UnPauseGame();
                return;
            } 
            PauseGame();
        }
    }
}
