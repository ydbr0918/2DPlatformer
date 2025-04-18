using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTraceController : MonoBehaviour
{
    public float movespeed = .8f;
    public float raycastDistance = .2f;
    public float traceDistance = 2f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;

        if (direction.magnitude > traceDistance)
            return;

        Vector2 directionNormalized = direction.normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionNormalized, raycastDistance);
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        foreach (RaycastHit2D rHit in hits)
        {
            if (rHit.collider != null && rHit.collider.CompareTag("Obstacle"))
            {
                Vector3 alternativeDirection = Quaternion.Euler(0f, 0f, -90f) * direction;
                transform.Translate(alternativeDirection * movespeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction * movespeed * Time.deltaTime);
            }

        }


        {




            // 좌우 방향에 따라 몬스터 스프라이트 반전
            if (direction.x != 0)
            {
                Vector3 scale = transform.localScale;

                // 오른쪽을 보면 localScale.x를 양수, 왼쪽을 보면 음수
                scale.x = direction.x < 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);

                // 반전된 크기를 적용
                transform.localScale = scale;
            }


        }

























    }
}