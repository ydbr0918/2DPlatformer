using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSkill : MonoBehaviour
{
    public float maxTravelDistance = 10f; // 날아갈 수 있는 최대 거리
    public float speed = 5f;              // 스킬 이동 속도

    private Vector3 startPos;
    private Vector2 direction;            // 이동 방향

    private void Start()
    {
        startPos = transform.position;

        // 방향 설정이 제대로 되어 있는지 체크
        Debug.Log("스킬 생성 위치: " + startPos);
        Debug.Log("스킬 이동 방향: " + direction);
    }

    private void Update()
    {
        // 지정된 방향으로 이동
        if (direction != Vector2.zero)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        // 일정 거리 이상 이동하면 스킬 제거
        float distance = Vector3.Distance(startPos, transform.position);
        if (distance > maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && !player.IsInvincible())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 리스폰 처리
            }
        }
    }

    // 방향 설정 함수
    public void SetDirection(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            direction = dir.normalized; // 방향 설정
            Debug.Log("방향 설정: " + direction); // 방향이 제대로 설정됐는지 로그로 확인
        }
    }
}
