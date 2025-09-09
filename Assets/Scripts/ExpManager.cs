using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class ExpManager : MonoBehaviour
{
  public static ExpManager Instance { get; set; }

  public AugmentManager augmentManager;

  public Slider expSlider;
  public TextMeshProUGUI levelText;

  [HideInInspector] public float currentExp;
  [HideInInspector] public float maxExp;
  [HideInInspector] public int level = 1;

  [SerializeField] private int augmentLevel = 3;

  private bool _isAnimating = false;
  private float _queuedExp = 0f;

  // ���� ��� ����
  private int _pendingAugmentCount = 0;

  // [�߰�] ���� ���� UI�� ���ִ��� ����(�ߺ� ȣ�� ����)
  private bool _isAugmentShowing = false;

  // [�߰�] ���� UI ����(�ν����Ϳ� �Ҵ�) Ȥ�� �̱��� ���
  [SerializeField] private AugmentChoiceUI augmentUI;

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
  }

  public void AddExp(float addValue)
  {
    // �ִϸ��̼� �߿��� ť�� �ױ⸸
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
    float toMax = maxExp - currentExp;
    float step = Mathf.Min(remaining, toMax);

    float startNorm = Mathf.Clamp01(currentExp / maxExp);
    float endNorm = Mathf.Clamp01((currentExp + step) / maxExp);

    // Ʈ�� ����
    expSlider.DOKill();

    
    float baseDur = 0.5f;                              // �� �� �� ä��� ���� �ð�
    float dur = Mathf.Lerp(0.15f, baseDur, endNorm - startNorm); // �ʹ� ª������ �ʰ� ����

    expSlider.DOValue(endNorm, dur)
             .SetEase(Ease.OutCubic)
             .OnComplete(() =>
             {
               currentExp += step;    // ���� �� �ݿ�
               remaining -= step;     // ���� ����ġ ����

               // [���̽� 1] �� ������ ����
               if (Mathf.Approximately(currentExp, maxExp))
               {
                 level++;
                 PlayerController.Instance.ps.AddBallCount();

                 DOTween.Sequence()
                            .AppendInterval(0.1f)   // ���� �� ����� ��� ����
                            .Append(expSlider.DOValue(0f, 0.1f).SetEase(Ease.Linear)) // ������ ���� �ִϸ��̼�
                            .OnComplete(() =>
                            {
                              levelText.text = $"Lv : {level}";

                              currentExp = 0f; // ���� ���� ����

                              // ������ ��ġ ����
                              maxExp = MaxExpUpdate(level);

                              if (level % augmentLevel == 0)
                              {
                                _pendingAugmentCount++;
                              }

                              // ���� ����ġ�� ������ ���� ���� �������� �̾ ä��
                              if (remaining > 0f) AnimateGain(remaining);
                              else ProcessQueuedExp(); // ť�� ������ �߰� �Է� ó��
                            });
               }
               // �Ϲ� ����ġ ä���
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
    augmentLevel = 3;

    expSlider.value = 0;
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
    PlayerController.Instance.OnPlayerReady += TryShowAugment; // [�߰�]
  }

  private void OnDisable()
  {
    if (PlayerController.Instance != null)
      PlayerController.Instance.OnPlayerReady -= TryShowAugment; // [�߰�]
    augmentManager.OnAugmentFinished -= OnAugmentFinished;
  }

  private void OnAugmentFinished()
  {
    _isAugmentShowing = false;

    if (_pendingAugmentCount > 0)
      _pendingAugmentCount--; 

    // ���� ������ �׶��� �簳
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
