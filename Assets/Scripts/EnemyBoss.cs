using System.Collections;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float jumpForce = 10f;

    public int pastAction = -1;              //보스의 이전 패턴을 확인하는 변수



    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private bool hasJumped = false;
    private bool isGrounded = false;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private enum BossState { Walk, Action }
    private BossState currentState = BossState.Walk;

    private float stateTimer = 0f;
    private float stateDuration = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateTimer = Time.time + stateDuration;
    }

    private void Update()
    {
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && hasJumped)
        {
            hasJumped = false;
            Debug.Log("보스: 착지 완료");
        }

        switch (currentState)
        {
            case BossState.Walk:
                animator.Play("Walk");
                FollowPlayer();

                if (Time.time >= stateTimer)
                {
                    rb.velocity = Vector2.zero;
                    currentState = BossState.Action;
                    stateTimer = Time.time + 2f;
                    ChooseRandomAction();
                }
                break;

            case BossState.Action:
                if (Time.time >= stateTimer)
                {
                    currentState = BossState.Walk;
                    stateTimer = Time.time + 2f;
                }
                break;
        }
    }

    /*private void ChooseRandomAction()
    {
        int randomIndex = Random.Range(0, 3);
        Debug.Log("선택된 보스 패턴: " + randomIndex);

        switch (randomIndex)
        {
            case 0: // Jump
                if (pastAction != 0)
                {
                    animator.SetTrigger("JumpTrigger");
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    hasJumped = true;
                    Debug.Log("보스: 점프!");
                    pastAction = 0;
                }
                break;

            case 1: // Sleep
                if (pastAction != 1)
                {
                    animator.SetTrigger("SleepTrigger");
                    Debug.Log("보스: 잠자는 중...");
                    pastAction = 1;
                    
                }
                break;
               

            case 2: // Spin
                if (pastAction != 2)
                {
                    animator.SetTrigger("SpinTrigger");
                    Debug.Log("보스: 회전 공격!");
                    pastAction = 2;
                }
                break;
        }
    }
    */

    private void ChooseRandomAction()
    {
        int newAction;

        // 이전과 같은 패턴이 나올 경우 재시도 (무한 루프 방지)
        do
        {
            newAction = Random.Range(0, 3);
        } while (newAction == pastAction && Time.frameCount % 10 != 0); // 한정 조건으로 루프 빠짐 방지

        pastAction = newAction;

        Debug.Log("선택된 보스 패턴: " + newAction);

        switch (newAction)
        {
            case 0: // Jump
                if (!hasJumped && isGrounded)
                {
                    animator.SetTrigger("JumpTrigger");
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    hasJumped = true;
                    Debug.Log("보스: 점프!");
                }
               
                break;

            case 1: // Sleep
                animator.SetTrigger("SleepTrigger");
                Debug.Log("보스: 잠자는 중...");
                break;

            case 2: // Spin
                animator.SetTrigger("SpinTrigger");
                Debug.Log("보스: 회전 공격!");
                break;
        }
    }



    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

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