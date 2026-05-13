using UnityEngine;

public class ObjectBox : MonoBehaviour
{
    [SerializeField] private GameObject objParticle;  // 박스가 파괴될 때 재생할 파티클 프리팹을 인스펙터에서 설정

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            if (objParticle != null)
            {
                GameObject obj = Instantiate(objParticle);
                obj.transform.position = transform.position;    // 박스 위치에 파티클 생성
            }

            Destroy(other.gameObject);  // 충돌한 화살 제거
            Destroy(gameObject);  // 화살과 충돌 시 박스 오브젝트 제거
        }
    }
}
