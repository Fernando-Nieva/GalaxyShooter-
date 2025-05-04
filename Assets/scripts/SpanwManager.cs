using System.Collections;
using UnityEngine;

public class SpanwManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
	private GameObject[] powerUps;

	void Start()
    {
       
		StartCoroutine(EnemySpawnRoutine());
		StartCoroutine(PowerUpSpawnRoutine());
	}

	IEnumerator EnemySpawnRoutine()
	{
		while (true)
		{
		
			Instantiate(enemyShipPrefab,new Vector3(Random.Range(-7f,7f),7,0), Quaternion.identity);
			yield return new WaitForSeconds(5.0f);
		}
	}


	IEnumerator PowerUpSpawnRoutine()
	{
		while (true)
		{
			int randomPowerUp = Random.Range(0,3);
			Instantiate(powerUps[randomPowerUp], new Vector3(Random.Range(-7f, 7f), 7, 0), Quaternion.identity);
			yield return new WaitForSeconds(5.0f);
		}
	}


}
