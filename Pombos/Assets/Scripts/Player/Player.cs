using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int maxHp = 100;
    private int _hp = 100;
    public int hp {
        get { return _hp; }
        set { _hp = value;
            UpdateHpText();
        }}

    public int armor = 0;
    public float attackCooldownPct = 1;
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

    public int lucky = 1;


    public bool isAlive = true;

    [SerializeField] TextMeshProUGUI hpText;

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collided = collision.gameObject;
        //Debug.Log("COLIDIU " + collided.tag);
        if (collided.tag == "Enemy")
        {
            Enemy enemy = collided.GetComponent<Enemy>();
            //Debug.Log(name + " colidiu com " + collision.gameObject.name);
            Vector2 dir = new Vector2(transform.position.x - collision.transform.position.x, transform.position.y - collision.transform.position.y);
            GetHit(enemy.Attack(), dir, enemy.knockbackStreght);
        }
        
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
