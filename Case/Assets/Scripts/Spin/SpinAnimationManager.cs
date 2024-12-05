using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAnimationManager : MonoBehaviour
{
    [SerializeField] private Animation indicatorAnimation;

    public void PlayIndicatorAnimation()
    {
        indicatorAnimation.Play();
    }
}
