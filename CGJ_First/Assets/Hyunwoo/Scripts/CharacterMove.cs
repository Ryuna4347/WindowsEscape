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

    [SerializeField]private Rigidbody2D collidingBoxRb;
    [SerializeField] private int boxDirection = 0; //현재 충돌중인 박스가 어느 방향에 있는가? -1 왼쪽, 0 없음, 1 오른쪽
    private bool isHoldingBox = false;

    private float moveDirection = 0;
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
            moveDirection = 0;

            if (Input.GetKey(KeyCode.LeftArrow))
            { 
                moveDirection += -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDirection += 1f;
            }

            if(!isHoldingBox && Input.GetKey(KeyCode.LeftShift))
            {
                RaycastBox();
            }
            if(Input.GetKeyUp(KeyCode.LeftShift) || moveDirection == boxDirection)
            {
                UnlinkToBox();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && canUsePortal)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
            animator.SetFloat("movingSpeed", moveDirection);
            if (moveDirection < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (moveDirection > 0)
            {
                spriteRenderer.flipX = false;
            }

            if (collidingBoxRb != null)
            {
                collidingBoxRb.velocity = rb.velocity;
            }
            if ((Input.GetKeyDown(KeyCode.Space) && !isJumping) && !isHoldingBox)
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

    private void UnlinkToBox()
    {
        if (collidingBoxRb != null)
        {
            isHoldingBox = false;
            collidingBoxRb.velocity = new Vector2(0, 0);
            collidingBoxRb.isKinematic = false;
            collidingBoxRb = null;
            boxDirection = 0;

            moveSpeed = 4.0f;
        }
    }
    
    private void RaycastBox() {
        RaycastHit2D hit;
        int direction = spriteRenderer.flipX ? -1 : 1;

        hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 1f, 1 << LayerMask.NameToLayer("Box"));
        
        if (hit.collider != null)
        {
            collidingBoxRb = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            collidingBoxRb.isKinematic = true;
            isHoldingBox = true;
            boxDirection = direction;

            moveSpeed = 2f;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isJumping && collision.gameObject.CompareTag("Map"))
        {
            isJumping = false;
            animator.SetBool("isJumping", isJumping);
        }
        if(collision.gameObject.CompareTag("Box"))
        {
            if (transform.position.y >= collision.gameObject.transform.position.y + 0.2f) //박스 위를 밟는 경우
            {
                isJumping = false;
                animator.SetBool("isJumping", isJumping);
            }
            else
            {
                collidingBoxRb = collision.gameObject.GetComponent<Rigidbody2D>();
                collidingBoxRb.isKinematic = true;
                boxDirection = spriteRenderer.flipX ? -1 : 1;

                moveSpeed = 2f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isHoldingBox && collision.gameObject.CompareTag("Box"))
        {
            UnlinkToBox();
        }
        if (collision.gameObject.CompareTag("Map"))
        {
            if (isHoldingBox)
            {
                UnlinkToBox();
            }
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
