using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	private List<PatrolPoint> _patrolPoints; 
	// Start is called before the first frame update
	void Start()
	{
		_patrolPoints = GetComponentsInChildren<PatrolPoint>().ToList();
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
			_enemyPrefab.SetActive(false);
			var enemy = Instantiate(_enemyPrefab, transform);
			enemy.transform.position = transform.position;
			var enemyComp = enemy.GetComponent<Enemy>();
			enemyComp.PatrolState.anchor1 = _patrolPoints[0].transform;
			enemyComp.PatrolState.anchor2 = _patrolPoints[1].transform;
			_currentEnemyCount++;
			enemy.SetActive(true);
		}
	}
}
