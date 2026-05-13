using UnityEngine;
using TMPro;

public class UpdaterStar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textStarCount;  // 별 개수를 표시할 TextMeshProUGUI 컴포넌트 참조 변수

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            // 현재 별 개수와 최대 별 개수를 텍스트로 표시
            textStarCount.text = GameManager.Instance.GetStarCount() + "/" + GameManager.Instance.GetMaxStarCount();
        }
    }
}
