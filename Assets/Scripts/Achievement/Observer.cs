using System.Collections.Generic;
using UnityEngine;

namespace Achievement
{
    public abstract class Observer : MonoBehaviour
    {
        // Notify
        public abstract void OnNotify(string value);
    }

    public abstract class Subject : MonoBehaviour
    {
        // List of observers
        private List<Observer> observers = new List<Observer>();

        public void RegisterObserver(Observer observer)
        {
            observers.Add(observer);
        }

        public void Notify(string value)
        {
            foreach (var observer in observers)
            {
                observer.OnNotify(value);
            }
        }
    }
}