using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool isPlayerNear = false;
    private Rigidbody2D rb;
    public bool isPlayerGrabbing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(UpdatePlayer());
    }

    private void OnDisable()
    {
        if (GetComponent<FixedJoint2D>().connectedBody != null)
        {
            GetComponent<FixedJoint2D>().connectedBody.GetComponent<CharacterMove>().UnregisterBox();
        }
    }

    private IEnumerator UpdatePlayer()
    {
        while (gameObject.activeSelf == true)
        {
            if (!isPlayerGrabbing && Physics2D.OverlapCircle(transform.position, 3, 1 << LayerMask.NameToLayer("Player")) != null)
            {
                rb.mass = 1000f;
            }
            else
            {
                rb.mass = 2f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            //먼지 애니메이션
        }
        if (collision.gameObject.CompareTag("Spring"))
        {
            if (!isPlayerGrabbing)
            {
                KZLib.SoundMgr.In.PlaySFX("Spring", 1, 0.75f);
            }
        }
    }
}
