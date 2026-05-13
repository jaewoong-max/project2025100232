using UnityEngine;

public class ObjectJumpSpring : MonoBehaviour
{
    [Header("점프 설정")]
    [SerializeField] private float fJumpForce = 10.0f;  // 점프 힘

    private Animator animator;  // 애니메이터 컴포넌트

    private void Start()
    {
        animator = GetComponent<Animator>();  // 애니메이터 컴포넌트 가져오기
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")  // 충돌한 객체가 "Character" 태그를 가지고 있는지 확인
        {
            Debug.Log("캐릭터가 스프링에 닿았습니다.");
            animator.SetTrigger("Spring");  // 점프 애니메이션 트리거 설정
            Rigidbody rb = other.GetComponent<Rigidbody>();  // 충돌한 객체의 Rigidbody 컴포넌트 가져오기
            rb.linearVelocity = Vector3.zero;  // 기존의 속도를 초기화하여 점프가 일정하게 적용되도록 함
            rb.AddForce(Vector3.up * fJumpForce, ForceMode.Impulse);  // 위 방향으로 점프 힘 적용

            other.GetComponent<Animator>().SetTrigger("Jump");  // 캐릭터의 점프 애니메이션 트리거 설정
        }
    }
}
