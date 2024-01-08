using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EnemyStats
{
	public int hp = 999;
	public int damage = 1;
	public int experience_reward = 400;
	public float moveSpeed = 1f;

	public EnemyStats(EnemyStats stats)
	{
		this.hp = stats.hp;
		this.damage = stats.damage;
		this.experience_reward = stats.experience_reward;
		this.moveSpeed = stats.moveSpeed;
	}

	internal void ApplyProgress(float progress)
	{
		this.hp = (int)(hp * progress);
		this.damage = (int)(damage * progress);
	}
}

public class Enemy : MonoBehaviour, IDamageable
{
	Transform targetDestination;
	GameObject targetGameObject;
	Character targetCharacter;

	Rigidbody2D rigidbody2D;

	public EnemyStats stats;

	public float shrinkTime = 1.0f;
	public float shrinkStartDelay = 0.5f;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	public void SetTarget(GameObject target)
	{
		targetGameObject = target;
		targetDestination = target.transform;
	}

	private void FixedUpdate()
	{
		Vector3 direction = (targetDestination.position - transform.position).normalized;
		rigidbody2D.velocity = direction * stats.moveSpeed;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject == targetGameObject)
		{
			Attack();
		}
	}

	private void Attack()
	{
		if (targetCharacter == null)
		{
			targetCharacter = targetGameObject.GetComponent<Character>();
		}

		targetCharacter.TakeDamage(stats.damage);
	}

	public void TakeDamage(int damage)
	{
		stats.hp -= damage;

		if (stats.hp < 1)
		{
			targetGameObject.GetComponent<Level>().AddExperience(stats.experience_reward);
			GetComponent<DropOnDestroy>().CheckDrop();
			StartCoroutine(ShrinkOverTime());
		}
	}

	internal void SetStats(EnemyStats stats)
	{
		this.stats = new EnemyStats(stats);
	}

	internal void UpdateStatsForProgress(float progress)
	{
		stats.ApplyProgress(progress);
	}

	IEnumerator ShrinkOverTime()
	{
		yield return new WaitForSeconds(shrinkStartDelay);

		float progress = 1.0f;
		float shrinkSpeed = 1.0f / shrinkTime;

		while (progress > 0.0f)
		{
			progress -= Time.deltaTime * shrinkSpeed;
			transform.localScale = new Vector3(progress, progress, progress);
			yield return null;
		}

		Destroy(gameObject);
	}
}
