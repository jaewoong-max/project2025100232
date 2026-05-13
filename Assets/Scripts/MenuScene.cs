using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private string GameSceneName = "BaseScene";  // 게임 씬의 이름을 인스펙터에서 설정

    public void ButtonStart()
    {
        SoundManager.Instance.PlaySE("BUTTON");
        Debug.Log("버튼 클릭 : 게임 씬으로 이동");
        SceneManager.LoadScene(GameSceneName);  // 게임 씬으로 이동
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM("BGM_MENU");  // 메뉴 씬에서 BGM 재생
    }
}
