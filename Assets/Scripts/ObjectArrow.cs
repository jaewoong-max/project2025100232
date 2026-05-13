using UnityEngine;

public class ObjectArrow : MonoBehaviour
{
    [SerializeField] private float fSpeed = 20.0f;       // 화살 속도
    [SerializeField] private float fLifetime = 3.0f;     // 화살 수명(초)

    private Rigidbody rbody = null;

    public void Launch(Vector3 vDirection)
    {
        rbody = GetComponent<Rigidbody>();     // Rigidbody 컴포넌트 가져오기
        rbody.linearVelocity = vDirection.normalized * fSpeed;    // 화살을 발사 방향으로 이동시키기
        Destroy(gameObject, fLifetime); //  fLifetime 초 지난후에 오브젝트 제거
    }
}
