using System.Collections;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private bool hasJumped = false;
    private bool isGrounded = false;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(EnemyBossPatternRoutine());
    }

    private void Update()
    {
        // 바닥 체크 수행
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // 점프 상태 리셋 조건
        if (isGrounded && hasJumped)
        {
            hasJumped = false;
            Debug.Log("보스: 착지 완료, 점프 가능");
        }
    }

    private IEnumerator EnemyBossPatternRoutine()
    {
        while (true)
        {
            // Walk 상태
            animator.Play("Walk");
            float walkDuration = 2f;
            float timer = 0f;

            while (timer < walkDuration)
            {
                FollowPlayer();
                timer += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;

            // 패턴 선택
            int randomIndex = Random.Range(0, 3);
            switch (randomIndex)
            {   
                case 0:
                    if (!hasJumped && isGrounded)
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

            yield return new WaitForSeconds(2f);
        }
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

            // 좌우 반전
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
}