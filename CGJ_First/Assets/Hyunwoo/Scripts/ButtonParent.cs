using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonParent : MonoBehaviour
{
    private Button buttonCollider;

    private void Awake()
    {
        buttonCollider = gameObject.GetComponentInChildren<Button>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")))
        {
            if (!buttonCollider.collidingObjects.Contains(collision.gameObject))
            {
                buttonCollider.collidingObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")))
        {
            if (buttonCollider.collidingObjects.Contains(collision.gameObject))
            {
                buttonCollider.collidingObjects.Remove(collision.gameObject);
            }
        }
    }
}
