using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [Header("Player Health Settings")]
    public int maxHp = 100;
    private int _hp = 100;
    public int hp {
        get { return _hp; }
        set { _hp = value;
            UpdateHpText();
        }}
    public int healthRegen = 0;

    [SerializeField] TextMeshProUGUI hpText;

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
        hp = maxHp;
        UpdateHpText();
        StartCoroutine("RegenHP");
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

    void UpdateHpText()
    {
        hpText.text = "Player  HP: " + hp + "/" + maxHp;
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
    }

    public void TakeDamage(int damage)
    {
        ApplyArmor(ref damage);

        hp -= damage;
        //Debug.Log(damage + " damage taken");

        if (hp <= 0)
        {
            isAlive = false;
        }
        UpdateHpText();
        return;
    }

    public void TakeKnockback(Vector2 direction, float force)
    {
        //Debug.Log(direction * force);
        _Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
