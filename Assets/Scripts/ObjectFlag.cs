using UnityEngine;

public class ObjectFlag : MonoBehaviour
{
    [SerializeField] private GameObject objParticle;    // 깃발에 도달했을 때 생성할 파티클 오브젝트

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")  // 충돌한 객체가 "Character" 태그를 가지고 있는지 확인
        {
            Debug.Log("캐릭터가 깃발에 도달했습니다.");
            GameManager.Instance.GameClear();   // GameManager의 GameClear 메서드 호출하여 게임 클리어 처리

            if (GameManager.Instance.IsGameClear())  // 게임 클리어 상태인지 확인
            {
                if (objParticle != null)        // objParticle이 할당되어 있는지 확인
                {
                    Instantiate(objParticle, transform.position, Quaternion.identity);  // 깃발 위치에 파티클 생성
                }
            }
        }
    }
}
