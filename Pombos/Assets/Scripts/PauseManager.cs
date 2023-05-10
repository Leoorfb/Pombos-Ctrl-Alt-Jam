using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

using UnityEngine;
public class PauseManager : MonoBehaviour
{   
    private bool gameIsPaused = false;
     private void Start() {
        
    }

    public void PauseGame()
    {
        
        Time.timeScale = 0;
        
        this.gameIsPaused = true;
    }

    public void UnPauseGame()
    {
        
        Time.timeScale = 1;
        
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
