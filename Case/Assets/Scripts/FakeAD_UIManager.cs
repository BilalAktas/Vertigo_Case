using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FakeAD_UIManager : Singleton<FakeAD_UIManager>
{
    #region Variables

    [SerializeField] private TextMeshProUGUI countDownTimerText;
    [SerializeField] private GameObject adUI;

    #endregion

    #region Methods

    public void ShowAd()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        adUI.SetActive(true);
        
        var _timer = 10f;

        while (_timer>0)
        {
            _timer -= Time.deltaTime;

            countDownTimerText.text = ((int)_timer).ToString();
        
            yield return null;
        }
        
        adUI.SetActive(false);
        
        GameManager.Instance.Ad_Force();
    }

    #endregion
    
   
}
