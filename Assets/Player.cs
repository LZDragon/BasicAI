using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (HealthComponent))]
public class Player : MonoBehaviour
{
    
    private HealthComponent playerHealthComponent;
    private CharacterController playerCharacterController;
    [SerializeField] private Slider playerHealthBar;
    
    private Vector3 moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        playerHealthComponent = GetComponent<HealthComponent>();
        playerHealthComponent.HandleHealthUpdated += OnHealthUpdated;
        playerCharacterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
    }

    void Movement()
    {
        float speed = 5f;
        float gravity = -9.8f;
        Vector3 movementAmount = (moveDirection * (speed * Time.deltaTime)) + new Vector3(0, gravity);
        playerCharacterController.Move(movementAmount);
    }

    void OnHealthUpdated(float updatedHealth)
    {
        playerHealthBar.value = updatedHealth;
    }

    void OnKilled()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        playerHealthComponent.TakeDamage(10);
        Debug.Log(playerHealthComponent.Health);
    }
}
