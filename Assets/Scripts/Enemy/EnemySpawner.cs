using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject _enemyPrefab;
	
	[SerializeField]
	private int _maxEnemyCount;
	private int _currentEnemyCount;
	[SerializeField]
	private int _spawnDelay;
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(SpawnEnemies());
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	private IEnumerator SpawnEnemies()
	{
		while(true)
		{
			yield return new WaitForSeconds(_spawnDelay);
			while(_currentEnemyCount >= _maxEnemyCount)
			{
				yield return new WaitForEndOfFrame();
			}
			Instantiate(_enemyPrefab, transform);
			_currentEnemyCount++;
		}
	}
}
