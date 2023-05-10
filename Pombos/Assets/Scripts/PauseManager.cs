using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

using UnityEngine;
public class PauseManager : MonoBehaviour
{   
    private bool gameIsPaused = false;
    public UIDocument uIDocument;

    private void ShowMenu() {
        uIDocument.gameObject.SetActive(true);
        this.PauseGame();
        this.gameIsPaused = true;
    }

    private void HideMenu() {
        uIDocument.gameObject.SetActive(false);
        this.UnPauseGame();
        this.gameIsPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }

    public void Update() {
        if(Input.GetKeyDown("escape")) {
            if(this.gameIsPaused) {
                this.HideMenu();
                return;
            } 
            this.ShowMenu();
        }
    }
}
