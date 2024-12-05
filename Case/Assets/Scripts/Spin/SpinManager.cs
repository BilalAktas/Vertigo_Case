using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpinManager : Singleton<SpinManager>, IPointerClickHandler
{
    #region Script Ref
     
    [Header("Scripts")]
    [SerializeField] private SpinButtonManager buttonManager;
    [SerializeField] private SpinSoundManager soundManager;
    [SerializeField] private SpinAnimationManager animationManager;
    
    #endregion
    
    #region Variables
    
    [Header("Spin")]
    [SerializeField] private AnimationCurve durationCurve;
    [SerializeField] private AnimationCurve durationCurveSecondPart;
    [SerializeField] private Transform spinWheel;
    [HideInInspector] public bool canSpin = true;
    
    private SlotItemForSpin currentSlotItem;
    private bool changeDurationCurve;
    private const float SpinDuration = 5f;
    private const float InitialSpinAngle = -1080;
    
    #endregion
    
    #region Private Methods

    #region Spin

    private void Spin()
    {
        buttonManager.SetSpinButtonColor(.5f);
        buttonManager.AnimateSpinButton();
        soundManager.PlaySpinSound();
        
        SpinLevelManager.Instance.ExitButton_ForceDisable();

        currentSlotItem = SlotManager.Instance.GetSlotItemForSpin();
        var targetAngle = CalculateSpinAngle(currentSlotItem.id);

        StartCoroutine(SpinRotateCoroutine(SpinDuration, targetAngle));
    }
    
    private float CalculateSpinAngle(int itemId) => (8 - (itemId - 1)) * -45f + InitialSpinAngle;
    
    private IEnumerator SpinRotateCoroutine(float duration, float targetAngle)
    {
        var timer = 0f;
        var currentAngle = 0f;
        var indicatorLimit = 45f;
        
        targetAngle -= 1440f; // 

        var _curve = durationCurve;

        while (timer < duration)
        {
            var nT = timer / duration;
            var curveTime = _curve.Evaluate(nT);

            timer += Time.deltaTime;
            var newAngle = Mathf.Lerp(currentAngle, targetAngle, curveTime);
            var absAngle = Mathf.Abs(newAngle);
            
            spinWheel.rotation = Quaternion.Euler(0, 0, newAngle);

            if (absAngle > indicatorLimit)
            {
                indicatorLimit += 45f;
                animationManager.PlayIndicatorAnimation();
            }

            if (newAngle < -1440f)
            {
                absAngle = .7f;

                if (!changeDurationCurve)
                {
                    changeDurationCurve = true;
                    _curve = durationCurveSecondPart;
                }
            }

            soundManager.AdjustSpinSoundPitch(absAngle);

            yield return null;
        }

        spinWheel.rotation = Quaternion.Euler(0, 0, targetAngle);
        soundManager.StopSpinSound();

        GameManager.Instance.HandleSpinResult(currentSlotItem.slotItemProperty);
    }

    #endregion
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canSpin)
        {
            canSpin = false;
            Spin();
        }
    }
    
    public void Set_OnAgain()
    {
        canSpin = true;
        changeDurationCurve = false;
        buttonManager.ResetSpinButton();
        buttonManager.SetSpinButtonColor(1f);
        
        SpinLevelManager.Instance.CheckExitButton();
    }
    #endregion
}
