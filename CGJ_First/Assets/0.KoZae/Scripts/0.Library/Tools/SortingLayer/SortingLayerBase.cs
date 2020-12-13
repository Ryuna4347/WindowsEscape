using UnityEngine;

public class SortingLayerBase : MonoBehaviour
{
	public const string DEFAULT_SORTINGLAYER = "Default";
	public const string DEFAULT_UI_SORTINGLAYER = "UI_0";

	public SortingLayerBase parent;

	[SerializeField] protected string sortingLayerName;

	[Tooltip("parent가 존재하면 무시된다.")]
	[SerializeField] protected int order;

	public string SortingLayerName => sortingLayerName;
	public int SortingOrder => SortingParentOrder + order;
	public int SortingParentOrder => parent != null ? parent.SortingOrder : 0;

	protected virtual void Reset()
	{
		SetMandatory();
		CollectComponents();

		sortingLayerName = parent == null ? DEFAULT_UI_SORTINGLAYER : parent.SortingLayerName;
	}

	protected virtual void OnValidate()
	{
		SetMandatory();
		RefreshComponents();
	}

	protected virtual void Start()
	{
		SetMandatory();
		RefreshComponents();
	}

	protected virtual void SetMandatory()
	{
		if (parent == null)
		{
			parent = transform.parent != null ? transform.parent.GetComponentInParent<SortingLayerBase>() : null;
		}
	}

	public virtual void CollectComponents() { }

	public virtual void RefreshComponents() { }

	public virtual void SetSortingVaule(string _name,int? _order = null)
	{
		if (sortingLayerName.IsOk())
		{
			sortingLayerName = _name;
		}

		if (_order.HasValue)
		{
			order = _order.Value;
		}

		RefreshComponents();
	}

	public virtual void SetSortingLayer(string _name,bool _include)
	{
		SetSortingVaule(_name);

		if (_include)
		{
			foreach (var layer in GetComponentsInChildren<SortingLayerBase>(true))
			{
				layer.SetSortingVaule(_name);
			}
		}
	}

	public string GetNextSortingLayerName()
	{
		var layers = SortingLayer.layers;
		var layerID = SortingLayer.NameToID(SortingLayerName);

		if (SortingLayer.IsValid(layerID))
		{
			var index = Mathf.Clamp(SortingLayer.GetLayerValueFromID(layerID) + 1,0,layers.Length - 1);

			return layers[index].name;
		}
		else
		{
			return layers[0].name;
		}
	}
}