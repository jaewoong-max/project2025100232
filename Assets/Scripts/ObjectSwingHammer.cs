using UnityEngine;

public class ObjectSwingHammer : MonoBehaviour
{
    [Header("회전할 해머 오브젝트")]
    [SerializeField] private GameObject objHammer;  // 회전할 해머 오브젝트    
    [Header("회전 설정")]
    [SerializeField] private float fSwtingSpeed = 2.0f;  // 회전 속도
    [SerializeField] private float fMaxAngle = 60.0f;     // 최대 회전 각도
    [SerializeField] private bool bAngleHorizontal = false;  // 수평 회전 여부
    private float fTimer = 0.0f;  // 회전 타이머

    private void Update()
    {
        fTimer += Time.deltaTime * fSwtingSpeed;  // 타이머에 회전 속도를 곱하여 증가        
        float fSwingFactor = Mathf.Sin(fTimer);     // Sin 함수를 이용하여, -1에서 1의 값을 생성
        float fNewAngle = fSwingFactor * fMaxAngle;  // 최대 각도에 Swing Factor를 곱하여 새로운 각도 계산
        Vector3 vAngle = objHammer.transform.localEulerAngles;  // 현재 해머의 로컬 회전 각도 가져오기

        if (bAngleHorizontal)
        {
            vAngle.z = fNewAngle;  // Z축 회전 적용            
        }
        else
        {
            vAngle.x = fNewAngle;  // X축 회전 적용
        }

        objHammer.transform.localEulerAngles = vAngle;  // 변경된 회전 각도 적용
    }
}
