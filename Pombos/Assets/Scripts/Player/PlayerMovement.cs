using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;

    [SerializeField] float speed = 5;

    Rigidbody2D _Rigidbody;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (!player.isAlive) return; // remover depois q a função de morrer estiver pronta

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            Vector2 direction = new Vector2(horizontal, vertical).normalized;
            //transform.rotation = Quaternion.LookRotation(direction);
            transform.Translate(direction * speed * Time.deltaTime);

            //_Rigidbody.AddForce(direction * speed * Time.deltaTime, ForceMode.VelocityChange);

            //_Rigidbody.velocity = direction * speed; //* Time.deltaTime;
        }
    }
}
