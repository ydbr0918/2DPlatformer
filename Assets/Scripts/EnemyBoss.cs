using System.Collections;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;

    private bool hasJumped = false; // 점프 중복 방지 변수
    private bool isGrounded = true; // 바닥에 닿았는지 확인하는 변수

    public Transform groundCheck;         // 땅 체크 위치
    public LayerMask groundLayer;         // 땅으로 인식할 레이어

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(EnemyBossPatternRoutine());
    }
    private void Update()
    {
        // GroundCheck 실행 (중복 제거용으로 Update에서만 처리)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // 점프가 끝났으면 리셋
        if (isGrounded && hasJumped)
        {
            hasJumped = false;
            Debug.Log("보스: 점프 종료, 점프 상태 리셋");
        }
    }

    private IEnumerator EnemyBossPatternRoutine()
    {
        while (true)
        {
            // ▶️ Walk 상태: 플레이어 추적
            animator.Play("Walk");

            float walkDuration = 2f;
            float timer = 0f;

            while (timer < walkDuration)
            {
                FollowPlayer();
                timer += Time.deltaTime;
                yield return null;
            }

            // ⏸️ 움직임 멈춤
            rb.velocity = Vector2.zero;

            // 🎲 랜덤 패턴 선택 (Jump, Sleep, Spin)
            int randomIndex = Random.Range(0, 3);
            switch (randomIndex)
            {
                case 0:
                    if (!hasJumped && isGrounded)  // 점프 중복 방지 및 바닥에 닿아 있을 때만
                    {
                        animator.SetTrigger("JumpTrigger");
                        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                        hasJumped = true;
                        Debug.Log("보스: 점프!");
                    }
                    break;
                case 1:
                    animator.SetTrigger("SleepTrigger");
                    Debug.Log("보스: 잠자는 중...");
                    break;
                case 2:
                    animator.SetTrigger("SpinTrigger");
                    Debug.Log("보스: 회전 공격!");
                    break;
            }

            yield return new WaitForSeconds(2f); // 애니메이션 실행 시간

            // 점프가 끝나면 다시 점프 가능하도록 설정
            hasJumped = false;
        }
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

            // 🔁 좌우 반전
            if (direction.x < 0 && !isFacingRight)
                Flip();
            else if (direction.x > 0 && isFacingRight)
                Flip();
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    // 바닥에 닿았는지 체크하는 함수 (보스를 바닥에 닿으면 점프 허용)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))  // Obstacle 태그로 바닥 체크
        {
            isGrounded = true;
            hasJumped = false; // 점프 가능 상태로 리셋
            Debug.Log("보스: 바닥에 닿음, 점프 가능");
        }
    }

    // 바닥을 벗어나면 점프가 안 되게 설정
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = false;  // 바닥에서 떨어짐
            Debug.Log("보스: 바닥에서 떨어짐, 점프 불가능");
        }
    }

    // 추가: 점프 애니메이션이 끝난 후 점프 상태를 리셋
   
}