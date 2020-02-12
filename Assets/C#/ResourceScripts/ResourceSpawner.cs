using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
	public GameObject[] resources;
	public float mapWidth;
	public float mapHeight;
	public float resourceSpacing;
	public int resourceAmt;
	private int retryLimit = 50;

	void Start()
	{
		Debug.Log("ResouceSpawner is running");

		for (int i = 0; i < resourceAmt; i += 2)
		{
			float topY = mapHeight / 2;
			float botY = mapHeight / 2 * -1;
			float righY = mapWidth / 2;
			float leftX = mapWidth / 2 * -1;


			//check to see if overlapping, if so then do it again
			bool overlapping = false;
			int retryTimes = 0;
			while (!overlapping && retryTimes <= retryLimit)
			{
				retryTimes++;
				float randomY = Random.Range(botY, topY);
				float randomX = Random.Range(-resourceSpacing, Mathf.Abs(randomY) + leftX); //y = x + leftX

				Vector3 spawnLocation = new Vector3(randomX, randomY, 0f);
				Vector3 mirrorLocation = new Vector3(-randomX, randomY, 0f);

				Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(randomX, randomY), resourceSpacing);
				if (hitColliders.Length == 0)
				{
					overlapping = true;

					//calculate which resource type to spawn based on it's y "altitude', 0-6 are resources, 0 being the cheapest (wood) and 6 being the best (diamond)
					float randomNum = Random.Range(0, randomY + mapHeight / 2);
					int resourceType = 0;
					if (randomNum < mapHeight / 6) 
					{
						if (Mathf.Floor(randomNum / 2) % 2 == 0)
						{
							resourceType = 0;
						} else
						{
							resourceType = 1;
						}
					} else if (randomNum < mapHeight / 3) 
					{
						if (Mathf.Floor(randomNum / 2) % 2 == 0)
						{
							resourceType = 2;
						}
						else
						{
							resourceType = 3;
						}
					} else 
					{
						if (Mathf.Floor(randomNum / 2) % 2 == 0)
						{
							resourceType = 4;
						}
						else
						{
							resourceType = 5;
						}
					}


					Instantiate(resources[resourceType], spawnLocation, Quaternion.identity);
					Instantiate(resources[resourceType], mirrorLocation, Quaternion.identity);
				}
				else
				{
					Debug.Log(hitColliders.Length);
					Debug.Log("Retrying...");
				}
			}

			if (retryTimes > retryLimit)
			{
				Debug.Log("Couldn't Spawn Object");
			}
		}
		//transform.rotation = new Quaternion(0f, 0f, 30f, 0f);
	}
}
