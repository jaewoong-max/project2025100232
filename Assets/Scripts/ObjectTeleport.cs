using UnityEngine;

public class ObjectTeleport : MonoBehaviour
{
    [SerializeField] private GameObject objCanvas = null;  // "E - 상호작용" 표시용 World Canvas
    [SerializeField] private Vector3 vTeleportPosition = Vector3.zero;  // 플레이어가 텔레포트할 위치

    private bool bPlayer = false;   // 플레이어가 텔레포트 영역에 있는지 여부

    private void Start()
    {
        objCanvas.SetActive(false);
    }

    private void Update()
    {
        // 플레이어가 텔레포트 영역에 있고 E 키를 눌렀을 때 텔레포트 처리
        if(bPlayer && Input.GetKeyDown(KeyCode.E))  
        {
            // 태그가 "Character"인 게임 오브젝트를 찾음
            GameObject player = GameObject.FindWithTag("Character");    
            if (player != null)
            {
                player.transform.position = vTeleportPosition;  // 플레이어 위치를 텔레포트 위치로 이동
            }
        }

        // World Canvas가 항상 카메라를 바라보도록 설정 (Billboard 효과)
        objCanvas.transform.forward = Camera.main.transform.forward;
    }

    // 플레이어가 텔레포트 영역에 들어왔을 때 "E - 상호작용" 표시 활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            bPlayer = true;
            objCanvas.SetActive(true);
        }
    }

    // 플레이어가 텔레포트 영역에서 나갔을 때 "E - 상호작용" 표시 비활성화
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            bPlayer = false;
            objCanvas.SetActive(false);
        }
    }
}
