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




            // �¿� ���⿡ ���� ���� ��������Ʈ ����
            if (direction.x != 0)
            {
                Vector3 scale = transform.localScale;

                // �������� ���� localScale.x�� ���, ������ ���� ����
                scale.x = direction.x < 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);

                // ������ ũ�⸦ ����
                transform.localScale = scale;
            }


        }

























    }
}