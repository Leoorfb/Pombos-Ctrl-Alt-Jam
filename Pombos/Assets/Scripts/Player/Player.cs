using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [Header("Player Health Settings")]
    [SerializeField] int _maxHp = 100;
    public int maxHp {
        get { return _maxHp; }
        set{ _maxHp = value; 
            UpdateHpMaxContainer();
        }}

    private int _hp = 100;
    public int hp {
        get { return _hp; }
        set { _hp = value;
            UpdateHpText();
        }}
    public int healthRegen = 0;

    private float healthContainerWidth;

    [SerializeField] TextMeshProUGUI hpText;
    public UIDocument hpTextUIDocument;
    public VisualTreeAsset healthTileTemplate;

    [Header("Player Stats Settings")]
    public int armor = 0;
    [SerializeField] private float _speed = 5000;
    public float speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
            playerMovement.baseSpeed = _speed;
            playerMovement.shootingSpeed = _speed * shootingSpeedSlowPct;
        }
    }
    public float shootingSpeedSlowPct = 0.7f;

    public float critChance = 0.01f;
    public int critModifier = 2;
    public int lucky = 1;
    public bool isAlive = true;

    [Header("Invulnerability Stats Settings")]
    public float invulnerabilityTime = .5f;
    [SerializeField] Color invulnerabilityColor;
    [SerializeField] Color normalColor;
    public bool isInvulnerable = false;
    [SerializeField] SpriteRenderer playerSpriteRenderer;

    [Header("Weapon Stats Settings")]
    [SerializeField] private int _baseDamage = 0;
    [SerializeField] private float _fireRateReductionPct = 0;
    [SerializeField] private float _spreadReductionPct = 0;
    public int baseDamage
    {
        get { return _baseDamage; }
        set
        {
            _baseDamage = value;
            weaponsManager.UpdateWeaponsStats();
        }
    }

    public float fireRateReductionPct
    {
        get { return _fireRateReductionPct; }
        set
        {
            _fireRateReductionPct = value;
            weaponsManager.UpdateWeaponsStats();
        }
    }
    public float spreadReductionPct
    {
        get { return _spreadReductionPct; }
        set
        {
            _spreadReductionPct = value;
            weaponsManager.UpdateWeaponsStats();
        }
    }

    Rigidbody2D _Rigidbody;
    WeaponsManager weaponsManager;
    PlayerMovement playerMovement;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        weaponsManager = GetComponent<WeaponsManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        VisualElement fillHealthContainer = hpTextUIDocument.rootVisualElement.Q<VisualElement>("FillHealthContainer");
        hp = maxHp;
        healthContainerWidth = fillHealthContainer.style.width.value.value;
        UpdateHpText();
        CreateHpTiles(maxHp);
        StartCoroutine("RegenHP");
        UpdateHpMaxContainer();
    }

    public void Heal(int healAmount)
    {
        hp += healAmount;
        if (hp > maxHp) hp = maxHp;
        UpdateHpText();
    }

    IEnumerator RegenHP()
    {
        while (hp > 0)
        {
            yield return new WaitForSeconds(1);
        }
    }

    // Talvez repensar funcionamento da armadura
    // assim caso a armadura for maior q o dano ela nega o dano por completo
    public void ApplyArmor(ref int damage)
    {
        damage -= armor;
        if (damage < 0) damage = 0;
    }
    
    private void ResetHpTiles() 
    {
        VisualElement fillHealthContainer = hpTextUIDocument.rootVisualElement.Q<VisualElement>("FillHealthContainer");
        foreach(VisualElement child in fillHealthContainer.Children())
        {
            child.style.display = DisplayStyle.None;
        }
    }

    private void FillHpTiles(int currentHp)
    {
        VisualElement fillHealthContainer = hpTextUIDocument.rootVisualElement.Q<VisualElement>("FillHealthContainer");
        int count = 0;

        foreach(VisualElement child in fillHealthContainer.Children())
        {
            if(count >= currentHp)
                break;
            child.style.display = DisplayStyle.Flex;
            count += 1;
        }
    }

    private void CreateHpTiles(int currentHealth) 
    {
        VisualElement fillHealthContainer = hpTextUIDocument.rootVisualElement.Q<VisualElement>("FillHealthContainer");
        for(int i = 0; i < currentHealth; i++) {
            TemplateContainer healthTileContainer = healthTileTemplate.Instantiate();
            fillHealthContainer.Add(healthTileContainer);
            fillHealthContainer.Children();
        }
    }


    private void CleanHpTiles()
    {
        VisualElement fillHealthContainer = hpTextUIDocument.rootVisualElement.Q<VisualElement>("FillHealthContainer");
        fillHealthContainer.Clear();
    }

    void UpdateHpMaxContainer()
    {
        VisualElement fillHealthContainer = hpTextUIDocument.rootVisualElement.Q<VisualElement>("FillHealthContainer");
        VisualElement tileHealthContainer = hpTextUIDocument.rootVisualElement.Q<VisualElement>("RootHealthTile");
        float tileWidth = 20;
        fillHealthContainer.style.width = new Length(healthContainerWidth + (tileWidth * (maxHp + 0.5f )));  
        CleanHpTiles();
        CreateHpTiles(maxHp);
    }

    void UpdateHpText()
    {
        hpText.text = "Player  HP: " + hp + "/" + maxHp;
        Label hpTextLabel = hpTextUIDocument.rootVisualElement.Q<Label>("HpCounterLabel");
        hpTextLabel.text = hp.ToString("#00");

        this.ResetHpTiles();
        this.FillHpTiles(hp);
    }

    internal bool RollCrit()
    {
        return (UnityEngine.Random.Range(0f,1f) <= critChance);
    }

    public void GetHit(int damage)
    {
        TakeDamage(damage);
    }
    public void GetHit(int damage, Vector2 direction, float force)
    {
        if (damage < 0) return;

        GetHit(damage);
        TakeKnockback(direction, force);
        StartCoroutine("GetInvulnerable");
    }

    public void TakeDamage(int damage)
    {
        ApplyArmor(ref damage);

        hp -= damage;
        //Debug.Log(damage + " damage taken");

        if (hp <= 0)
        {
            isAlive = false;
            GameManager.instance.GameOver();
        }
        UpdateHpText();
        return;
    }

    public void TakeKnockback(Vector2 direction, float force)
    {
        //Debug.Log(direction * force);
        _Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public IEnumerator GetInvulnerable()
    {
        isInvulnerable = true;
        playerSpriteRenderer.color = invulnerabilityColor;

        yield return new WaitForSeconds(invulnerabilityTime);

        isInvulnerable = false;
        playerSpriteRenderer.color = normalColor;

    }
}
