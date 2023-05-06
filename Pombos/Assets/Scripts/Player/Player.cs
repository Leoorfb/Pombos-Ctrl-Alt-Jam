using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int maxHp = 100;
    public int hp = 100;
    public int armor = 0;
    public int healthRegen = 0;
    public int projectileSize = 1;
    public int projectileAmount = 1;
    public float attackCooldownPct = 1;
    public float speed = 5;

    public bool isAlive = true;

    [SerializeField] TextMeshProUGUI hpText;

    WeaponsManager weaponsManager;


    private void Awake()
    {
        weaponsManager = GetComponent<WeaponsManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateHpText();
        StartCoroutine("RegenHP");
    }

    void Heal(int healAmount)
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
            Heal(healthRegen);
        }
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
            //Debug.Log(name + " colidiu com " + collision.gameObject.name);
            TakeDamage(collided.GetComponent<Enemy>().Attack());
        }
        
    }

}
