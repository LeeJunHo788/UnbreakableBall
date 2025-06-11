using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{

  public static PlayerController Instance { get; private set; }

  [HideInInspector]
  public InputSystem_Actions controls;


  [HideInInspector]
  public Rigidbody2D rb;

  [Header("할당 오브젝트")]
  public GameObject directionObj;

  [Header("스탯")]
  public float att = 10;
  public float defIg = 0;


  bool isReady = true;
  Transform startPos;



  float moveSpeed = 10;
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
    startPos = transform;

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
       
      angle = Mathf.Clamp(angle, 20, 160);
     
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
      
      rb.linearVelocity = dir.normalized * moveSpeed;
      directionObj.gameObject.SetActive(false);
      isReady = false;
    }
  }


  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("DSideBar"))
    {
      startPos.position = collision.GetContact(0).point;
      rb.linearVelocity = Vector2.zero;
      transform.rotation = Quaternion.Euler(0, 0, 90);

      angle = 90f;
      isReady = true;
      directionObj.gameObject.SetActive(true);

      OnPlayerReady?.Invoke();    // 준비 이벤트 호출
    }
  }
}
