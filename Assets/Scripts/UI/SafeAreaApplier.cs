using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaApplier : MonoBehaviour
{
  Rect lastSafeArea;

  void OnEnable() => ApplySafeArea();
  void Update()
  {
    if (Screen.safeArea != lastSafeArea)
      ApplySafeArea();
  }

  void ApplySafeArea()
  {
    Rect safe = Screen.safeArea;       // [����] �ȼ� ���� SafeArea
    lastSafeArea = safe;

    RectTransform rt = (RectTransform)transform;
    Vector2 min = safe.position;
    Vector2 max = safe.position + safe.size;

    // [����] 0~1 ���� ��Ŀ�� ����ȭ
    min.x /= Screen.width; min.y /= Screen.height;
    max.x /= Screen.width; max.y /= Screen.height;

    rt.anchorMin = min;
    rt.anchorMax = max;
    rt.offsetMin = Vector2.zero;
    rt.offsetMax = Vector2.zero;
  }
}