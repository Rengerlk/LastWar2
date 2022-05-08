﻿using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Editor;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _required;
        [SerializeField] private bool _removeAfterUse;

       [SerializeField] private UnityEvent _onSuccess;
       [SerializeField] private UnityEvent _onFail;
       public void Check()
       {
           var session = FindObjectOfType<GameSession>();
           var areisAllRequirementsMet = true;
           foreach (var item in _required)
           {
               var numItems= session.Data.Inventory.Count(item.Id);
               if (numItems < item.Value)
                   areisAllRequirementsMet = false;
           }
         
          if ( areisAllRequirementsMet)
          {
              if (_removeAfterUse)
              {
                  foreach (var item in _required)
                      session.Data.Inventory.Remove(item.Id, item.Value);
              }
              
              
              _onSuccess?.Invoke();
                 
          }
          else
          {
              _onFail?.Invoke();
          }
       }

    }
}