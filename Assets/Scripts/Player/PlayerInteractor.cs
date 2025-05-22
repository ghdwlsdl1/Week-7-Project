using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    // 상호작용 가능한 최대 거리
    public float interactRange = 3f;

    // 상호작용 대상으로 인식할 레이어 (예: 아이템만)
    public LayerMask interactableLayer;

    // 현재 바라보고 있는 상호작용 대상
    private ItemPickup currentTarget;

    void Start()
    {
        // 게임 시작 시 커서를 잠그고 숨김
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 화면 중앙 기준으로 Ray 발사 (조준점 방향)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Ray가 상호작용 가능한 오브젝트에 닿았는지 검사
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            // 맞은 오브젝트에서 ItemPickup 컴포넌트를 가져옴
            ItemPickup pickup = hit.collider.GetComponentInParent<ItemPickup>();

            if (pickup != null)
            {
                // 새로운 대상이면 툴팁 표시
                if (currentTarget != pickup)
                {
                    currentTarget = pickup;
                    currentTarget.ShowTooltip();
                }

                // E 키를 누르면 아이템 획득 시도
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentTarget.PickUp();
                }
            }
        }
        else
        {
            // 아무 대상도 없으면 툴팁 숨김 및 대상 초기화
            if (currentTarget != null)
            {
                currentTarget.HideTooltip();
                currentTarget = null;
            }
        }
    }
}
