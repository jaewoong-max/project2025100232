using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdaterTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTimer;  // 타이머를 표시할 TextMeshProUGUI 컴포넌트 참조 변수
    [SerializeField] private Image imgFillTimer;  // 타이머의 채워지는 이미지를 참조하는 변수

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            // 남은 시간을 텍스트로 표시 (소수점 둘째 자리까지)
            textTimer.text = GameManager.Instance.GetRemainingTime().ToString("F2") + "s";
            // 타이머 이미지의 fillAmount를 남은 시간에 비례하여 업데이트 (0에서 1 사이의 값)
            imgFillTimer.fillAmount = GameManager.Instance.GetRemainingTime() / GameManager.Instance.GetMaxTime();
        }
    }
}
