using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUpgradeUIElement : MonoBehaviour
{
	[SerializeField] PlayerPersistenUpgrades upgrade;

	[SerializeField] TextMeshProUGUI upgradeName;
	[SerializeField] TextMeshProUGUI level;
	[SerializeField] TextMeshProUGUI price;

	[SerializeField] DataContainer dataContainer;

	private void Start()
	{
		UpdateElement();
	}

	public void Upgrade()
	{
		PlayerUpgrades playerUpgrades = dataContainer.upgrades[(int)upgrade];

		if (playerUpgrades.level >= playerUpgrades.max_level) { return; }
		if (dataContainer.coins >= playerUpgrades.costToUpgrade)
		{
			dataContainer.coins -= playerUpgrades.costToUpgrade;
			playerUpgrades.level += 1;
			UpdateElement();
		}
	}

	void UpdateElement()
	{
		PlayerUpgrades playerUpgrades = dataContainer.upgrades[(int)upgrade];
		upgradeName.text = upgrade.ToString();
		level.text = playerUpgrades.level.ToString();
		price.text = playerUpgrades.costToUpgrade.ToString();
	}
}
