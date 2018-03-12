using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour {

	public RectTransform healthbar;

	[SerializeField]
	private float maxSize;
	// Use this for initialization
	void Start () {
		maxSize = healthbar.sizeDelta.x;
		GameManager.instance.OnHeroRespawn += (Hero h) => {
			Damageable player = h.GetComponentInChildren<Damageable>();
			player.OnDamaged += UpdateHealthbar;
			UpdateHealthbar(player, 0);
		};


	}

	void UpdateHealthbar(Damageable self, int amount)  {
		healthbar.sizeDelta = new Vector2 (maxSize * self.HealthRatio, healthbar.sizeDelta.y);
	}

}
