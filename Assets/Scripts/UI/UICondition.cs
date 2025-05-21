using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition expendables;

    public Condition stamina;
    public Condition health;

    public GameObject expendablesUI;

    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
