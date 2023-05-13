using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


using UnityEngine;
public class PauseManager : MonoBehaviour
{   
    private bool gameIsPaused = false;

    private Player player;
    
    public UIDocument uiDocument;

    public void Start() 
    {
        player = GameManager.instance.playerTransform.GetComponent<Player>(); 
    }
    private void ShowMenu() 
    {

        uiDocument.gameObject.SetActive(true);
        this.PauseGame();
        this.gameIsPaused = true;

        WeaponBase weapon = player.GetComponent<WeaponsManager>().GetWeaponBase();
        int playerGold = player.GetComponent<PlayerGold>().gold;


        Label armorLabelUI = uiDocument.rootVisualElement.Q<Label>("ArmorLabel");
        armorLabelUI.text = armorLabelUI.text.Replace("XX", player.armor.ToString());

        Label healthLabelUI = uiDocument.rootVisualElement.Q<Label>("HealthLabel");
        healthLabelUI.text = healthLabelUI.text.Replace("XX", player.hp.ToString() + "/"  + player.maxHp.ToString());

        Label damageLabelUI = uiDocument.rootVisualElement.Q<Label>("DamageLabel");
        damageLabelUI.text = damageLabelUI.text.Replace("XX", weapon.weaponStats.damage.ToString()); 

        Label coinLabelUI = uiDocument.rootVisualElement.Q<Label>("CoinLabel");
        coinLabelUI.text = coinLabelUI.text.Replace("XX", playerGold.ToString());

        Label speedLabelUI = uiDocument.rootVisualElement.Q<Label>("SpeedLabel");
        speedLabelUI.text = speedLabelUI.text.Replace("XX", player.speed.ToString());

        Label shootingSpeedLabelUI = uiDocument.rootVisualElement.Q<Label>("ShootingSpeedLabel");
        shootingSpeedLabelUI.text = shootingSpeedLabelUI.text.Replace("XX", weapon.weaponStats.fireRate.ToString());

        float normalizedCriticalValue = player.critChance * 100;

        Label criticalLabelUI = uiDocument.rootVisualElement.Q<Label>("CriticalLabel");
        criticalLabelUI.text = criticalLabelUI.text.Replace("XX", normalizedCriticalValue.ToString() + "%");

        Label ammoMaxLabelUI = uiDocument.rootVisualElement.Q<Label>("AmmoMaxLabel");
        ammoMaxLabelUI.text = ammoMaxLabelUI.text.Replace("XX", weapon.weaponStats.ammoMax.ToString());

        Button closeStatsBtn = uiDocument.rootVisualElement.Q<Button>("CloseStatsBtn");
        closeStatsBtn.clicked += () => this.HideMenu();

        Button quitBtn = uiDocument.rootVisualElement.Q<Button>("QuitBtn");
        quitBtn.clicked += () => {
            this.HideMenu();
            SceneManager.LoadScene("MainMenu");
        };
    }

    private void HideMenu() 
    {
        uiDocument.gameObject.SetActive(false);
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

    public void Update() 
    {
        if(Input.GetKeyDown("escape")) {
            if(this.gameIsPaused) {
                this.HideMenu();
                return;
            } 
            this.ShowMenu();
        }
    }
}
