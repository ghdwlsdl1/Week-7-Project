using System;
using UnityEngine;
public interface IDamagalbe
{
    void TakePhysicaIDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UICondition uiCondition;

    Condition expendables {get{ return uiCondition.expendables; }}

    Condition stamina {get{ return uiCondition.stamina; }}
    Condition health {get{ return uiCondition.health; }}

    public event Action onTakeDamage;
    void Update()
    {
        expendables.Subtract(expendables.passiveValue*Time.deltaTime);
        stamina.Add(stamina.passiveValue*Time.deltaTime);

        if (expendables.curValue == 0f)
        {
            if (uiCondition.expendablesUI.activeSelf)
            {
                uiCondition.expendablesUI.SetActive(false);
            }
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }
    public void Heal(float amout)
    {
        health.Add(amout);
    }

    public void Eat(float amout)
    {
        expendables.Add(amout);
    }

    public void Die()
    {
        Debug.Log("사망");
    }

    public void TakePhysicaIDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
}
