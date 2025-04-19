using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firepoint;
    // 🔹 이동 및 점프 관련 변수
    public float moveSpeed = 5f;          // 플레이어 이동 속도
    public float jumpForce = 1f;          // 점프 힘
    public Transform groundCheck;         // 땅 체크 위치
    public LayerMask groundLayer;         // 땅으로 인식할 레이어

    // 🔹 속도 증가 관련 변수
    private bool isSpeedBoosted = false;
    private float originalSpeed;
    public float speedBoostAmount = 3f;
    public float speedBoostDuration = 3f;

    // 🔹 점프력 증가 관련 변수
    private bool isJumpBoosted = false;             // 점프 강화 상태 여부
    private float originalJumpForce;                // 원래 점프 힘 저장
    public float jumpBoostAmount = 5f;            // 점프 강화량
    public float jumpBoostDuration = 3f;            // 강화 지속 시간

    // 🔹 컴포넌트 참조
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator pAni;

    // 🔹 무적 상태 관련 변수
    private bool isInvincible = false;
    private float invincibleDuration = 5f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalSpeed = moveSpeed;
        originalJumpForce = jumpForce; // ← 원래 점프 힘도 저장
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Shoot();
        }
        // 🔹 이동 처리
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 🔹 방향 전환 (왼쪽/오른쪽)
        if (moveInput < 0)
            transform.localScale = new Vector3(-0.16f, 0.16f, 0.16f);
        if (moveInput > 0)
            transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);

        // 🔹 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // 🔹 점프
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("jumpAction");
        }
    }
    void Shoot()
    {
        Instantiate(bulletPrefab,firepoint.position,firepoint.rotation);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 🔹 적과 충돌
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

        // 🔹 낙사 등
        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // 🔹 레벨 완료
        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        // 🔹 무적 아이템 획득
        if (collision.CompareTag("InvincibilityItem"))
        {
            StartCoroutine(BecomeInvincible());
            Destroy(collision.gameObject);
        }

        // 🔹 속도 아이템 획득
        if (collision.CompareTag("SpeedItem"))
        {
            Debug.Log("속도 증가 아이템 충돌");
            StartCoroutine(SpeedBoost());
            Destroy(collision.gameObject);
        }

        // 🔹 점프력 아이템 획득
        if (collision.CompareTag("JumpItem"))
        {
            Debug.Log("점프력 증가 아이템 충돌");
            StartCoroutine(JumpBoost());
            Destroy(collision.gameObject);
        }

    }

    // 🔹 무적 코루틴
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        Debug.Log("무적 상태 시작");

        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
        spriteRenderer.color = Color.white;
        Debug.Log("무적 상태 종료");
    }

    // 🔹 속도 증가 코루틴
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

    // 🔹 점프력 증가 코루틴
    private IEnumerator JumpBoost()
    {
        isJumpBoosted = true;
        jumpForce += jumpBoostAmount;

        Debug.Log("점프력 증가 시작");

        yield return new WaitForSeconds(jumpBoostDuration);

        jumpForce = originalJumpForce;
        isJumpBoosted = false;

        Debug.Log("점프력 증가 종료");
    }
}