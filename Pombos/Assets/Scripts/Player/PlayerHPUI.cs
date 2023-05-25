using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{

    [SerializeField] RectTransform hpBar;
    [SerializeField] RectTransform hpTileStart;
    [SerializeField] RectTransform hpTile;
    [SerializeField] TextMeshProUGUI hpTileText;

    [SerializeField] float healthContainerWidth;
    [SerializeField] float healthTileWidth;
    [SerializeField] string healthTileName;

    Player player;

    private void Start()
    {
        player = GameManager.instance.playerTransform.GetComponent<Player>();
        SetUpHpUi();
    }

    private void SetUpHpUi()
    {
        healthContainerWidth = hpBar.sizeDelta.x;
        healthTileWidth = hpTile.sizeDelta.x;
    }

    public void UpdateHpUi()
    {
        //Debug.Log("update hp");
        hpTileText.text = player.hp.ToString("#00");

        this.ResetHpTiles();
        this.FillHpTiles(player.hp);
    }

    private void CreateHpTiles(int currentHealth)
    {

        for (int i = 0; i < currentHealth; i++)
        {
            RectTransform healthTile = Instantiate(hpTile, hpBar);
            healthTile.position = hpTileStart.position;
            healthTile.Translate(Vector2.right * healthTileWidth * i /2);
            healthTile.gameObject.SetActive(true);
        }
        healthTileName = hpBar.GetChild(hpBar.childCount-1).name;
    }
    private void ResetHpTiles()
    {
        //Debug.Log("reset hp tile");
        foreach (RectTransform child in hpBar)
        {
            if (child.name != healthTileName) continue;
            child.gameObject.SetActive(false);
        }
    }

    private void FillHpTiles(int currentHp)
    {
        //Debug.Log("fill hp tiles");
        int count = 0;

        foreach (RectTransform child in hpBar)
        {
            if (child.name != healthTileName) continue;

            if (count >= currentHp)
                break;

            child.gameObject.SetActive(true);
            count++;
        }
    }

    private void CleanHpTiles()
    {
        foreach (RectTransform child in hpBar)
        {
            if (child.name != healthTileName) continue;
            Destroy(child.gameObject);
        }
    }


    public void UpdateHpMaxContainer()
    {
        hpBar.sizeDelta = new Vector2 (healthContainerWidth + (healthTileWidth * (player.maxHp - 0.5f)), hpBar.sizeDelta.y);
        CleanHpTiles();
        CreateHpTiles(player.maxHp);
    }
}
