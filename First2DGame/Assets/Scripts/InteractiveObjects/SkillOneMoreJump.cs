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
        CollectionManager.PickedUp(this);
        PlayerController.ExtraJumpIncrease();
        PlayerManager.PickSkill(this.gameObject);
        Dialog.ShowDialog("获得技能：空中跳跃",2);
    }
}
