using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool canUsePortal = false;
    private bool isDead = false;
    public float moveSpeed;
    public float jumpSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            float speed = 0;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                speed += -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                speed += 1f;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(CharacterDead());
            }

            rb.velocity = new Vector2(speed * moveSpeed, rb.velocity.y);
            animator.SetFloat("movingSpeed", speed);
            if (speed < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (speed > 0)
            {
                spriteRenderer.flipX = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                isJumping = true;
                animator.SetBool("isJumping", isJumping);
            }
        }
    }

    public IEnumerator CharacterDead()
    {
        float disappearTime = 1.5f;
        float time = 0;

        Color alpha = spriteRenderer.color;

        isDead = true;
        while (time < disappearTime)
        {
            alpha.a = 1 - time / disappearTime;
            spriteRenderer.color = alpha;

            yield return null;
            time += Time.deltaTime;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isJumping && collision.gameObject.CompareTag("Map"))
        {
            isJumping = false;
            animator.SetBool("isJumping", isJumping);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Portal"))
        {
            canUsePortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Portal"))
        {
            canUsePortal = false;
        }
    }

}
