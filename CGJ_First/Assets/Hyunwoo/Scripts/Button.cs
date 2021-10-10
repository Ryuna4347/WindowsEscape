using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Door connectedDoor;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isPressed = false;
    public List<GameObject> collidingObjects = new List<GameObject>();
    
    private Coroutine buttonCoroutine;
    private Coroutine checkPressed;
    private ButtonParent buttonParent;
    [SerializeField]private float basePosY;
    [SerializeField]private float targetPosY;

    private void Start()
    {
#if UNITY_EDITOR
        if (connectedDoor == null)
            Debug.LogWarning("연결된 문 없음");
#endif  
        buttonParent = transform.parent.GetComponent<ButtonParent>();
        if (!isPressed)
        {
            basePosY = transform.localPosition.y;
            targetPosY = transform.localPosition.y - 0.5f;
        }
        else 
        {
            basePosY = transform.localPosition.y + 0.5f;
            targetPosY = transform.localPosition.y;

            StartCoroutine(CheckPressed());
        }
    }

    private IEnumerator ButtonPressedDown()
    {
        connectedDoor.OpenDoor();

        while (transform.localPosition.y > targetPosY)
        {
            transform.position += -transform.up * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(CheckPressed()); 
    }

    private IEnumerator ButtonPressedUp()
    {
        connectedDoor.CloseDoor();
        while (transform.localPosition.y < basePosY)
        {
            transform.position += transform.up * Time.deltaTime;
            yield return null;
        }

        buttonParent.GetComponent<BoxCollider2D>().enabled = false;
        foreach (BoxCollider2D c in GetComponents<BoxCollider2D>())
        {
            c.enabled = true;
        }

        isPressed = false;
    }

    private IEnumerator CheckPressed()
    {
        foreach (BoxCollider2D c in gameObject.GetComponents<BoxCollider2D>())
        {
            c.enabled = false;
        }

        buttonParent.GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(0.1f); //return null로 하면 너무 순식간이라 트리거가 불러지기 전에 아래 루프를 지나가는 것 같음.

        while (collidingObjects.Count > 0)
        {
            yield return null;
        }

        if(buttonCoroutine != null)
        {
            StopCoroutine(buttonCoroutine);
        }
        buttonCoroutine = StartCoroutine(ButtonPressedUp());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")))
        {
            if (!isPressed)
            {
                if (buttonCoroutine != null)
                {
                    StopCoroutine(buttonCoroutine);
                }
                isPressed = true;

                buttonCoroutine = StartCoroutine(ButtonPressedDown());

            }
        }
        
    }

}
