using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (HealthComponent))]
public class Player : MonoBehaviour
{
    
    private HealthComponent playerHealthComponent;
    private CharacterController playerCharacterController;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Camera mainCamera;
    
    private Vector3 moveDirection;
    private bool playerFired = false;


    // Start is called before the first frame update
    void Start()
    {
        playerHealthComponent = GetComponent<HealthComponent>();
        playerHealthComponent.HandleHealthUpdated += OnHealthUpdated;
        playerHealthComponent.HandleOnKilled += OnKilled;
        playerCharacterController = GetComponent<CharacterController>();
        playerHealthBar.maxValue = playerHealthComponent.Health;
    }

    private void FixedUpdate()
    {
        Movement();
        if (playerFired)
        {
            
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        
    }

    void CheckHit()
    {

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
