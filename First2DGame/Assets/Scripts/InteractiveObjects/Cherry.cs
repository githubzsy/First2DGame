using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : InteractiveBase
{
    protected override void PlayerInteractive()
    {

    }

    protected override void OnTriggerEnter2DAfter(Collider2D collision)
    {
        PlayerController.PickCherry(this.gameObject);
    }
}
