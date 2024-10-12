using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPointEnemy : MonoBehaviour
{
	public static SpawnPointEnemy instanceEnemy;
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] List<GameObject> enemyList;
	[SerializeField] Transform spawnPoint;
	[SerializeField] List<Transform> spawnPointList;
	[SerializeField] int maxSpawn;
	private bool CanSpawn = true;

	private void Start()
	{
		for (int i = 0; i <= transform.GetComponentInChildren<Transform>().childCount; i++)
		{
			spawnPointList.Add(transform.GetComponentInChildren<Transform>());
		}
	}
	void Update()
	{

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponentInParent<Player>() != null && CanSpawn == true)
		{
			CanSpawn = false;
			
			enemyPrefab = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
			enemyList.Add(enemyPrefab);
			
		}
		

	}
}
