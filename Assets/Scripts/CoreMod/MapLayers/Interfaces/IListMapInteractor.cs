using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public interface IListInteractor<TObject, TLayerObject, TLayer> where TLayer : IListMapLayer<TLayerObject> where TLayerObject : class
	{
		event ObjectDelegate<TObject> ObjectClicked;
		event ObjectDelegate<TObject> ObjectDeClicked;
		event ObjectDelegate<TObject> ObjectRightClicked;
		event ObjectDelegate<TObject> ObjectDeRightClicked;
		event ObjectDelegate<TObject> ObjectHighlighted;
		event ObjectDelegate<TObject> ObjectDeHighlighted;
		event ObjectDelegate<TObject> ObjectHovered;
		event ObjectDelegate<TObject> ObjectDeHovered;
	}
}

