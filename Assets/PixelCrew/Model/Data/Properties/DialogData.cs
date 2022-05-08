using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private string[] _sentence;
        public string[] Sentences => _sentence;

    }
}