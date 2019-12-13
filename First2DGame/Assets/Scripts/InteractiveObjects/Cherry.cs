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
        CollectionManager.PickedUp(this);
        PlayerController.PickCherry(this.gameObject);
    }
}
