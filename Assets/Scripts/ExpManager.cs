using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System;

public class ExpManager : MonoBehaviour
{
  public static ExpManager Instance { get; set; }

  public AugmentManager augmentManager;
		private PlayerController pc;

  public Slider expSlider;
  public TextMeshProUGUI levelText;

  [HideInInspector] public float currentExp;
  [HideInInspector] public float maxExp;
  [HideInInspector] public int level = 1;

  private int augmentLevel = 5;

  private bool _isAnimating = false;
  private float _queuedExp = 0f;

		[HideInInspector] public int _pendingAugmentCount = 0;					// 증강 대기 개수
  private bool _isAugmentShowing = false;			// 증강을 보여주는 상태 확인 변수

		// 대기 상태 돌입 이벤트
		public event Action OnIdle;

		public bool CanReadyNow
		{
				get
				{
						return !_isAnimating  && !_isAugmentShowing;
				}
		}

		private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject); 
  }

  void Start()
  {
    ResetExp();
				pc = PlayerController.Instance;
  }

  public void AddExp(float addValue)
  {
    // 애니메이션 중에는 큐에 쌓기만
    _queuedExp += addValue * pc.ps.expGain;
    if (_isAnimating) return;

    _isAnimating = true;
    ProcessQueuedExp();
  }

  private void ProcessQueuedExp()
  {
    float gain = _queuedExp;
    _queuedExp = 0f;
    if (gain <= 0f)
    {
      _isAnimating = false;
						NotifyIdleIfFree();
						return;
    }

    AnimateGain(gain);
  }

  private void AnimateGain(float remaining)
  {
    float toMax = maxExp - currentExp;
    float step = Mathf.Min(remaining, toMax);

    float startNorm = Mathf.Clamp01(currentExp / maxExp);
    float endNorm = Mathf.Clamp01((currentExp + step) / maxExp);

    // 트윈 정리
    expSlider.DOKill();

    
    float baseDur = 0.5f;                              
    float dur = Mathf.Lerp(0.15f, baseDur, endNorm - startNorm); 

    expSlider.DOValue(endNorm, dur)
             .SetEase(Ease.OutCubic)
             .OnComplete(() =>
             {
               currentExp += step;   
               remaining -= step;     

              
               if (Mathf.Approximately(currentExp, maxExp))
               {
                 level++;
                 

                 DOTween.Sequence()
                            .AppendInterval(0.075f) // 딜레이
                            .Append(expSlider.DOValue(0f, 0.1f).SetEase(Ease.Linear)) // 게이지 리셋
                            .OnComplete(() =>
                            {
                              levelText.text = $"Lv : {level}";

                              currentExp = 0f;

                              
                              maxExp = MaxExpUpdate(level);

                              if (level % augmentLevel == 0)
                              {
                                _pendingAugmentCount++;
                                PlayerController.Instance.ps.AddBallCount();
                              }

                              
                              if (remaining > 0f) AnimateGain(remaining);
                              else ProcessQueuedExp(); 
                            });
               }
               // 일반 경험치 채우기
               else if (remaining > 0f)
               {
                 AnimateGain(remaining);
               }
               // 
               else
               {
                 ProcessQueuedExp();
               }
             });
  }

  float MaxExpUpdate(int level)
  {
    return 3 * Mathf.Pow(1.2f, level - 1);

  }

  public void ResetExp()
  {
    currentExp = 0;
    maxExp = 3;
    level = 1;
    _queuedExp = 0;

    expSlider.value = 0;
  }

		private void NotifyIdleIfFree()
		{
				if (CanReadyNow)
				{
						OnIdle?.Invoke();
				}
		}

		private void TryShowAugment()
  {
    if (_isAugmentShowing) return;
    if (_pendingAugmentCount <= 0) return;

    _isAugmentShowing = true;

    Time.timeScale = 0f;                
    augmentManager.AugmentApply(level); 
  }


  private void OnEnable()
  {
    augmentManager.OnAugmentFinished += OnAugmentFinished;
    StartCoroutine(Co_SubscribeReady());
  }

  private IEnumerator Co_SubscribeReady()
  {
    while (PlayerController.Instance == null) yield return null;
    PlayerController.Instance.OnPlayerReady += TryShowAugment; 
  }

  private void OnDisable()
  {
    if (PlayerController.Instance != null)
      PlayerController.Instance.OnPlayerReady -= TryShowAugment;
    augmentManager.OnAugmentFinished -= OnAugmentFinished;
  }

  private void OnAugmentFinished()
  {
    _isAugmentShowing = false;

    if (_pendingAugmentCount > 0)
      _pendingAugmentCount--; 

    if (_pendingAugmentCount > 0)
    {
      TryShowAugment();     
    }
    else
    {
      Time.timeScale = 1f;
				}
  }
}
