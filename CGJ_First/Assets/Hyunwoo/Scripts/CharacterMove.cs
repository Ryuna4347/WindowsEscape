using KZLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int jumpCount = 0;

    [SerializeField]private FixedJoint2D fixedJoint2D; //연결되어있는 상자의 조인트
    [SerializeField] private int boxDirection = 0; //현재 충돌중인 박스가 어느 방향에 있는가? -1 왼쪽, 0 없음, 1 오른쪽
    [SerializeField]private bool isHoldingBox = false;

    private Transform landingParticle;
    private Transform runningParticle;

    private float moveDirection = 0;
    private bool isDead = false;
    public float moveSpeed;
    public float jumpSpeed;

    [SerializeField]private int blockDirection = 0;
    [SerializeField]private bool verticalTrigger = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        landingParticle = transform.Find("LandingParticle");
        runningParticle = transform.Find("RunningParticle");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            moveDirection = 0;

            if (Input.GetKey(KeyCode.LeftArrow) && blockDirection != -1)
            {
                moveDirection += -1f;
                animator.SetFloat("Idle_LR", -1);
            }
            if (Input.GetKey(KeyCode.RightArrow) && blockDirection != 1)
            {
                moveDirection += 1f;
                animator.SetFloat("Idle_LR", 1);
            }

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                if(fixedJoint2D == null)
                {
                    RaycastBox();
                }
                else
                {
                    UnregisterBox();
                }
            }

            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
            animator.SetFloat("movingSpeed", moveDirection);
            
            if ((Input.GetKeyDown(KeyCode.Space) && jumpCount < 2) && (fixedJoint2D == null))
            {
                rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                jumpCount++;
                if (jumpCount == 2 && rb.velocity.y < 1.5f) //더블 점프 이후에도 속도가 나지 않는 경우 약간의 가속을 더해준다.
                {
                    rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
                }
                if (jumpCount == 1) //바닥에 닿자마자 다시 점프하면 JumpCount가 갱신되지 않는다.(한 프레임 내에서 1->0->1로 돌아와서 안변하는 걸로 판단하는듯) 그래서 강제로 실행
                {
                    animator.Play(Animator.StringToHash("Base Layer.Jump.Jump"), 0, 0f);
                }
                animator.SetInteger("JumpCount", jumpCount);
                SoundMgr.In.PlaySFX("Jump", 1, 0.1f);
            }

            if (moveDirection < 0)
            {
                spriteRenderer.flipX = (isHoldingBox && boxDirection == 1) ? false : true; //박스 당기기 시 반전되면 안된다.
                if (!isHoldingBox && jumpCount < 1)
                {
                    runningParticle.gameObject.SetActive(true);
                    runningParticle.localPosition = new Vector3(0.71f, -0.68f, 0);
                    runningParticle.localScale = new Vector3(-1, 1, 1);
                }
            }
            else if (moveDirection > 0)
            {
                spriteRenderer.flipX = (isHoldingBox && boxDirection == -1) ? true : false; //박스 당기기 시 반전되면 안된다.
                if (!isHoldingBox && jumpCount < 1)
                {
                    runningParticle.gameObject.SetActive(true);
                    runningParticle.localPosition = new Vector3(-0.71f, -0.68f, 0);
                    runningParticle.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Idle.Idle"))
                {
                    spriteRenderer.flipX = false;
                }
                else if (!isHoldingBox)
                {
                    spriteRenderer.flipX = animator.GetFloat("Idle_LR") > 0 ? false : true;
                }
                else if (boxDirection == -1) //박스가 왼쪽에 있는 상태에서 가만히 있는 경우는 예외로 flip을 해주어야한다.
                {
                    spriteRenderer.flipX = true;
                }

                runningParticle.localScale = new Vector3(1, 1, 1);
                runningParticle.gameObject.SetActive(false);
            }
        }
    }

    public void StartDead()
    {
        StartCoroutine(CharacterDead());
    }

    private IEnumerator CharacterDead()
    {
        float disappearTime = 1f;
        float time = 0;

        Color alpha = spriteRenderer.color;

        runningParticle.gameObject.SetActive(false);
        while (time < disappearTime)
        {
            alpha.a = 1 - time / disappearTime;
            spriteRenderer.color = alpha;

            yield return null;
            time += Time.deltaTime;
        }

        InGameMgr.In.EndGame(false);
    }
    
    private void RaycastBox() {
        RaycastHit2D hit;
        int direction = animator.GetFloat("Idle_LR") < 0 ? -1 : 1;

        if (!verticalTrigger)
            return;

        hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 0.6f, 1 << LayerMask.NameToLayer("Box"));
        
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Box>().isPlayerGrabbing = true;

            fixedJoint2D = hit.collider.GetComponent<FixedJoint2D>();

            fixedJoint2D.enabled = true;
            fixedJoint2D.connectedBody = gameObject.GetComponent<Rigidbody2D>();

            moveSpeed = 2f;

            isHoldingBox = true;
            boxDirection = direction;
            animator.SetBool("isHoldingBox", isHoldingBox);
            animator.SetInteger("BoxDirection", boxDirection);
        }
    }

    public void UnregisterBox()
    {
        if(fixedJoint2D != null)
        {
            fixedJoint2D.connectedBody = null;
            fixedJoint2D.enabled = false;
            fixedJoint2D.gameObject.GetComponent<Box>().isPlayerGrabbing = false;

            fixedJoint2D = null;

            isHoldingBox = false;
            boxDirection = 0;
            animator.SetBool("isHoldingBox", isHoldingBox);
            animator.SetInteger("BoxDirection", boxDirection);

            moveSpeed = 4f;
        }
    }

    public bool CanEnterPortal()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Idle.Idle") ||
            animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Idle.HiddenIdle"))
        { //서있는 상태에서만 입장 가능
            return true;
        }

        return false;
    }

    public void CheckCollideV(bool enterExit)
    {
        verticalTrigger = enterExit;

        this.blockDirection = 0;
        if (enterExit)
        {
            jumpCount = 0;
            animator.SetInteger("JumpCount", jumpCount);
            landingParticle.gameObject.SetActive(true);
        }
        else
        {
            if(jumpCount < 1 && rb.velocity.y <0)
            {
                jumpCount++;
                animator.SetInteger("JumpCount", jumpCount);
            }
            runningParticle.localScale = new Vector3(1, 1, 1);
            runningParticle.gameObject.SetActive(false);
        }
    }

    public void CheckCollideH(bool enterExit, int blockDirection = 0)
    {
        if (enterExit)
        {
            if(!verticalTrigger)
            {
                this.blockDirection = blockDirection;
            }
            else
            {
                this.blockDirection = 0;
            }
        }
        else
        {
            this.blockDirection = 0;
        }
    }

    /// <summary>
    /// 포탈 들어갈 때 등도는 애니메이션으로 전환
    /// </summary>
    /// <returns></returns>
    public void EnterPortal()
    {
        animator.SetTrigger("LookBack");
        this.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpCount > 0 && (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Button")))
        {
            if(verticalTrigger) //트리
            {
                Debug.Log("2");
                jumpCount = 0;
                animator.SetInteger("JumpCount", 0);
            }
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            if (transform.position.y >= collision.gameObject.transform.position.y + 1.1f) //박스 위를 밟는 경우
            {
                Debug.Log("3");
                jumpCount = 0;
                animator.SetInteger("JumpCount", jumpCount);
            }
        }
        if (collision.gameObject.CompareTag("Thorn"))
        {
            isDead = true;
            animator.SetTrigger("CharacterDead");
        }
        if (collision.gameObject.CompareTag("Spring"))
        {
            animator.Play(Animator.StringToHash("Base Layer.Jump.Jump"),0,0f);
            jumpCount = 1;
            animator.SetInteger("JumpCount", jumpCount);
            SoundMgr.In.PlaySFX("Spring", 1, 0.75f);

            rb.velocity = Vector2.ClampMagnitude(rb.velocity,10);
            Debug.Log(rb.velocity);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            runningParticle.localScale = new Vector3(1, 1, 1);
            runningParticle.gameObject.SetActive(false);
        }
    }

}
