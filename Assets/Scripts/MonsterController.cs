using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    private Transform playerTransform;  // 플레이어의 Transform 컴포넌트
    private NavMeshAgent navAgent;  // NavMeshAgent 컴포넌트
    private Animator animator;  // Animator 컴포넌트
    [SerializeField] private float triggerDistance = 6.0f;  // 몬스터 활성화 트리거 거리    
    [SerializeField] private GameObject objParticle;  // 몬스터가 파괴될 때 재생할 파티클 프리팹을 인스펙터에서 설정

    private void Start()
    {
        // 태그가 "Player"인 게임 오브젝트의 Transform을 가져옴
        playerTransform = GameObject.FindWithTag("Character").transform;

        navAgent = GetComponent<NavMeshAgent>();  // NavMeshAgent 컴포넌트를 가져옴
        animator = GetComponent<Animator>();  // Animator 컴포넌트를 가져옴

        navAgent.isStopped = true;  // 처음에는 몬스터를 비활성화 상태로 설정
    }

    private void Update()
    {
        if (playerTransform != null)    // 플레이어가 존재하는 경우 처리
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);  // 플레이어와의 거리 계산

            if (distanceToPlayer <= triggerDistance)        // 플레이어가 트리거 거리 내에 있는 경우 몬스터 활성화
            {
                navAgent.isStopped = false;  // 몬스터를 활성화 상태로 설정
                navAgent.SetDestination(playerTransform.position);  // 몬스터가 플레이어를 추적하도록 설정
                animator.SetBool("IsMove", true);  // 이동 애니메이션 활성화
            }
            else
            {
                navAgent.isStopped = true;  // 몬스터를 비활성화 상태로 설정
                animator.SetBool("IsMove", false);  // 이동 애니메이션 비활성화
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            GameManager.Instance.GameOver();  // 플레이어와 충돌하면 게임 오버 처리
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            if (objParticle != null)
            {
                GameObject obj = Instantiate(objParticle);
                obj.transform.position = transform.position;  // 몬스터 위치에 파티클 생성
            }

            Destroy(other.gameObject);  // 충돌한 화살 제거
            Destroy(gameObject);  // 화살과 충돌 시 몬스터 오브젝트 제거
        }
    }
}