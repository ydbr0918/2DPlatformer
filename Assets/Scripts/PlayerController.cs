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

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator pAni;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();

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


        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }
        if (collision.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

