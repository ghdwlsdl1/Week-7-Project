using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition expendables {get{ return uiCondition.expendables; }}

    Condition stamina {get{ return uiCondition.stamina; }}
    Condition health {get{ return uiCondition.health; }}


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
}
