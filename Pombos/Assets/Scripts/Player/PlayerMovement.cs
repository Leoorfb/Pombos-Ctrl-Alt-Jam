using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    public float speed = 5;
    public float baseSpeed = 5;
    public float shootingSpeed = 5;

    Rigidbody2D _Rigidbody;


    [SerializeField] Camera mainCamera;
    Vector3 mouse_pos;
    Vector3 object_pos;
    float angle;
    Vector3 newRotation = Vector3.zero;
    Vector2 moveDirection = Vector2.zero;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GetComponent<Player>();
        baseSpeed = player.speed;
        shootingSpeed = player.speed * player.shootingSpeedSlowPct;
    }

    void Update()
    {
        if (!player.isAlive) return; // remover depois q a função de morrer estiver pronta

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        moveDirection.x = horizontal;
        moveDirection.y = vertical;
        moveDirection = moveDirection.normalized;
            
        LookAtMouse();

        if (Input.GetMouseButton(0))
        {
            speed = shootingSpeed;
        }
        else
        {
            speed = baseSpeed;
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(moveDirection * speed * Time.fixedDeltaTime);

        _Rigidbody.AddForce(moveDirection * speed * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    void LookAtMouse()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        object_pos = mainCamera.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        newRotation.z = angle;
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
