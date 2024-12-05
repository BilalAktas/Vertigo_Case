using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpinLevelManager : Singleton<SpinLevelManager>
{

    #region Variables

    [SerializeField] private HorizontalLayoutGroup layoutGroup;

    [SerializeField] private Image currentBox;
    [SerializeField] private Sprite[] currentBoxSprites;

    [SerializeField] private GameObject newContentPrefab;
    
    [HideInInspector] public int currentLevel = 1;

    [SerializeField] private GameObject exitButton;

    [SerializeField] private Transform contentParent;
    private List<TextMeshProUGUI> contentTexts = new();
    
    #endregion


    #region Methods

      private void Start()
    {
        contentTexts = contentParent.GetComponentsInChildren<TextMeshProUGUI>().ToList();

        SetTexts();
        CheckExitButton();
    }

    private void SetTexts()
    {
        var _id = 1;
        foreach (var _text in contentTexts)
        {
            var _color = new Color();
            if (_id == currentLevel)
            {
                _text.fontSize = 115;
                _color = _id % 5 == 0 ? Color.green : Color.black;
            }
            else
            {
                _color = _id % 5 == 0 ? Color.green : Color.white;
                _text.fontSize = 72;
            }
            //var 
            _color.a = _id < currentLevel ? .5f : 1f;
            _text.color = _color;
            _text.text = _id.ToString();

       
            
            _id++;
        }

        currentBox.sprite = currentLevel % 5 == 0 ? currentBoxSprites[1] : currentBoxSprites[0];
    }

    private float CalculatePadding()
    {
        var _m = currentLevel < 10 ? 85 : 84f;
        _m = currentLevel >= 5 && currentLevel < 10 ? 84.5f : _m;
        return 669f - ((currentLevel-1) * _m);
    }
    
    private IEnumerator LayoutGroupAnim()
    {
        var _timer = 0f;
        var _duration = .5f;

        var _s = layoutGroup.padding.left;
        var _t = CalculatePadding();
        
        while (_timer < _duration)
        {
            _timer += Time.deltaTime;

            var _p = Mathf.Lerp(_s, _t, _timer / _duration);
            layoutGroup.padding = new RectOffset((int)_p,0,-11,0);
            
            yield return new WaitForEndOfFrame();
        }
        
        layoutGroup.padding = new RectOffset((int)_t,0,-11,0);
    }
    
    public void IncreaseLevel()
    {
        currentLevel++;
        
        var _clone = Instantiate(newContentPrefab, contentParent);
        contentTexts.Add(_clone.GetComponentInChildren<TextMeshProUGUI>());
        
        StartCoroutine(LayoutGroupAnim());
        SetTexts();
        
    }
    
    public void CheckExitButton()=> exitButton.SetActive(SpinManager.Instance.canSpin && currentLevel % 5 == 0);

    public void ExitButton_ForceDisable() => exitButton.SetActive(false);

    #endregion

 

  
}
