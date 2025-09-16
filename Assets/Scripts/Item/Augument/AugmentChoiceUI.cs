using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class AugmentChoiceUI : MonoBehaviour
{
		[Serializable]
		public class OptionWidgets
		{
				public Button button;
				public Image icon;
				public TextMeshProUGUI title;
				public TextMeshProUGUI desc;

				public CanvasGroup cg;

		}

		[Header("옵션 슬롯")]
		public OptionWidgets[] options = new OptionWidgets[3];

  // 패널 등장/퇴장 연출용
  [Header("Panel Anim")]
  [SerializeField] private CanvasGroup panelCg; 
  [SerializeField] private Transform content;   
  [SerializeField] private CanvasGroup dimBg;   

  // 애니메이션 파라미터
  [SerializeField] private float enterDuration = 0.25f;
  [SerializeField] private float exitDuration = 0.15f;
  [SerializeField] private float dimAlpha = 0.5f;
  [SerializeField] private bool unscaledTime = true;   

  private Action<AugmentDefinition> _onPick;


  void Awake()
  {
    if (content) content.localScale = Vector3.one * 0.9f;
				if(dimBg) dimBg.alpha = 0f;
  }

  public void Show(List<AugmentDefinition> defs, Action<AugmentDefinition> onPick)
  {
    gameObject.SetActive(true);
    _onPick = onPick;

    // 트윈 정리
    DOTween.Kill(panelCg);
    if (content) DOTween.Kill(content);
    if (dimBg) DOTween.Kill(dimBg);

    // 패널 등장 초기값
    {
      panelCg.alpha = 0f;
      panelCg.interactable = false;
      panelCg.blocksRaycasts = true;
      if (content) content.localScale = Vector3.one * 0.9f;
      if (dimBg) dimBg.alpha = 0f;
    }

    for (int i = 0; i < options.Length; i++)
    {
      var w = options[i];

      if (i < defs.Count)
      {
        var d = defs[i];
        if (w.title) w.title.text = d.displayName;
        if (w.desc) w.desc.text = d.description;
        if (w.icon)
        {
          w.icon.sprite = d.icon;
        }

        w.button.onClick.RemoveAllListeners();

								int index = i;
								var def = d;

        w.button.onClick.AddListener(() =>
        {
          PlaySelectAnimation(index, () =>
          {
												Hide(() =>
												{
														_onPick?.Invoke(def);

												});
												
          });
        });
        

        ResetOptionVisual(w);

        if (w.cg) w.cg.alpha = 0f;

        w.button.gameObject.SetActive(true);
						}
						else
						{
        w.button.onClick.RemoveAllListeners();
        w.button.gameObject.SetActive(false);
						}
				}

    var seq = DOTween.Sequence().SetUpdate(unscaledTime);
    
    // 페이드 인
    if (dimBg) seq.Join(dimBg.DOFade(dimAlpha, enterDuration * 0.72f).SetUpdate(unscaledTime));
    seq.Join(panelCg.DOFade(1f, enterDuration).SetUpdate(unscaledTime));

    // 스케일 키우기
    if (content) seq.Join(content.DOScale(1f, enterDuration).SetEase(Ease.OutBack, 1.2f).SetUpdate(unscaledTime));

    foreach (var w in options)
    {
      if (w.icon)
      {
        if (w?.cg) seq.Join(w.cg.DOFade(1f, enterDuration).SetUpdate(unscaledTime));
        if (w?.icon) seq.Join(w.icon.DOColor(Color.white, enterDuration).SetUpdate(unscaledTime)); 
      }
    }

    seq.OnComplete(() =>
    {
      panelCg.interactable = true;   // 입력 허용
    });


  }

  public void Hide(System.Action onClosed = null)
  {
    panelCg.interactable = false;

    DOTween.Kill(panelCg);
    if (content) DOTween.Kill(content);
    if (dimBg) DOTween.Kill(dimBg);

    var seq = DOTween.Sequence().SetUpdate(unscaledTime);
    if (dimBg) seq.Join(dimBg.DOFade(0f, exitDuration).SetUpdate(unscaledTime));
    if (content) seq.Join(content.DOScale(0.95f, exitDuration).SetUpdate(unscaledTime));
    seq.Join(panelCg.DOFade(0f, exitDuration).SetUpdate(unscaledTime));

    foreach (var w in options)
    {
      if (w?.cg) seq.Join(w.cg.DOFade(0f, exitDuration).SetUpdate(unscaledTime));
    }

    seq.OnComplete(() =>
    {
      foreach (var w in options)
        if (w.button) w.button.onClick.RemoveAllListeners();

      panelCg.blocksRaycasts = false;
      gameObject.SetActive(false);
      onClosed?.Invoke();
    });
  }

  private void ResetOptionVisual(OptionWidgets w)
  {
    if (!w.cg)
    {
      w.cg = w.button.GetComponent<CanvasGroup>();
    }
    if (!w.cg)
    {
      w.cg = w.button.gameObject.AddComponent<CanvasGroup>();
    }

    w.cg.ignoreParentGroups = false;

    w.cg.alpha = 1f;
    w.cg.interactable = true;
    w.cg.blocksRaycasts = true;

    // 살짝 축소되었다가 등장시키고 싶다면 여기서 처리 가능
    w.button.transform.localScale = Vector3.one; // 안전 초기화
  }

  private void PlaySelectAnimation(int selectedIndex, Action onComplete)
  {
    if (selectedIndex < 0 || selectedIndex >= options.Length) { onComplete?.Invoke(); return; }

    var selected = options[selectedIndex];

    for (int i = 0; i < options.Length; i++)
    {
      if (i == selectedIndex) continue;
      var w = options[i];
      if (w?.cg)
      {
        w.cg.interactable = false;
        w.cg.blocksRaycasts = false;
        w.cg.DOFade(0.3f, 0.15f).SetUpdate(unscaledTime);                 
        w.button.transform.DOScale(0.98f, 0.15f).SetUpdate(unscaledTime);
       
      }
    }

    selected.button.transform
        .DOPunchScale(new Vector3(0.1f, 0.1f, 0f), 0.2f, 6, 0.8f)
        .SetUpdate(unscaledTime);

    if (selected.icon)
    {
      selected.icon.DOColor(Color.white, 0.15f).SetUpdate(unscaledTime);
    }



    DOVirtual.DelayedCall(0.25f, () => onComplete?.Invoke())
             .SetUpdate(unscaledTime);
  }

}