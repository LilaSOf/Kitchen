using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public interface IHasProgress
{
    public event EventHandler<OnProgressChangedEvnetArgs> OnProgressChanged;
    public class OnProgressChangedEvnetArgs : EventArgs
    {
        public float progressNormalized;
    }
}
