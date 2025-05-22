using System;
using UnityEngine;
using System.Collections;

// 데미지를 받을 수 있는 객체가 구현해야 할 인터페이스
public interface IDamagalbe
{
    void TakePhysicaIDamage(int damage); // 물리 데미지 처리
}

// 플레이어의 상태(체력, 스태미너, 허기)를 관리하는 컴포넌트
public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UICondition uiCondition; // 상태 정보를 담은 UI 연결

    public Condition expendables;   // 허기 수치 상태
    public Condition stamina;       // 스태미너 상태
    public Condition health;        // 체력 상태

    private Coroutine speedCoroutine;
    private Coroutine jumpCoroutine;

    private Coroutine recoveryCoroutine;
    // 허기 상태 수치 외부 접근용
    public float GetExpendablesMax() => expendables.maxValue;
    public float GetExpendablesValue() => expendables.curValue;

    // 데미지 발생 시 호출되는 이벤트 (DamageIndicator 등에서 구독)
    public event Action<int> onTakeDamage;

    void Update()
    {
        // 허기는 줄어들고, 스태미너는 회복
        expendables.Subtract(expendables.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // 허기가 0이 되면 UI 비활성화
        if (expendables.curValue <= 0f)
        {
            if (uiCondition.expendablesUI.activeSelf)
                uiCondition.expendablesUI.SetActive(false);
        }
        // 허기가 0 초과면 UI 활성화
        else
        {
            if (!uiCondition.expendablesUI.activeSelf)
                uiCondition.expendablesUI.SetActive(true);
        }

        // 체력이 0이 되면 사망 처리
        if (health.curValue <= 0f)
            Die();
    }

    // 소모품 효과: 일정량 허기 회복 및 지속시간 동안 회복 제한 처리
    public void RecoverExpendables(float amount, float duration)
    {
        if (expendables.curValue + amount > expendables.maxValue)
            return;

        expendables.Add(amount);

        if (recoveryCoroutine != null)
            StopCoroutine(recoveryCoroutine);

        recoveryCoroutine = StartCoroutine(RecoverRoutine(duration));

        if (!uiCondition.expendablesUI.activeSelf)
            uiCondition.expendablesUI.SetActive(true);
    }

    // 허기 회복 지속시간 처리 (현재는 단순 대기)
    private IEnumerator RecoverRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        recoveryCoroutine = null;
    }

    // 체력 회복
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 데미지 처리 (인터페이스 구현)
    public void TakePhysicaIDamage(int damage)
    {
        health.Subtract(damage);          // 체력 감소
        onTakeDamage?.Invoke(damage);     // 이벤트로 피격 반응 전달
    }

    // 사망 처리
    public void Die()
    {
        Debug.Log("사망");
    }

    private IEnumerator TempSpeedBoost(float multiplier, float duration)
    {
        var controller = CharacterManager.Instance.Player.controller;

        controller.moveSpeed = controller.baseMoveSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        controller.moveSpeed = controller.baseMoveSpeed;
    }

    private IEnumerator TempJumpBoost(float multiplier, float duration)
    {
        var controller = CharacterManager.Instance.Player.controller;

        controller.jumpPower = controller.baseJumpPower * multiplier;
        yield return new WaitForSeconds(duration);
        controller.jumpPower = controller.baseJumpPower;
    }

    public void ApplyItemEffect(ItemData item)
    {
        switch (item.effectType)
        {
            case ItemEffectType.Heal:
                Heal(item.effectValue);
                break;

            case ItemEffectType.Stamina:
                stamina.Add(item.effectValue);
                break;

            case ItemEffectType.SpeedBoost:
                if (speedCoroutine != null) StopCoroutine(speedCoroutine);
                speedCoroutine = StartCoroutine(TempSpeedBoost(2f, item.duration));
                break;

            case ItemEffectType.JumpBoost:
                if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
                jumpCoroutine = StartCoroutine(TempJumpBoost(2f, item.duration));
                break;

            case ItemEffectType.SpeedJumpBoost:
                if (speedCoroutine != null) StopCoroutine(speedCoroutine);
                speedCoroutine = StartCoroutine(TempSpeedBoost(2f, item.duration));

                if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
                jumpCoroutine = StartCoroutine(TempJumpBoost(2f, item.duration));
                break;
        }
    }
}

