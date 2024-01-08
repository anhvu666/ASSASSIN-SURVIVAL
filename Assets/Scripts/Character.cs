using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public int maxHp = 1000;
	public int currentHp = 1000;

	public int armor = 0;

	public float hpRegenerationRate = 1f;
	public float hpRegenerationTimer;

	public float damageBonus;

	private int xPressCount = 0;
	private float xPressTime = 0f;
	public float timeBetweenPresses = 0.5f;

	[SerializeField] StatusBar hpBar;

	[HideInInspector] public Level level;
	[HideInInspector] public Coins coins;
	private bool isDead;

	[SerializeField] DataContainer dataContainer;

	private void Awake()
	{
		level = GetComponent<Level>();
		coins = GetComponent<Coins>();
	}

	private void Start()
	{
		ApplyPeristantUpgrades();

		hpBar.SetState(currentHp, maxHp);
	}

	private void ApplyPeristantUpgrades()
	{
		int hpUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistenUpgrades.HP);

		maxHp += maxHp / 10 * hpUpgradeLevel;

		int damageUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistenUpgrades.Damage);

		damageBonus = 1f + 0.1f * damageUpgradeLevel;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			float currentTime = Time.time;
			if ((currentTime - xPressTime) < timeBetweenPresses)
			{
				xPressCount++;
				if (xPressCount >= 3)
				{
					
					Heal(1000);
					xPressCount = 0;
				}
			}
			else
			{
				xPressCount = 1;
			}
			xPressTime = currentTime;
		}
		/*hpRegenerationTimer += Time.deltaTime * hpRegenerationRate;

		if (hpRegenerationTimer > 1f)
		{
			Heal(1);
			hpRegenerationTimer -= 1f;
		}*/
	}

	public void TakeDamage(int damage)
	{
		if (isDead == true) { return; }
		ApplyArmor(ref damage);

		currentHp -= damage;

		if (currentHp <= 0)
		{
			GetComponent<CharacterGameOver>().GameOver();
			isDead = true;
		}
		hpBar.SetState(currentHp, maxHp);
	}

	private void ApplyArmor(ref int damage)
	{
		damage -= armor;
		if (damage < 0) { damage = 0; }
	}

	public void Heal(int amount)
	{
		if (currentHp <= 0) { return; }

		currentHp += amount;
		if (currentHp > maxHp)
		{
			currentHp = maxHp;
		}
		hpBar.SetState(currentHp, maxHp);
	}
}
