using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _ObjectToDestroy;
        public void DestroyObject()
        {
            Destroy(_ObjectToDestroy);
        }
    }
}