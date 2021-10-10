using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KZLib
{
    public class CreditWindow : Window
    {
        public Text notepadText;

        private StringBuilder builder;

        protected override void DoAwake()
        {
            builder = new StringBuilder();
        }

        protected override void DoOnEnable()
        {
            builder.Clear();

            notepadText.text = builder.ToString();

            StopAllCoroutines();

            StartCoroutine(ShowCredit());
        }

        IEnumerator ShowCredit()
        {
            for(int i=0;i<2;i++)
            {
                notepadText.text = "l";

                yield return new WaitForSeconds(0.5f);

                notepadText.text = "";

                yield return new WaitForSeconds(0.5f);
            }

            builder.Append("l");

            notepadText.text = builder.ToString();

            yield return WriteText("기획 : 이은수");

            yield return new WaitForSeconds(0.3f);

            yield return WriteText("그래픽 : 김태우");

            yield return new WaitForSeconds(0.3f);

            yield return WriteText("그래픽 : 황우빈");

            yield return new WaitForSeconds(0.3f);

            yield return WriteText("프로그래머 : 고재현");

            yield return new WaitForSeconds(0.3f);

            yield return WriteText("프로그래머 : 유현우");

            yield return new WaitForSeconds(0.3f);

            yield return WriteText("감사합니다.");

            yield return new WaitForSeconds(0.3f);

            while (true)
            {
                builder.Remove(builder.Length-1,1);

                notepadText.text = builder.ToString();

                yield return new WaitForSeconds(0.5f);

                builder.Append("l");

                notepadText.text = builder.ToString();

                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator WriteText(string _text)
        {
            foreach(var letter in _text.ToCharArray())
            {
                builder.Remove(builder.Length-1,1);

                builder.Append($"{letter}l");

                notepadText.text = builder.ToString();

                yield return new WaitForSeconds(0.1f);
            }

            builder.Remove(builder.Length-1,1);

            builder.Append("\nl");

            notepadText.text = builder.ToString();
        }
    }
}