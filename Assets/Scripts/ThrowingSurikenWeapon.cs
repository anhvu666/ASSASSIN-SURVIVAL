using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingSurikenWeapon : WeaponBase
{
	[SerializeField] GameObject surikenPrefab;
	[SerializeField] float spread = 0.5f;

	public override void Attack()
	{
		UpdateVectorOfAttack();
		for (int i = 0; i < weaponStats.numberOfAttacks; i++)
		{
			GameObject thrownSuriken = Instantiate(surikenPrefab);

			Vector3 newSurikenPosition = transform.position;
			if (weaponStats.numberOfAttacks > 1)
			{
				newSurikenPosition.y -= (spread * (weaponStats.numberOfAttacks - 1)) / 2;   //calculating offset
				newSurikenPosition.y += i * spread; //spreading the knives along the line 
			}

			thrownSuriken.transform.position = newSurikenPosition;

			ThrowingSurikenProjectile throwingSurikenProjectile = thrownSuriken.GetComponent<ThrowingSurikenProjectile>();
			throwingSurikenProjectile.SetDirection(vectorOfAttack.x, vectorOfAttack.y);
			throwingSurikenProjectile.damage = GetDamage();
		}
	}
}
