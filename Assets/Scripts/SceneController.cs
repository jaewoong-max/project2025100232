using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private string currentSceneName;  // 현재 씬의 이름을 저장하는 변수
    [SerializeField] private string nextSceneName;  // 다음 씬의 이름을 인스펙터에서 설정    

    [SerializeField] private int MaxStarCount = 0;  // 현재 씬에서 수집할 수 있는 최대 별의 개수
    [SerializeField] private int MaxTime = 60;  // 현재 씬에서 플레이어가 클리어하기 위해 필요한 최대 시간 (초 단위)

    [SerializeField] private GameObject objPanel;  // 게임 UI 오브젝트를 인스펙터에서 설정
    [SerializeField] private GameObject objPopupClear;  // 게임 클리어 팝업 UI 오브젝트를 인스펙터에서 설정
    [SerializeField] private GameObject objPopupGameOver;  // 게임 오버 팝업 UI 오브젝트를 인스펙터에서 설정

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;  // 현재 씬의 이름을 가져와 저장
        GameManager.Instance.GameStart(this);  // 게임 시작 시 GameManager의 GameStart 메서드 호출

        objPanel.SetActive(true);  // 게임 UI 패널을 활성화하여 표시
        objPopupClear.SetActive(false);  // 게임 클리어 팝업 UI를 비활성화하여 숨김
        objPopupGameOver.SetActive(false);  // 게임 오버 팝업 UI를 비활성화하여 숨김

        SoundManager.Instance.PlayBGM("BGM_GAME");  // 게임 씬에서 BGM 재생
    }

    public void ShowGameClearPopup()
    {
        objPopupClear.SetActive(true);  // 게임 클리어 팝업 UI를 활성화하여 표시
        objPanel.SetActive(false);  // 게임 UI 패널을 비활성화하여 숨김
    }

    public void ShowGameOverPopup()
    {
        objPopupGameOver.SetActive(true);  // 게임 오버 팝업 UI를 활성화하여 표시
        objPanel.SetActive(false);  // 게임 UI 패널을 비활성화하여 숨김
    }

    public void ButtonRestart()
    {
        Debug.Log("버튼 클릭 : 게임 재시작");
        SceneManager.LoadScene(currentSceneName);  // 현재 씬을 다시 로드하여 게임 재시작        
    }

    public void ButtonNext()
    {
        Debug.Log("버튼 클릭 : 다음 씬으로 이동");
        SceneManager.LoadScene(nextSceneName);  // 다음 씬을 로드
    }

    public string GetCurrentSceneName()
    {
        return currentSceneName;  // 현재 씬의 이름을 반환하는 메서드
    }

    public string GetNextSceneName()
    {
        return nextSceneName;  // 다음 씬의 이름을 반환하는 메서드
    }

    public int GetMaxStarCount()
    {
        return MaxStarCount;  // 현재 씬에서 수집할 수 있는 최대 별의 개수를 반환하는 메서드
    }

    public float GetMaxTime()
    {
        return MaxTime;  // 현재 씬에서 플레이어가 클리어하기 위해 필요한 최대 시간을 반환하는 메서드
    }
}
