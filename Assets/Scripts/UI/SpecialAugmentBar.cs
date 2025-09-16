using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpecialAugmentBar : MonoBehaviour
{
  [Header("아이콘 슬롯")]
  [SerializeField] private Image[] slots = new Image[3];

  public void ResetBar()
  {
    for (int i = 0; i < slots.Length; i++)
    {
      if (!slots[i]) continue;
      slots[i].sprite = null;
      slots[i].enabled = false;
      slots[i].color = Color.white; 
      slots[i].transform.localScale = Vector3.one;
      DOTween.Kill(slots[i].transform);
    }
  }

  public bool AddIcon(Sprite icon)
  {
    if (!icon) return false;

    for (int i = 0; i < slots.Length; i++)
    {
      if (!slots[i]) continue;

      if (slots[i].sprite == null || slots[i].enabled == false)
      {
        slots[i].sprite = icon;
        slots[i].enabled = true;

        var t = slots[i].transform;
        t.localScale = Vector3.one * 0.9f;
        t.DOScale(1f, 0.2f).SetEase(Ease.OutBack);

        return true;
      }
    }
    return false;
  }

  public int FilledCount
  {
    get
    {
      int c = 0;
      for (int i = 0; i < slots.Length; i++)
      {
        if (slots[i] && slots[i].enabled && slots[i].sprite != null) c++;
      }
      return c;
    }
  }

  public int Capacity => slots != null ? slots.Length : 0;
}