using UnityEngine;
using System.Collections;

namespace CoreMod
{
    public delegate void ObjectDelegate<T> (T obj);
    public interface IListInteractor<TObject, TLayerObject, TLayer> where TLayer : IListMapLayer<TLayerObject>
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

