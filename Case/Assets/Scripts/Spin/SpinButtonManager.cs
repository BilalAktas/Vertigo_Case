using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SpinButtonManager : MonoBehaviour
{
    #region Variables

    [Header("Spin Button")]
    [SerializeField] private RectTransform spinButtonTransform;
    [SerializeField] private Image spinButton;
    private const float SpinButtonScaleDefault = 2f;
    private const float SpinButtonScaleUp = 2.5f;
    private const float ButtonScaleDuration = .2f;
    private const float ButtonScaleDownDuration = .1f;

    #endregion

    #region Methods

    public void AnimateSpinButton()
    {
        spinButtonTransform.DOScale(SpinButtonScaleUp, ButtonScaleDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            spinButtonTransform.DOScale(0f, ButtonScaleDownDuration).SetEase(Ease.Linear);
        });
    }
 
    public void ResetSpinButton()
    {
        spinButtonTransform.DOScale(SpinButtonScaleDefault, ButtonScaleDuration).SetEase(Ease.Linear);
    }

    public void SetSpinButtonColor(float alpha)
    {
        var color = spinButton.color;
        color.a = alpha;
        spinButton.color = color;
    }

    #endregion

   
}