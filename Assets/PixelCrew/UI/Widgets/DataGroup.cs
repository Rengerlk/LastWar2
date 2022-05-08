using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.UI.Widgets
{
    public class DataGroup<TDatatype, TItemType> where TItemType: MonoBehaviour, IItemRenderer<TDatatype>
    {
        private  readonly List<TItemType> _createdItem = new List<TItemType>();
        private readonly TItemType _prefab;
        private readonly Transform _container;

        public DataGroup(TItemType prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }
        public void SetData(IList<TDatatype> data)
        {
            
//create required  items
            if (data != null)
                for (var i = _createdItem.Count; i < data.Count; i++)
                {
                    var item = Object.Instantiate(_prefab, _container);
                    _createdItem.Add(item);
                }

            for (var i = 0; i < data.Count; i++)
            {
                _createdItem[i].SetData(data[i], i); 
                _createdItem[i].gameObject.SetActive(true);
            }
            
            // hide unused items

            for (var i = data.Count; i < _createdItem.Count; i++)
            {
                _createdItem[i].gameObject.SetActive(false);
            }   
        }
    }

    public interface IItemRenderer<TDataType>
    {
        void SetData(TDataType data, int index);
    }
}