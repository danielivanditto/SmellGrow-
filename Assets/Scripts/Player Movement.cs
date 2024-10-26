using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;


public class PlayerMovement : MonoBehaviour
{
    [Header("Player Values and Classes")]
    public PlayerAttributes attributes = new PlayerAttributes();
    private float currentMultiJumpCount;

    [Header("Timers")]
    [SerializeField] private float resetJumpsTimer;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerStamina playerStamina;

    [Space] [Header("Colliders")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private void Start()
    {
        attributes.SetStartValues();
    }

    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(xAxis * attributes.speed, Mathf.Clamp(rb.linearVelocityY, -10, float.MaxValue));

        resetJumpsTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && currentMultiJumpCount < attributes.multiJumpCount && playerStamina.CheckForAvailablePoints())
        {
            rb.linearVelocityY = attributes.jumpPower;
            resetJumpsTimer = attributes.jumpPower / 10;
            playerStamina.RemoveStaminaPoint(1, true);
            playerStamina.StopAllCoroutines();
            currentMultiJumpCount += 1;
        }

        if (IsGrounded() && currentMultiJumpCount > 0 && resetJumpsTimer < 0)
        {
            currentMultiJumpCount = 0;
            resetJumpsTimer = attributes.jumpPower / 10;
            playerStamina.StopAllCoroutines();
            playerStamina.StartCoroutine(playerStamina.RegainStamina());
        }
    }

    public bool IsGrounded()
    {
        Collider2D hitCheck = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return hitCheck;
    }
}

public class PlayerAttributes
{
    // Player
    public float speed;
    public float jumpPower;
    public int multiJumpCount;

    // Stamina
    public int maxStaminaCount;
    public float staminaTimer;

    public void SetStartValues()
    {
        speed = 5;
        jumpPower = 5;
        multiJumpCount = 2;

        maxStaminaCount = 5;
        staminaTimer = 0.5f;
    }
}