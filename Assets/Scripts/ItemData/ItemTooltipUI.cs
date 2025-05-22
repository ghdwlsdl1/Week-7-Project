using UnityEngine;
using TMPro;

public class ItemTooltipUI : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static ItemTooltipUI Instance;

    // 툴팁에 표시할 텍스트
    public TextMeshProUGUI infoText;

    // 툴팁 전체 오브젝트
    public GameObject tooltipObject;

    private void Awake()
    {
        // 인스턴스 초기화 및 툴팁 비활성화
        Instance = this;
        tooltipObject.SetActive(false);
    }

    // 툴팁 표시 및 정보 갱신
    public void Show(ItemData data, Vector3 worldPos)
    {
        tooltipObject.SetActive(true);

        // 아이템 이름 + 설명 표시
        infoText.text = $"{data.itemName}\n<size=36>{data.description}</size>";

        // 툴팁 위치 화면 하단 중앙으로 고정
        tooltipObject.transform.position = new Vector2(Screen.width / 2f, 100f);
    }

    // 툴팁 숨김
    public void Hide()
    {
        tooltipObject.SetActive(false);
    }
}
