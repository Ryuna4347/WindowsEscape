using KZLib;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]

public class SortingLayerCanvas : SortingLayerBase
{
	public Canvas canvas;

	protected override void OnValidate()
	{
		if (canvas == null)
		{
			canvas = GetComponent<Canvas>();

			if (canvas == null)
			{
				canvas = gameObject.AddComponent<Canvas>();
			}
		}

		canvas.overrideSorting = true;
		canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;

		var raycaster = GetComponent<GraphicRaycaster>();

		if (raycaster == null)
		{
			raycaster = gameObject.AddComponent<GraphicRaycaster>();
		}

		raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

	}

	public override void RefreshComponents()
	{
		base.RefreshComponents();

		canvas.sortingLayerName = SortingLayerName;
		canvas.sortingOrder = SortingOrder;
	}
}