using System;
using UnityEngine;
using System;
using UnityEngine;

// 데미지를 받을 수 있는 객체가 구현해야 할 인터페이스
public interface IDamagalbe
{
    void TakePhysicaIDamage(int damage); // 물리 데미지 처리
}

// 플레이어의 상태(체력, 스태미너, 허기)를 관리하는 컴포넌트
public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UICondition uiCondition; // 상태 정보를 담은 UI 연결

    // UICondition에서 각각의 상태 가져오기
    Condition expendables { get { return uiCondition.expendables; } }
    Condition stamina     { get { return uiCondition.stamina; } }
    Condition health      { get { return uiCondition.health; } }

    // 데미지 발생 시 호출되는 이벤트 (DamageIndicator 등에서 구독)
    public event Action<int> onTakeDamage;

    void Update()
    {
        // 허기는 줄어들고, 스태미너는 회복
        expendables.Subtract(expendables.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // 허기가 0이 되면 UI 비활성화
        if (expendables.curValue == 0f)
        {
            if (uiCondition.expendablesUI.activeSelf)
            {
                uiCondition.expendablesUI.SetActive(false);
            }
        }

        // 체력이 0이 되면 사망 처리
        if (health.curValue == 0f)
        {
            Die();
        }
    }

    // 체력 회복
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 소모품 효과
    public void Eat(float amount)
    {
        expendables.Add(amount);
    }

    // 사망 처리
    public void Die()
    {
        Debug.Log("사망");
    }

    // 데미지 처리 (인터페이스 구현)
    public void TakePhysicaIDamage(int damage)
    {
        health.Subtract(damage);          // 체력 감소
        onTakeDamage?.Invoke(damage);     // 이벤트로 피격 반응 전달
    }
}

