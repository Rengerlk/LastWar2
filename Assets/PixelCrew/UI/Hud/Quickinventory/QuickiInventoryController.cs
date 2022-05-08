using System;
using System.Collections.Generic;
using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposable;
using UnityEngine;

using UnityEngine.UI;

namespace PixelCrew.UI.Hud.Quickinventory
{
    public class QuickiInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        
        private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();

        private DataGroup<InventoryItemData, InventoryItemWidget> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidget>(_prefab, _container);
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
          
           
            Rebuild();
        }

        private void Rebuild()
        {
            var _inventory = _session.QuickInventory.Inventory;
            _dataGroup.SetData(_inventory);
//create required  items
           
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}