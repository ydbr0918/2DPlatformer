using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 1f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool isSpeedBoosted = false;
    private float originalSpeed;
    public float speedBoostAmount = 3f;
    public float speedBoostDuration = 3f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator pAni;

    // 🔹 무적 상태 관련 변수 추가
    private bool isInvincible = false;
    private float invincibleDuration = 5f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 시각 효과용

        originalSpeed = moveSpeed; // ← 원래 속도 기억
    }


    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput < 0)
            transform.localScale = new Vector3(-0.16f, 0.16f, 0.16f);

        if (moveInput > 0)
            transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("jumpAction");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 🔹 무적 상태 아닐 경우에만 리스폰 처리
        if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_trace"))
        {
            if (!isInvincible)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Debug.Log("무적 상태로 적과 충돌 - 리스폰 안함");
            }
        }

        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        // 🔹 무적 아이템 처리
        if (collision.CompareTag("InvincibilityItem"))
        {
            StartCoroutine(BecomeInvincible());
            Destroy(collision.gameObject); // 아이템 제거
        }

        if (collision.CompareTag("SpeedItem"))
        {
            Debug.Log("속도 증가 아이템 충돌");
            StartCoroutine(SpeedBoost());
            Destroy(collision.gameObject);
        }
    }

    // 🔹 무적 상태 실행 코루틴
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        Debug.Log("무적 상태 시작");

        // 시각 효과: 반투명하게 표시
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
        spriteRenderer.color = Color.white;
        Debug.Log("무적 상태 종료");
    }
    private IEnumerator SpeedBoost()
    {
        isSpeedBoosted = true;
        moveSpeed += speedBoostAmount;

        Debug.Log("속도 증가 시작");

        yield return new WaitForSeconds(speedBoostDuration);

        moveSpeed = originalSpeed;
        isSpeedBoosted = false;

        Debug.Log("속도 증가 종료");
    }
}

