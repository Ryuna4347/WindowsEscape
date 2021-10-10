using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]private float basePosY;
    [SerializeField]private float targetPosY;
    [SerializeField]private bool isActivated;
    [SerializeField] private bool isHorizontal;
    public float animationSpeed;
    private Coroutine doorCoroutine;


    private void Awake()
    {
        if(!isActivated)
        {
            basePosY = transform.localPosition.y;
            targetPosY = transform.localPosition.y - transform.localScale.y * GetComponent<SpriteRenderer>().size.y;
        }
        else
        {
            basePosY = transform.localPosition.y + transform.localScale.y * GetComponent<SpriteRenderer>().size.y;
            targetPosY = transform.localPosition.y;
        }

        if(isHorizontal)
        {
            gameObject.tag = "Map";
        }
    }


    public void OpenDoor()
    {
        if (!isActivated) {
            if(doorCoroutine != null)
            {
                StopCoroutine(doorCoroutine);
            }

            doorCoroutine = StartCoroutine(Open());
        }
    }

    public void CloseDoor()
    {
        if (isActivated)
        {
            if (doorCoroutine != null)
            {
                StopCoroutine(doorCoroutine);
            }

            doorCoroutine = StartCoroutine(Close());
        }
    }

    private IEnumerator Open()
    {
        isActivated = true;
        
        while (transform.localPosition.y > targetPosY)
        {
            transform.Translate(-transform.up * Time.deltaTime * animationSpeed, Space.World);
            yield return null;
        }
    }

    private IEnumerator Close()
    {
        isActivated = false;

        while (transform.localPosition.y < basePosY)
        {
            transform.Translate(transform.up * Time.deltaTime *animationSpeed, Space.World);
            yield return null;
        }
    }


}
