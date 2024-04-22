using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
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
            CheckHit();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        playerFired = context.performed;
    }

    private static bool ShouldDebug = false;
    private static float DebugDuration = 2.0f;
    private bool RayCastWithDebug(Vector3 Start, Vector3 Dir, out RaycastHit HitData, float Distance, int LayerMask)
    {
        bool WasHit = Physics.Raycast(Start, Dir, out HitData, Distance, LayerMask);
        if (ShouldDebug)
        {
            Color HitColor = Color.green;
            Color MissedColor = Color.red;
            Vector3 EndPoint = (Dir * Distance) + Start;
            
            if (WasHit)
            {
                Vector3 HitPoint = HitData.point;
                Debug.DrawLine(Start, HitPoint, HitColor, DebugDuration);
                Debug.DrawLine(HitPoint, EndPoint, MissedColor, DebugDuration);
            }
            else
            {
                Debug.DrawLine(Start, EndPoint, MissedColor, DebugDuration);
            }
            Debug.DrawRay(Start, Dir);
        }

        return WasHit;
    }
    

    void CheckHit()
    {
        Plane clickPlane = new Plane(Vector3.up, -0.5f);
        float distance;
        Vector3 mouseWorldPosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (clickPlane.Raycast(ray, out distance))
        {
            mouseWorldPosition = ray.GetPoint(distance);
            Vector3 heightOffset = new Vector3(mouseWorldPosition.x, 0, mouseWorldPosition.z);
            Vector3 shotDirection = heightOffset - transform.position;

            if (RayCastWithDebug(transform.position, shotDirection, out RaycastHit hit, 5f, 1<<6))
            {
                if (hit.transform.TryGetComponent(out Enemy enemyComponent))
                {
                    hit.transform.GetComponent<HealthComponent>().TakeDamage(20);
                }
            }
            else
            {
                if (Physics.SphereCast(transform.position , 1.5f, shotDirection, out RaycastHit radiusHit, 5f, 1<<7,
                        QueryTriggerInteraction.Collide))
                {
                    radiusHit.transform.GetComponent<HealthComponent>().TakeDamage(20f);
                }
            }
        }
        
        
        
        playerFired = false;
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
        SceneManager.LoadScene(3);
    }


    private void OnTriggerEnter(Collider other)
    {
        playerHealthComponent.TakeDamage(10);
        Debug.Log(playerHealthComponent.Health);
    }
}
