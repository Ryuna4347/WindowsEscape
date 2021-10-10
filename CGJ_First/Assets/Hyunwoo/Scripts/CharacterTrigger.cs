using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrigger : MonoBehaviour
{
    CharacterMove characterMove;
    public bool isVertical;
    public bool isLeft;
    public List<GameObject> collidingObj = new List<GameObject>();

    private void Awake()
    {
        characterMove = transform.parent.GetComponent<CharacterMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Button") || collision.gameObject.CompareTag("Box"))
        {
            if(isVertical)
            {
                characterMove.CheckCollideV(true);
            }
            else
            {
                if(isLeft)
                {
                    characterMove.CheckCollideH(true, -1);
                }
                else
                {
                    characterMove.CheckCollideH(true, 1);
                }
            }
            collidingObj.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Button") || collision.gameObject.CompareTag("Box"))
        {
            if (collidingObj.Count >= 1)
            {
                if (collidingObj.Contains(collision.gameObject))
                {
                    collidingObj.Remove(collision.gameObject);
                }
                else
                {
                    return;
                }

                if (isVertical)
                {
                    if (collidingObj.Count < 1)
                    {
                        characterMove.CheckCollideV(false);
                    }
                }
                else
                {
                    int direction = collision.gameObject.transform.position.x < transform.position.y ? -1 : 1;
                    characterMove.CheckCollideH(false);
                }
            }
        }
    }
}
