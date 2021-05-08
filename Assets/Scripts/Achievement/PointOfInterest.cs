using System;
using UnityEngine;

namespace Achievement
{
    public class PointOfInterest : Subject
    {
        public static event Action<PointOfInterest> OnPointInterestEntered;
    
        [SerializeField] private string poiName;

        public string PoiName => poiName;
    
        private void OnDisable()
        {
            Notify(poiName);
            OnPointInterestEntered?.Invoke(this);
        }
    }
}
