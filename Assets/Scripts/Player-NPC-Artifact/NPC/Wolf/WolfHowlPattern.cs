using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfHowlPattern : EnemyPattern
{
	public WolfHowlPattern() {
		range = new CircleTileSearch(4, 7);
	}
	public override void Use(EntityStats source, EntityStats target) {
		ActionManager.AddToBottom(new DebugAction(source, target));
		Debug.Log("USING HOWL");
	}
}
