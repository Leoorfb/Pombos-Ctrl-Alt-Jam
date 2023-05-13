using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;


public class AmmoIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoText;
    public UIDocument ammoCountUIDocument;
    public VisualTreeAsset bulletTile;

    private float ammoContainerWidth;
    private int maxAmmo = 30;


    void Start() {
        VisualElement ammoContainer = ammoCountUIDocument.rootVisualElement.Q<VisualElement>("BulletTileContainer");
        CreateAmmoTiles(30);
        ammoContainerWidth = ammoContainer.style.width.value.value;
        UpdateMaxAmmoContainer();
    }


    void UpdateMaxAmmoContainer() 
    {
        VisualElement ammoContainer = ammoCountUIDocument.rootVisualElement.Q<VisualElement>("BulletTileContainer");
        float tileWidth = 10;
        ammoContainer.style.width = new Length(ammoContainerWidth + (tileWidth * (maxAmmo + 0.3f)));
    }

    private void ResetBulletTiles()
    {
        VisualElement ammoContainer = ammoCountUIDocument.rootVisualElement.Q<VisualElement>("BulletTileContainer");
        foreach(VisualElement child in ammoContainer.Children()) {
            child.style.display = DisplayStyle.None;
        }
    }

    private void FillBulletTiles(int currentAmmo)
    {
        VisualElement ammoContainer = ammoCountUIDocument.rootVisualElement.Q<VisualElement>("BulletTileContainer");
        int count = 0;

        foreach(VisualElement child in ammoContainer.Children())
        {
            if(count >= currentAmmo)
                break;
            child.style.display = DisplayStyle.Flex;
            count += 1;
        }
    }

    private void CreateAmmoTiles(int maxAmmo)
    {
        VisualElement ammoContainer = ammoCountUIDocument.rootVisualElement.Q<VisualElement>("BulletTileContainer");
        for(int i = 0; i < maxAmmo; i++) 
        {
            TemplateContainer bulletTileItem = bulletTile.Instantiate();
            ammoContainer.Add(bulletTileItem);
    
        }
    }

    private void CleanAmmoTiles()
    {
        VisualElement ammoContainer = ammoCountUIDocument.rootVisualElement.Q<VisualElement>("BulletTileContainer");
        ammoContainer.Clear();
    }
    public void SetAmmo(int currentAmmo, int maxAmmo)
    {
        ammoText.text ="Ammo: "+ currentAmmo.ToString("#000") + "/" + maxAmmo.ToString("#000");
        
        if(this.maxAmmo != maxAmmo) {
            this.maxAmmo = maxAmmo;
            UpdateMaxAmmoContainer();
            CleanAmmoTiles();
            CreateAmmoTiles(maxAmmo);
        }

        ResetBulletTiles();
        FillBulletTiles(currentAmmo);
    }
}
