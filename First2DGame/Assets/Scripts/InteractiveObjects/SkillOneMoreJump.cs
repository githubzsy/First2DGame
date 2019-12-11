using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空中多跳一次的技能
/// </summary>
public class SkillOneMoreJump : InteractiveBase
{
    protected override void PlayerInteractive()
    {

    }

    protected override void OnTriggerEnter2DAfter(Collider2D playerCollision)
    {
        PlayerController.PlayerAttributes.ExtraJumpCount++;
        PlayerController.GetSkill(this.gameObject);
    }
}
