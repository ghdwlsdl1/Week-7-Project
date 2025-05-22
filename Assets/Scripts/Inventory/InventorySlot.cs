using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    // 슬롯에 표시될 아이콘 이미지
    public Image icon;

    // 아이템 개수를 표시하는 텍스트
    public TextMeshProUGUI countText;

    // 현재 슬롯이 가진 아이템 데이터
    private ItemData item;

    // 현재 아이템 개수
    private int count;

    // 외곽선 강조 효과 (선택 시 사용)
    private Outline outline;

    private void Awake()
    {
        // 슬롯 생성 시 외곽선 컴포넌트 가져오고 비활성화
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }

    // 슬롯에 아이템과 개수 설정
    public void Set(ItemData newItem, int newCount)
    {
        item = newItem;
        count = newCount;

        icon.sprite = item?.icon;
        icon.enabled = true;
        icon.color = Color.white;

        countText.text = count > 1 ? count.ToString() : "";

        if (outline != null)
            outline.enabled = false;
    }

    // 슬롯 클릭 시 인벤토리에 선택 슬롯으로 전달
    public void OnClick()
    {
        InventoryUI.Instance.SelectSlot(this);
    }

    // 아이템 사용 처리
    public void Use()
    {
        ItemData item = GetItem();
        Debug.Log($"[Use] item: {item?.name}, effectValue: {item?.effectValue}, duration: {item?.duration}, count: {count}");

        if (item == null || count <= 0) return;

        PlayerCondition player = CharacterManager.Instance.Player.condition;

        float cur = player.GetExpendablesValue();
        float max = player.GetExpendablesMax();

        // 포만 상태일 경우 사용 불가
        if (cur + item.effectValue > max)
        {
            Debug.Log("포만 상태라 사용할 수 없습니다");
            return;
        }

        // 소비 아이템일 경우 효과 적용
        if (item.type == ItemType.Consumable && item.effectValue > 0f)
        {
            player.RecoverExpendables(item.effectValue, item.duration);
            player.ApplyItemEffect(item);
        }

        count--;
        if (count <= 0)
            Clear();

        InventoryUI.Instance.UpdateInfoPanel(null);
    }

    // 슬롯 초기화 (아이템 제거)
    public void Clear()
    {
        item = null;
        count = 0;

        icon.sprite = null;
        icon.enabled = true;
        icon.color = Color.white;

        countText.text = "";

        if (outline != null)
            outline.enabled = false;
    }

    // 슬롯 강조 표시 (선택 효과)
    public void Highlight(bool on)
    {
        if (outline != null)
            outline.enabled = on;
    }

    // 슬롯이 비어있는지 여부
    public bool IsEmpty()
    {
        return item == null;
    }

    // 특정 아이템과 같은지 확인
    public bool HasItem(ItemData data)
    {
        return item == data;
    }

    // 아이템 개수 추가
    public void AddCount(int amount)
    {
        count += amount;
        countText.text = count > 1 ? count.ToString() : "";
    }

    // 현재 슬롯의 아이템 반환
    public ItemData GetItem()
    {
        return item;
    }
}
