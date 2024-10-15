using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTrigger : EnemyAnimationTrigger
{
    private DeathBringer enemy => GetComponentInParent<DeathBringer>();
    private void Relocate() => enemy.FindPosition();

    private void MakeInvisible() => enemy.fx.MakeTransprent(true);
    private void MakeVisible() => enemy.fx.MakeTransprent(false);
}
