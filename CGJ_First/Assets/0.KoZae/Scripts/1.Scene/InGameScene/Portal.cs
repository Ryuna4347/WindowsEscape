using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private bool canUsePortal = false;
        [SerializeField] private Animator animator;
        [SerializeField]CharacterMove characterMove;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                canUsePortal = true;
                characterMove = collision.gameObject.GetComponent<CharacterMove>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                canUsePortal = false;
                characterMove = null;
            }
        }

        private void Update()
        {
            if (characterMove != null)
            {
                if (Input.GetKey(KeyCode.UpArrow) && canUsePortal)
                {
                    if (characterMove.CanEnterPortal())
                    {
                        transform.parent = null;

                        characterMove.transform.position = new Vector3(transform.position.x, transform.position.y-0.3f, characterMove.transform.position.z);
                        characterMove.EnterPortal();
                        animator.SetTrigger("OpenDoor");
                    }
                }
            }
        }

        public void DisappearCharacter()
        {
            characterMove.gameObject.SetActive(false);
            KZLib.SoundMgr.In.PlaySFX("Bye", 1, 0.5f);
        }

        public void StageOver()
        {
            InGameMgr.In.EndGame(true);
        }
    }
}