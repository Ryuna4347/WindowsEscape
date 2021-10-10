using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingPlayer : MonoBehaviour
{
   

    public Animator animator;

    public IEnumerator Play(Text _text)
    {
        animator.Play("Idle_R");

        yield return AddQ(_text);

        animator.Play("Idle_L");

        yield return AddQ(_text);

        animator.Play("Look_Back");

        yield return AddQ(_text);

        animator.Play("Idle_R");
    }

    IEnumerator AddQ(Text _text)
    {
        for(int i=0;i<5;i++)
        {
            _text.text += "?";

            yield return new WaitForSeconds(0.1f);
        }
    }
}
