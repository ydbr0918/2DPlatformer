using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyBoss : MonoBehaviour
{
    [Header("스킬 오브젝트 프리팹")]
    public GameObject skillObjectPrefab;
    public Transform skillSpawnPoint;

    public Transform player;
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float jumpForce = 10f;

    public int pastAction = -1;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private bool hasJumped = false;

    public Transform groundCheck;
    public LayerMask groundLayer;

    // HP 관련 변수
    public int maxHP = 100;
    private int currentHP;
    public Slider bossHpSlider;

    private enum BossState { Walk, Action }
    private BossState currentState = BossState.Walk;

    private float stateTimer = 0f;
    private float stateDuration = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;

        if (bossHpSlider != null)
        {
            bossHpSlider.maxValue = maxHP;
            bossHpSlider.value = currentHP;
        }
    }

    private void Start()
    {
        stateTimer = Time.time + stateDuration;
    }

    private void Update()
    {
        if (hasJumped) // 착지했을 때 hasJumped를 false로 설정
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

    private void ChooseRandomAction()
    {
        pastAction = Random.Range(0, 3);

        Debug.Log("선택된 보스 패턴: " + pastAction);

        switch (pastAction)
        {
            case 0: // Jump
                if (!hasJumped) //hasJumped가 true일 때 점프 실행
                {
                    animator.SetTrigger("JumpTrigger");
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 점프 힘 추가
                    hasJumped = true; // 점프 상태로 변경
                    Debug.Log("보스: 점프!");
                }
                break;

            case 1: // Sleep
                animator.SetTrigger("SleepTrigger");
                Debug.Log("보스: 잠자는 중...");
                SpawnSkillObject(); // 단일 방향 스킬
                break;

            case 2: // Spin
                animator.SetTrigger("SpinTrigger");
                Debug.Log("보스: 회전 공격!");
                SpawnSkillInFiveDirections(); // 다방향 스킬
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

            if (direction.x < 0 && isFacingRight)
                Flip();
            else if (direction.x > 0 && !isFacingRight)
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

    private void SpawnSkillObject()
    {
        if (skillObjectPrefab != null && skillSpawnPoint != null)
        {
            GameObject skill = Instantiate(skillObjectPrefab, skillSpawnPoint.position, Quaternion.identity);

            // BossSkill 컴포넌트 가져오기
            BossSkill skillScript = skill.GetComponent<BossSkill>();
            if (skillScript != null)
            {
                // 방향 설정
                Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
                skillScript.SetDirection(direction);
            }

            // 스킬의 회전 설정 (필요시)
            Vector3 scale = skill.transform.localScale;
            scale.x = isFacingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            skill.transform.localScale = scale;
        }
    }

    // ⭐ Spin 패턴 전용 - 5방향 스킬 발사
    private void SpawnSkillInFiveDirections()
    {
        Vector2[] directions = new Vector2[]
        {
            //Vector2.right,
            Vector2.left,
            Vector2.up,
            new Vector2(1, 1),
            //new Vector2(-1, 1)
        };

        foreach (Vector2 dir in directions)
        {
            GameObject skill = Instantiate(skillObjectPrefab, skillSpawnPoint.position, Quaternion.identity);

            // BossSkill 스크립트를 통해 방향 설정
            BossSkill skillScript = skill.GetComponent<BossSkill>();
            if (skillScript != null)
            {
                skillScript.SetDirection(dir);  // 방향 설정
            }

            // 회전 설정
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            skill.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        if (bossHpSlider != null)
            bossHpSlider.value = currentHP;

        Debug.Log("보스 남은 HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("보스 사망");
        Destroy(gameObject);
    }
}

