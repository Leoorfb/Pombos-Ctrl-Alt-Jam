using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifespan;
    public Enemy enemy;
    public Vector2 direction = Vector2.zero;


    private void Start()
    {
        StartCoroutine("AttackDuration");
    }

    private void OnEnable()
    {
        StartCoroutine("AttackDuration");
        //direction.x = ;
    }

    protected virtual void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Projetil ENTROU GATILHO");

        if (collision.gameObject.tag == "Player")
        {
            enemy.HitPlayer();
            AudioManager.instance.Play("HitPlayer");
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            AudioManager.instance.Play("HitWall");
            Destroy(gameObject);
        }
    }

    IEnumerator AttackDuration()
    {
        //Debug.Log("Projetil terminou lifespan");
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }
}
