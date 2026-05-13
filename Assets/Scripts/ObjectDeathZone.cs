using UnityEngine;

public class ObjectDeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")  // 충돌한 객체가 "Character" 태그를 가지고 있는지 확인
        {
            Debug.Log("캐릭터가 사망 구역에 들어갔습니다.");
            GameManager.Instance.GameOver();    // GameManager의 GameOver 메서드 호출하여 게임 오버 처리
        }
    }
}
