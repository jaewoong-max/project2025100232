using UnityEngine;

public class ObjectStar : MonoBehaviour
{
    [SerializeField] private GameObject objParticle;    // 별을 수집했을 때 생성할 파티클 오브젝트

    private void Update()
    {
        transform.Rotate(0f, 90f * Time.deltaTime, 0f);  // Y축을 기준으로 초당 90도 회전
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")  // 충돌한 객체가 "Character" 태그를 가지고 있는지 확인
        {
            Debug.Log("캐릭터가 별을 수집했습니다.");
            GameManager.Instance.AddStarCount();  // GameManager의 AddStarCount 메서드 호출하여 별 수집 처리

            if (objParticle != null)
            {
                GameObject obj = Instantiate(objParticle);  // 파티클 프리팹을 인스턴스화하여 생성
                obj.transform.position = transform.position;  // 파티클을 별 위치에 생성
            }

            Destroy(gameObject);  // 별 오브젝트 제거
        }
    }
}
