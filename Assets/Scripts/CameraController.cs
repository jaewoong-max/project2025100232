using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("추적할 대상")]
    [SerializeField] private Transform target;  // 추적할 대상

    [Header("카메라 오프셋")]
    [SerializeField] private Vector3 offset = new Vector3(0, 6, -6);  // 카메라와 대상 사이의 오프셋

    [Header("카메라 회전값")]
    [SerializeField] private Vector3 rot = new Vector3(45, 0, 0);  // 카메라 회전값

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.Log("카메라가 추적할 대상이 없습니다.");
            return;
        }

        // 대상의 위치에 오프셋을 더하여 카메라 위치 계산
        transform.position = target.position + offset;

        // 카메라 회전값 적용
        transform.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
    }
}
