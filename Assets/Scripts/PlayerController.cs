using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동속도")]
    [SerializeField] private float fMoveSpeed = 4.0f;       // 이동속도
    [Header("점프 힘")]
    [SerializeField] private float fJumpForce = 6.0f;       // 점프 힘

    [Header("활 공격")]
    [SerializeField] private ObjectArrow prefabArrow;  // 발사할 화살 프리팹
    [SerializeField] private Transform trBow;  // 활의 위치를 나타내는 트랜스폼

    private Animator animator = null;                 // 애니메이터 컴포넌트
    private Rigidbody rbody = null;                 // 강체
    private Vector3 vMoveDir = Vector3.zero;        // 이동방향
    private float fMoveThreshold = 0.1f;             // 이동 입력 임계값

    private LayerMask layerGround;     // 지면 레이어 마스크
    private bool bJumping = false;              // 점프 중 여부
    private bool bJumpRequested = false;            // 점프 요청 여부
    private bool bGroundCheck = false;             // 지면 체크 여부
    private float fGroundDistance = 0.1f;          // 지면과의 거리 (점프 가능 여부 판단용)

    private bool isGameOver = false;    // 게임 오버 상태를 나타내는 변수
    private bool isGameClear = false;   // 게임 클리어 상태를 나타내는 변수

    private bool isShooting = false;  // 공격 중 여부를 나타내는 변수

    private void Start()
    {
        animator = GetComponent<Animator>();        // 애니메이터 컴포넌트 가져오기
        rbody = GetComponent<Rigidbody>();          // 강체 컴포넌트 가져오기
        layerGround = LayerMask.GetMask("Ground");  // "Ground" 레이어 마스크 설정
    }

    private void Update()
    {
        if (isGameOver || isGameClear)
        {
            return; // 게임 오버 또는 클리어 상태에서는 입력 처리 중단
        }

        if (GameManager.Instance.IsGameClear())
        {
            isGameClear = true;  // 게임 클리어 상태 설정
            GetComponent<Collider>().enabled = false;  // 플레이어의 콜라이더를 비활성화하여 충돌 처리 방지        
            rbody.isKinematic = true;  // 강체를 키네마틱으로 설정하여 물리적 상호작용 방지            
            animator.SetTrigger("GameClear");  // 게임 클리어 애니메이션 트리거 설정
        }
        else if (GameManager.Instance.IsGameOver())
        {
            isGameOver = true;  // 게임 오버 상태 설정
            GetComponent<Collider>().enabled = false;  // 플레이어의 콜라이더를 비활성화하여 충돌 처리 방지        
            rbody.isKinematic = true;  // 강체를 키네마틱으로 설정하여 물리적 상호작용 방지            
            animator.SetTrigger("GameOver");  // 게임 오버 애니메이션 트리거 설정
        }

        HandleMoveInput();   // 이동 입력 처리
        CheckGround();       // 지면 체크
        HandleJumpInput();   // 점프 입력 처리
        HandleShootInput();  // 공격 입력 처리
    }

    private void FixedUpdate()
    {
        if (isGameOver || isGameClear)
        {
            return; // 게임 오버 또는 클리어 상태에서는 이동 및 회전 처리 중단
        }

        Move();  // 이동 처리
        Rotate();  // 회전 처리
        if (bJumpRequested)
        {
            Jump();  // 점프 처리
            bJumpRequested = false;  // 점프 요청 플래그 초기화
        }
    }

    private void HandleMoveInput()
    {
        if(isShooting)
        {
            vMoveDir = Vector3.zero;  // 공격 중에는 이동 방향을 0으로 설정하여 이동 불가
            animator.SetBool("isWalk", false);  // 이동 애니메이션 해제
            return; // 공격 중에는 이동 입력 처리 중단
        }

        float fHorizontal = Input.GetAxis("Horizontal");  // 수평 입력값
        float fVertical = Input.GetAxis("Vertical");      // 수직 입력값

        vMoveDir = new Vector3(fHorizontal, 0, fVertical);  // 이동 방향 계산

        if (vMoveDir.magnitude > fMoveThreshold)   // 이동 방향의 크기가 0보다 크면 노말라이즈(정규화)
        {
            vMoveDir.Normalize();       // 이동 방향 노말라이즈(정규화)
            animator.SetBool("isWalk", true);  // 이동 애니메이션 설정
        }
        else
        {
            animator.SetBool("isWalk", false);  // 이동 애니메이션 해제
        }
    }

    private void CheckGround()
    {
        // 레이캐스트 시작 위치 (플레이어의 발 위치에 약간의 오프셋 추가)
        Vector3 vOrigin = transform.position + new Vector3(0.0f, 0.01f, 0.0f);

        bGroundCheck = Physics.Raycast(vOrigin, Vector3.down, fGroundDistance, layerGround);  // 지면과의 거리 체크

        if (bGroundCheck)
        {
            bJumping = false;  // 점프 중 상태 해제
            animator.SetBool("isRanding", true);  // 착지 애니메이션 설정
        }
        else
        {
            bJumping = true;  // 공중 상태 설정
            animator.SetBool("isRanding", false);  // 착지 애니메이션 해제
        }
    }

    private void HandleJumpInput()
    {
        // 스페이스 키 입력, 지면에 붙어 있을 때, 점프 중이 아닐 때, 공격 중이 아닐 때 점프 처리
        if (Input.GetKeyDown(KeyCode.Space) && bGroundCheck && bJumping == false && isShooting == false)
        {
            bJumpRequested = true;  // 점프 요청 플래그 설정
            bJumping = true;  // 점프 중 상태 설정

            animator.SetTrigger("Jump");  // 점프 애니메이션 트리거 설정
        }
    }

    private void Move()
    {
        // 이동량 계산 (이동 방향 * 이동 속도 * 고정된 시간 간격)
        Vector3 vMoveAmount = vMoveDir * fMoveSpeed * Time.fixedDeltaTime;

        rbody.MovePosition(rbody.position + vMoveAmount);  // 강체 위치 업데이트
    }

    private void Rotate()
    {
        if (vMoveDir.magnitude > 0)
        {
            transform.forward = vMoveDir;  // 이동 방향으로 회전
        }
    }

    private void Jump()
    {
        rbody.AddForce(Vector3.up * fJumpForce, ForceMode.Impulse);  // 위쪽으로 점프 힘 적용
        bGroundCheck = false;  // 점프 후에는 지면 체크를 false로 설정하여 공중 상태로 전환
    }

    private void HandleShootInput()
    {
        // 이동 입력이 일정 임계값 이상일 때 걷는 상태로 간주
        bool bIsWalking = vMoveDir.magnitude > fMoveThreshold;

        // 마우스 왼쪽 버튼 클릭시 공격 , 지면에 붙어 있을때만 공격, 걷는 상태에서는 공격 불가
        if (Input.GetMouseButtonDown(0) && bGroundCheck && bJumping == false && bIsWalking == false && isShooting == false) 
        {
            animator.SetTrigger("ShootBow");
        }
    }

    private void OnDrawGizmos()
    {

        // 지면 체크를 위한 레이캐스트 시각화
        Gizmos.color = Color.red;  // 레이캐스트 색상 설정
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * fGroundDistance);  // 레이캐스트 라인 그리기
        if (bGroundCheck)
        {
            Gizmos.color = Color.blue;  // 지면에 닿았을 때 색상 변경            
        }
        else
        {
            Gizmos.color = Color.yellow;  // 지면에 닿지 않았을 때 색상 설정
        }
        Gizmos.DrawSphere(transform.position + Vector3.down * fGroundDistance, 0.1f);  // 지면 체크 위치에 구 그리기
    }

    public void ShootStart()
    {
        // 공격 애니메이션 이벤트에서 호출되는 함수
        isShooting = true;  // 공격 중 상태 설정
    }

    public void ShootEnd()
    {
        // 공격 애니메이션 이벤트에서 호출되는 함수
        isShooting = false;  // 공격 중 상태 해제
    }

    public void Shooting()
    {
        // 화살이 발사되는 시점에 공격 애니메이션 이벤트에서 호출되는 함수
        // 화살 모델 회전 보정 (Y -90도)
        Quaternion qRotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, -90, 0);
        // 화살 프리팹을 활 위치에 생성하여 발사
        ObjectArrow objArrow = Instantiate(prefabArrow, trBow.position, qRotation); 
        objArrow.Launch(transform.forward);  // 캐릭터 정면 방향으로 발사
    }
}
