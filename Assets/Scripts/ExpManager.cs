using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ExpManager : MonoBehaviour
{
  public static ExpManager Instance { get; set; }

  public Slider expSlider;
  public TextMeshProUGUI levelText;

  [HideInInspector] public float currentExp;
  [HideInInspector] public float maxExp;
  [HideInInspector] public int level = 1;

  private bool _isAnimating = false;
  private float _queuedExp = 0f;

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
				level = 1;
    currentExp = 0;
    maxExp = 3;
  }

  public void AddExp(float addValue)
  {
    // 애니메이션 중에는 큐에 쌓기만
    _queuedExp += addValue;
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
      return;
    }

    AnimateGain(gain);
  }

  private void AnimateGain(float remaining)
  {
    // 한 스텝: 현재 레벨에서 max까지 채우거나, 남은 만큼만 채우기
    float toMax = maxExp - currentExp;
    float step = Mathf.Min(remaining, toMax);

    float startNorm = Mathf.Clamp01(currentExp / maxExp);
    float endNorm = Mathf.Clamp01((currentExp + step) / maxExp);

    // 트윈 정리
    expSlider.DOKill();

    
    float baseDur = 0.5f;                              // 한 번 꽉 채우는 기준 시간
    float dur = Mathf.Lerp(0.15f, baseDur, endNorm - startNorm); // 너무 짧아지지 않게 하한

    expSlider.DOValue(endNorm, dur)
             .SetEase(Ease.OutCubic)
             .OnComplete(() =>
             {
               currentExp += step;    // 실제 값 반영
               remaining -= step;     // 남은 경험치 차감

               // [케이스 1] 막 레벨업 도달
               if (Mathf.Approximately(currentExp, maxExp))
               {
                 level++;
                 PlayerController.Instance.ps.AddBallCount();

                 DOTween.Sequence()
                            .AppendInterval(0.1f)   // 가득 찬 모습을 잠깐 유지
                            .Append(expSlider.DOValue(0f, 0.1f).SetEase(Ease.Linear)) // 게이지 리셋 애니메이션
                            .OnComplete(() =>
                            {
                              levelText.text = $"Lv : {level}";

                              currentExp = 0f; // 내부 값도 리셋

                              // 레벨업 수치 조정
                              maxExp = MaxExpUpdate(level);

                              // 남은 경험치가 있으면 다음 레벨 게이지를 이어서 채움
                              if (remaining > 0f) AnimateGain(remaining);
                              else ProcessQueuedExp(); // 큐에 누적된 추가 입력 처리
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
}
