using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스 (set : private - 외부에서 수정 불가)
    public static GameManager Instance { get; private set; }
    private bool isGameOver = false;    // 게임 오버 상태를 나타내는 변수
    private bool isClear = false;   // 게임 클리어 상태를 나타내는 변수
    private int nStarCount = 0;  // 수집한 별의 개수를 저장하는 변수
    private float fStageTime = 60.0f;  // 현재 스테이지의 남은 시간을 저장하는 변수
    private SceneController sceneController;  // SceneController 참조 변수

    private void Awake()
    {
        if (Instance == null)   // 인스턴스가 아직 할당되지 않은 경우
        {
            Instance = this;    // 현재 객체를 인스턴스로 할당
            DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 이 객체가 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);    // 이미 인스턴스가 존재하는 경우, 중복된 객체를 파괴하여 싱글톤 패턴 유지
        }
    }

    private void Update()
    {
        if( isGameOver || isClear)
        {
            return; // 게임 오버 또는 클리어 상태에서는 타이머 업데이트 중단
        }
        
        if (isClear == false && isGameOver == false)  // 게임이 클리어되지 않았고 게임 오버되지 않은 상태에서만 타이머 업데이트
        {
            fStageTime -= Time.deltaTime;  // 매 프레임마다 남은 시간을 감소시킴
            if (fStageTime <= 0)
            {
                GameOver();  // 시간이 다 되었을 때 게임 오버 처리
                fStageTime = 0.0f;  // 남은 시간을 0으로 설정하여 음수로 가지 않도록 함
            }
        }
    }

    public void GameStart(SceneController _scene)
    {
        Debug.Log("게임이 시작되었습니다.");
        sceneController = _scene;       // SceneController 참조 저장
        isGameOver = false;         // 게임 오버 상태 초기화
        isClear = false;            // 게임 클리어 상태 초기화
        nStarCount = 0;    // 게임 시작 시 별 개수 초기화
        fStageTime = _scene.GetMaxTime();  // 게임 시작 시 최대 시간 설정
    }

    public void GameOver()
    {
        if (isGameOver == false)
        {
            Debug.Log("게임 오버!");
            isGameOver = true;
            StartCoroutine(routineGameOver());  // 게임 오버 처리 후 2초 대기 후 게임 오버 팝업 UI 표시
        }
    }

    public void GameClear()
    {
        if (nStarCount < sceneController.GetMaxStarCount())
        {
            Debug.Log("별을 모두 수집해야 깃발에 도달해도 클리어할 수 있습니다.");
            return;  // 별을 모두 수집하지 않았으면 클리어 처리하지 않음
        }

        if (isClear == false)
        {
            isClear = true;
            StartCoroutine(routineClear());  // 게임 클리어 처리 후 2초 대기 후 게임 클리어 팝업 UI 표시
        }
    }

    public void AddStarCount()
    {
        nStarCount++;  // 별 개수 증가
        Debug.Log("별 수집 개수 : " + nStarCount);  // 현재 별 개수 출력 (디버그용)
    }

    public int GetStarCount()
    {
        return nStarCount;  // 현재 수집한 별의 개수를 반환
    }

    public int GetMaxStarCount()
    {
        return sceneController.GetMaxStarCount();  // 현재 씬에서 수집해야 하는 최대 별 개수를 반환
    }

    public float GetRemainingTime()
    {
        return fStageTime;  // 현재 남은 시간을 반환
    }

    public float GetMaxTime()
    {
        return sceneController.GetMaxTime();  // 현재 씬에서 플레이어가 클리어하기 위해 필요한 최대 시간을 반환
    }

    private IEnumerator routineClear()
    {
        yield return new WaitForSeconds(2.0f);  // 2초 대기
        sceneController.ShowGameClearPopup();  // 게임 클리어 팝업 UI 표시            
    }

    private IEnumerator routineGameOver()
    {
        yield return new WaitForSeconds(2.0f);  // 2초 대기
        sceneController.ShowGameOverPopup();  // 게임 오버 팝업 UI 표시        
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsGameClear()
    {
        return isClear;
    }
}
