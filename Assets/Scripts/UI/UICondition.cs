using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition expendables;     // 허기 상태 UI
    public Condition stamina;         // 스태미너 상태 UI
    public Condition health;          // 체력 상태 UI

    public GameObject expendablesUI;  // 허기 UI 전체 오브젝트 (활성/비활성용)

    void Start()
    {
        // 플레이어의 상태 시스템과 연결
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
