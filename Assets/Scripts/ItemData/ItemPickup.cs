using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // 이 오브젝트가 보유한 아이템 데이터
    public ItemData itemData;

    // 마우스가 올려졌을 때 툴팁 표시
    public void ShowTooltip()
    {
        ItemTooltipUI.Instance.Show(itemData, transform.position);
    }

    // 마우스가 벗어났을 때 툴팁 숨김
    public void HideTooltip()
    {
        ItemTooltipUI.Instance.Hide();
    }

    // 아이템 획득 처리
    public void PickUp()
    {
        // 인벤토리에 아이템 추가
        InventoryUI.Instance.AddItem(itemData);

        // 툴팁 숨김
        ItemTooltipUI.Instance.Hide();

        // 씬에서 오브젝트 제거
        Destroy(gameObject);
    }
}
