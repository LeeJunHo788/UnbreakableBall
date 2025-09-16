using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
  public static PlayerController Instance { get; private set; }

  [HideInInspector] public InputSystem_Actions controls;
  [HideInInspector] public Rigidbody2D rb;
  [HideInInspector] public Vector3 startPos;
  [HideInInspector] public bool isStartPosFixed = false;
  public bool canForceReady = true;
  [HideInInspector] public bool isGameOver = false;
  [HideInInspector] public float minReflectAngle = 5f;

  // 회수해야 하는 공
  private int returnBallThisRound = 0;
  public int ReturnBallThisRound => returnBallThisRound;    // 읽기 전용
  // 발사 시점의 추가 공
  private int plannedAddsThisRound = 0;


  List<GameObject> subBalls = new List<GameObject>();   // 서브 공 리스트

  [HideInInspector]
  public int activeBallCount = 0;
  public bool isReady = true;

  [Header("할당 오브젝트")]
  public GameObject directionObj;
  public GameObject ballPrefab;
		public AugmentManager augmentManager;

  [HideInInspector]
  public PlayerStats ps;


		public event Action OnPlayerReady;
  public event Action OnPlayerFire;
		private bool _pendingReadyOnce = false;

  private void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(gameObject);  // 중복된 오브젝트는 삭제


    controls = new InputSystem_Actions();
    controls.Player.Enable();
    controls.Player.Fire.performed += ct => BallFire();

    EnhancedTouchSupport.Enable();
    Touchscreen.current?.MakeCurrent();
  }

  private void Start()
  {
    ps = GetComponent<PlayerStats>();
    rb = GetComponent<Rigidbody2D>();
    startPos = transform.position;

    transform.rotation = Quaternion.Euler(0, 0, ps.angle);


    OnPlayerReady?.Invoke();
  }


  private void Update()
  {
    if (isGameOver) return;

    HandleTouchAimAndFire();
    SetDirection();

  }

  void HandleTouchAimAndFire()
  {
    if (!isReady) return;

    if (Touch.activeTouches.Count > 0)
    {
      var t = Touch.activeTouches[0];

      if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(0))
        return;

      var screenPos = t.screenPosition;
      float z = -Camera.main.transform.position.z;
      Vector3 world = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));

      Vector2 dir = (world - transform.position);
      if (dir.sqrMagnitude > 0.0001f)
      {
        float newAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        newAngle = Mathf.Clamp(newAngle, 20f, 170f);

								ps.angle = newAngle;
        transform.rotation = Quaternion.Euler(0, 0, ps.angle);
      }

      if (t.phase == UnityEngine.InputSystem.TouchPhase.Ended)
      {
        BallFire();
      }
    }
  }

  private void SetDirection()
  {
    if (isGameOver) return;

    if (!isReady) return;

    // 터치가 활성 중이면 키보드 입력 무시
    if (Touch.activeTouches.Count > 0) return;

    float input = controls.Player.Move.ReadValue<Vector2>().x;

    if (Mathf.Abs(input) > 0.0001f)
    {
						ps.angle += -input * ps.angleSpeed * Time.deltaTime;
						ps.angle = Mathf.Clamp(ps.angle, 20, 170);
      transform.rotation = Quaternion.Euler(0, 0, ps.angle);
    }
  }

  // 공 발사 메서드
  void BallFire()
  {
    if (isGameOver) return;

    if (isReady && Time.timeScale != 0)
    {
      canForceReady = false;

      OnPlayerFire?.Invoke();

      float angleRad = ps.angle * Mathf.Deg2Rad;
      Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

      plannedAddsThisRound = ps.additionalBallCount;      
      returnBallThisRound = 1 + plannedAddsThisRound;     

      StartCoroutine(AddBall(dir, plannedAddsThisRound)); 
      
      rb.linearVelocity = dir.normalized * ps.moveSpeed;
      directionObj.gameObject.SetActive(false);
      isReady = false;

    }
  }

  // 공 추가 생성 메서드
  IEnumerator AddBall(Vector2 dir, int plannedCount)
  {
    for (int i = 0; i < plannedCount; i++)
    {
      yield return new WaitForSeconds(ps.shootInterval);

      GameObject subBall = Instantiate(ballPrefab, startPos, Quaternion.Euler(0, 0, ps.angle));
      Rigidbody2D subRb = subBall.GetComponent<Rigidbody2D>();
      subBalls.Add(subBall);

      subRb.linearVelocity = dir.normalized * ps.moveSpeed;
    }

    canForceReady = true;
  }

		public void RegisterSubBall(GameObject ball)
		{
				subBalls.Add(ball);
		}

  public void IncreaseReturnTargetForSplit()
  {
    returnBallThisRound++;
  }

  // 준비, 라운드 증가
  public void IsReady()
  {

    if (activeBallCount != returnBallThisRound)    // [수정]
      return;

    if (ExpManager.Instance != null && !ExpManager.Instance.CanReadyNow)
				{
						if (!_pendingReadyOnce)
						{
								_pendingReadyOnce = true;
								ExpManager.Instance.OnIdle += OnExpIdle_ReadyOnce;
						}
						return;
				}


    if (ps.isAdded != 0)
    {
						ps.additionalBallCount += ps.isAdded;
						ps.isAdded = 0;

    }

    for (int i = 0; i < subBalls.Count; i++)
    {
      Destroy(subBalls[i]);
    }
    subBalls.Clear();

				isReady = true;
				directionObj.gameObject.SetActive(true);

    activeBallCount = 0;

    OnPlayerReady?.Invoke();    // 준비 이벤트 호출
    StageManager.Instance.NextStage();

    returnBallThisRound = 0;

    ps.angle = 90f;

    canForceReady = false;
    DOVirtual.DelayedCall(3f, () =>
    {
						canForceReady = true;
    });
  }

		private void OnExpIdle_ReadyOnce()
		{
				if (ExpManager.Instance != null)
						ExpManager.Instance.OnIdle -= OnExpIdle_ReadyOnce;

				_pendingReadyOnce = false;

    if (activeBallCount != returnBallThisRound)   
      return;

    IsReady(); 
		}

  public static Vector2 ClampMinAngle(Vector2 v, float minDeg)
  {
    float speed = v.magnitude;

    float minSin = Mathf.Sin(minDeg * Mathf.Deg2Rad);

    if (Mathf.Abs(v.y) < speed * minSin)
    {
      float sx = Mathf.Sign(v.x == 0 ? 1f : v.x);
      float sy = Mathf.Sign(v.y == 0 ? 1f : v.y);
      float a = minDeg * Mathf.Deg2Rad;

      Vector2 dir = new Vector2(sx * Mathf.Cos(a), sy * Mathf.Sin(a));
      return dir.normalized * speed;
    }

    return v;
  }


  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("DSideBar"))
    {
      rb.linearVelocity = Vector2.zero;

      if (!isStartPosFixed)
      {
        transform.position = new Vector3(transform.position.x, -6.59f);

        startPos = new Vector3(transform.position.x, -6.57f);
        isStartPosFixed = true;  // 고정 완료

        transform.rotation = Quaternion.Euler(0, 0, 90);
        activeBallCount++;

        if (activeBallCount == returnBallThisRound)
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

        Vector3 movePoint = new Vector3(startPos.x, -6.59f);

        transform.DOMove(movePoint, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
          transform.rotation = Quaternion.Euler(0, 0, 90);
          activeBallCount++;

          if (activeBallCount == returnBallThisRound)
          {

            isStartPosFixed = false;
            IsReady();
          }

        });
      }
    }

				if (collision.collider.CompareTag("Block"))
				{
						var ctx = new BallHitContext
						{
								ball = gameObject,
								ballRb = rb,
								isMainBall = true,
								block = collision.collider.gameObject,
								hitPoint = collision.GetContact(0).point,
								ballVelocity = rb.linearVelocity
						};
						augmentManager.RaiseBlockHit(in ctx);
				}


    if (!collision.collider.CompareTag("DSideBar"))
    {
      rb.linearVelocity = ClampMinAngle(rb.linearVelocity, minReflectAngle);
    }

  }

}
