using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelMastery : SingleTargetArtifact
{
	public DuelMastery() {
		this.Prefab = (GameObject)Resources.Load("VFX/BlackHole/BlackHole", typeof(GameObject));

		cost = 2;

		range = new AreaInfo(1, 1, AreaType.CIRCLE);

		maximumUsePerTurn = 1;
		cooldown = 0;

		size = new Vector2(1, 1);
		lootRate = 0.01f;

		targets.Add("Enemy");
	}

	public override void ApplyEffects(PlayerStats source, EntityStats target) {
		ActionManager.AddToBottom(new DamageAction(source, target, 30, 30));
		ActionManager.AddToBottom(new ArmorAction(source, 30));
	}
}
