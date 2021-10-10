using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[ExecuteInEditMode]
	public class ButtonToggler : Toggler
	{
		public UnityEngine.UI.Button[] buttons;

		public override void Apply()
		{
			base.Apply();

			if (!enabled)
			{
				return;
			}

			if (buttons != null)
			{
				for (int i = 0;i < buttons.Length;i++)
				{
					if (buttons[i] != null) buttons[i].interactable = _Toggle;
				}
			}
		}
	}
}