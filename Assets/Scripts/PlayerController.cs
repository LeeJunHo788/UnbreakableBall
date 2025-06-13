using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

  public static PlayerController Instance { get; private set; }

  [HideInInspector]
  public InputSystem_Actions controls;

  [HideInInspector]
  public Rigidbody2D rb;

  [HideInInspector]
  public Vector3 startPos;

  [HideInInspector]
  public bool isStartPosFixed = false;

  [HideInInspector]
  public bool addBall = false;

  [HideInInspector]
  public int activeBallCount = 0;
  public bool isReady = true;

  [Header("할당 오브젝트")]
  public GameObject directionObj;
  public GameObject ballPrefab;

  [Header("스탯")]
  public float att = 10;
  public float defIg = 0;
  public int additionalBallCount = 0;
  public float moveSpeed = 30;

  List<GameObject> subBalls = new List<GameObject>();

  float angle = 90f;         // 현재 각도
  float angleSpeed = 60f;   // 회전 속도

  public event Action OnPlayerReady;

  private void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(gameObject);  // 중복된 오브젝트는 삭제


    controls = new InputSystem_Actions();
    controls.Player.Enable();
    controls.Player.Fire.performed += ct => BallFire();
  }

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    startPos = transform.position;

    transform.rotation = Quaternion.Euler(0, 0, angle);

    OnPlayerReady?.Invoke();
  }


  private void Update()
  {
    SetDirection();
  }

  private void SetDirection()
  {
    float input = controls.Player.Move.ReadValue<Vector2>().x;

    // 입력이 있을 때 각도를 조절
    if (input != 0 && isReady)
    {
      angle += -input * angleSpeed * Time.deltaTime;
       
      angle = Mathf.Clamp(angle, 20, 170);
     
      transform.rotation = Quaternion.Euler(0, 0, angle);
    }
  }

  // 공 발사 메서드
  void BallFire()
  {
    if(isReady)
    {
      float angleRad = angle * Mathf.Deg2Rad;
      Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

      StartCoroutine(AddBall(dir));
      
      rb.linearVelocity = dir.normalized * moveSpeed;
      directionObj.gameObject.SetActive(false);
      isReady = false;

    }
  }

  // 공 추가 생성 메서드
  IEnumerator AddBall(Vector2 dir)
  {
    for (int i = 0; i < additionalBallCount; i++)
    {
      yield return new WaitForSeconds(0.1f);

      Debug.Log(startPos);
      GameObject subBall = Instantiate(ballPrefab, startPos, Quaternion.Euler(0, 0, angle));
      Rigidbody2D subRb = subBall.GetComponent<Rigidbody2D>();
      subBalls.Add(subBall);

      subRb.linearVelocity = dir.normalized * moveSpeed;

    }
  }

  public void IsReady()
  {
    isReady = true;
    directionObj.gameObject.SetActive(true);

    if(addBall)
    {
      additionalBallCount++;
      addBall = false;
    }

    for (int i = 0; i < subBalls.Count; i++)
    {
      Destroy(subBalls[i]);
    }

    activeBallCount = 0;

    OnPlayerReady?.Invoke();    // 준비 이벤트 호출
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("DSideBar"))
    {
      rb.linearVelocity = Vector2.zero;

      if (!isStartPosFixed)
      {
        startPos = transform.position;
        isStartPosFixed = true;  // 고정 완료

        transform.rotation = Quaternion.Euler(0, 0, 90);
        activeBallCount++;

        if (activeBallCount == additionalBallCount + 1)
        {

          isStartPosFixed = false;
          IsReady();
        }

      }

      else
      {
        float speed = 20;
        float distance = Vector3.Distance(transform.position, startPos);
        float duration = distance / speed;

        transform.DOMove(startPos, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
          transform.rotation = Quaternion.Euler(0, 0, 90);
          activeBallCount++;

          if (activeBallCount == additionalBallCount + 1)
          {

            isStartPosFixed = false;
            IsReady();
          }

        });
      }
     

      


      angle = 90f;
      
    }
  }
}
