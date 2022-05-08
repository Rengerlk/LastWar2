using UnityEngine;

namespace PixelCrew.Components.Interactions
{
    public class DoInteractionComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
           var interactable = go.GetComponent<Interectablecomponent>();
           if (interactable != null)
               interactable.Interact();
        }
    }
}