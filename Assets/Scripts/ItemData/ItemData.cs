using UnityEngine;
public enum ItemType
{
    Consumable, // 소비 아이템
    Equipment,  // 장비 아이템
    Resource    // 자원 아이템
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;        // 아이템 이름
    [TextArea]
    public string description;     // 아이템 설명
    public Sprite icon;            // UI 아이콘
    public ItemType type;          // 아이템 타입
    public GameObject dropPrefab;  // 드랍 아이템

    [Header("스택 관련")]
    public bool canStack = false;  // 중첩 가능 여부
    public int maxStack = 1;       // 최대 스택 수 (1이면 중첩 불가)

    [Header("사용 효과")]
    public float effectValue;      // 효과 값 (예: 체력 회복량)
    public float duration;         // 지속 시간 (0이면 즉시 효과)
}
