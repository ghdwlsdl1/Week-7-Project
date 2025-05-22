using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static InventoryUI Instance;

    // 인벤토리 슬롯 배열
    public InventorySlot[] slots;

    // 아이템 정보 UI 텍스트
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statValueText;

    // 버튼 참조
    public Button useButton;
    public Button dropButton;

    // 인벤토리 UI 루트 (On/Off 토글 대상)
    [SerializeField] private GameObject contentRoot;

    // 현재 선택된 슬롯
    private InventorySlot selectedSlot;

    // 인벤토리 열림 여부
    private bool isOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 시작 시 인벤토리 비활성화 + 버튼 이벤트 연결
        contentRoot.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        useButton.onClick.AddListener(OnClickUse);
        dropButton.onClick.AddListener(OnClickDrop);
    }

    // 인벤토리 열기/닫기 토글
    public void ToggleInventory()
    {
        isOpen = !isOpen;
        contentRoot.SetActive(isOpen);

        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;

        // 인벤토리 열릴 때 시점 고정
        CharacterManager.Instance.Player.controller.canLook = !isOpen;
    }

    // 아이템 추가 처리
    public void AddItem(ItemData item)
    {
        // 스택 가능한 경우 같은 슬롯에 추가
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.HasItem(item) && item.canStack)
            {
                slot.AddCount(1);
                return;
            }
        }

        // 비어있는 슬롯에 새로 추가
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.Set(item, 1);
                SelectSlot(slot);
                return;
            }
        }

        Debug.Log("인벤토리 가득 참");
    }

    // 슬롯 선택 시 강조 표시 및 정보 패널 갱신
    public void SelectSlot(InventorySlot slot)
    {
        if (selectedSlot != null)
            selectedSlot.Highlight(false);

        selectedSlot = slot;
        selectedSlot.Highlight(true);

        UpdateInfoPanel(slot.GetItem());
    }

    // 아이템 상세 정보 표시
    public void UpdateInfoPanel(ItemData item)
    {
        if (item == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
            statNameText.text = "";
            statValueText.text = "";
            return;
        }

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        statNameText.text = "포만감\n지속시간";
        statValueText.text = item.effectValue.ToString("F0") +
                             (item.duration > 0 ? $"\n{item.duration:F1} 초" : "");
    }

    // 사용 버튼 클릭 시 실행
    public void OnClickUse()
    {
        Debug.Log($"[Use 버튼 클릭] selectedSlot: {selectedSlot?.GetItem()?.name}");
        if (selectedSlot != null)
            selectedSlot.Use();
    }

    // 버리기 버튼 클릭 시 실행
    public void OnClickDrop()
    {
        if (selectedSlot != null)
        {
            ItemData item = selectedSlot.GetItem();

            // 드롭 프리팹이 있으면 캐릭터 앞에 소환
            if (item.dropPrefab != null)
            {
                Instantiate(item.dropPrefab, CharacterManager.Instance.Player.dropPosition.position, Quaternion.identity);
            }

            // 슬롯 비우고 정보 패널 초기화
            selectedSlot.Clear();
            UpdateInfoPanel(null);
        }
    }
}
