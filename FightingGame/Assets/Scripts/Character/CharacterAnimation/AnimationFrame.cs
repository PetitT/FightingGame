using System;
using UnityEngine;

[Serializable]
public struct AnimationFrame
{
    [SerializeField] private GameObject _parent;
    [SerializeField] private int _frameCount;

    public int FrameCount => _frameCount;

    public void SetDisplayed(
        bool displayed
        )
    {
        _parent.SetActive( displayed );
    }
}