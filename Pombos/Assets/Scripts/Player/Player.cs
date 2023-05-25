using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [Header("Player Health Settings")]
    [SerializeField] GameEvent onHPChange;
    [SerializeField] GameEvent onMaxHPChange;

    [SerializeField] int _maxHp = 100;
    public int maxHp {
        get { return _maxHp; }
        set{ _maxHp = value;
            onMaxHPChange.TriggerEvent();
        }}

    private int _hp = 100;
    public int hp {
        get { return _hp; }
        set { _hp = value;
            onHPChange.TriggerEvent();
        }}

    public int healthRegen = 0;

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
    [SerializeField] private int _baseAmmo = 0;
    [SerializeField] private float _fireRateReductionPct = 0;
    [SerializeField] private float _spreadReductionPct = 0;

    public Animator playerBodyAnimator;
    public Animator playerLegsAnimator;
    public int baseDamage
    {
        get { return _baseDamage; }
        set
        {
            _baseDamage = value;
            weaponsManager.UpdateWeaponsStats();
        }
    }
    public int baseAmmo
    {
        get { return _baseAmmo; }
        set
        {
            _baseAmmo = value;
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

    void Start()
    {
        onMaxHPChange.TriggerEvent();
        hp = maxHp;
        StartCoroutine("RegenHP");
    }


    public void Heal(int healAmount)
    {
        hp += healAmount;
        if (hp > maxHp) hp = maxHp;
        //UpdateHpText();
    }

    IEnumerator RegenHP()
    {
        while (hp > 0)
        {
            yield return new WaitForSeconds(1);
        }
    }

    public int ApplyArmor(int damage)
    {

        int damageMitigado = (int) Mathf.Floor(damage/((armor + 100f)/100f));
        if (damageMitigado <= 0) damageMitigado = 1;

        //Debug.Log("(" + damage + "/((" + armor + "+ 100)100)) = (" + damage + "/" + ((armor + 100f) / 100f));
        //Debug.Log("Dano inicial: " + damage + " - Dano mitigado: " + damageMitigado);

        return damageMitigado;
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
        damage = ApplyArmor(damage);

        hp -= damage;
        //Debug.Log(damage + " damage taken");

        if (hp <= 0)
        {
            isAlive = false;
            GameManager.instance.GameOver();
        }
        //UpdateHpText();
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
