using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	List<GameObject> spawnedGems;
	public float minXCoord;
	public float maxXCoord;
	public float minYCoord;
	public float maxYCoord;

	public float spawnInterval;
	float previousSpawnInterval;
	public int spawnCount;
	int previousSpawnCount;
	float timer;	
		
	GameObject[] gemPrefabs;
	Material[] gemMaterials;
	
	public int maxSpawnCount;	
	
	public int spawnRateDifficultyRatio;
	public int spawnCountDifficultyRatio;
	
	public enum GemTypes {
		GEM_1,
		GEM_2,
		GEM_3,
		GEM_4
	}

	bool bActive;

	void Reset()
	{
		if (spawnedGems == null)
			return;

		foreach (GameObject gem in spawnedGems)
		{
			if (gem != null)
				Destroy(gem);
		}

		spawnedGems = new List<GameObject>();
	}

	public void Activate()
	{
		spawnedGems = new List<GameObject>();

		if(spawnRateDifficultyRatio == 0)
		{
			spawnRateDifficultyRatio = 1500;
		}
		
		if(spawnCountDifficultyRatio == 0)
		{
			spawnCountDifficultyRatio = 2000;
		}

		UnityEngine.Object[] tempArray = Resources.LoadAll(Paths.JEWELPATH);
		gemPrefabs = new GameObject[tempArray.Length];
		int index = 0;
		foreach(UnityEngine.Object obj in tempArray)
		{
			gemPrefabs[index] = obj as GameObject;
			index++;
		}
		tempArray = Resources.LoadAll(Paths.JEWELMATERIALS);
		gemMaterials = new Material[tempArray.Length];
		index = 0;
		foreach(UnityEngine.Object obj in tempArray)
		{
			gemMaterials[index] = obj as Material;
			index++;
		}
		minXCoord *= UIScaler.uiScale;
		maxXCoord *= UIScaler.uiScale;
		minYCoord *= UIScaler.uiScale;
		maxYCoord *= UIScaler.uiScale;
		bActive = true;
		spawnCount = 1;
		previousSpawnCount = 1;
		spawnInterval = 2.0f;
		previousSpawnInterval = 2.0f;
		timer = 0.0f;
	}

	public void Deactivate()
	{
		bActive = false;
		Reset();
	}

	// Update is called once per frame
	void Update () {
		if (!bActive)
			return;

		timer += Time.deltaTime;

		if (timer >= spawnInterval)
		{
			if (spawnCount < maxSpawnCount)
			{				
				spawnCount = 1 + GameManager.Manager.points / spawnCountDifficultyRatio;
				if (spawnCount != previousSpawnCount)
					Announcer.Instance.ShowMessage("Level UP!");
				previousSpawnCount = spawnCount;
			}

			for (int i = 0; i < spawnCount; i++)
			{
				Spawn();
			}
			timer = 0.0f;
			if (spawnInterval > 0.1f)
			{
				spawnInterval = 2.0f - GameManager.Manager.points / spawnRateDifficultyRatio * 0.05f;
				if (spawnInterval != previousSpawnInterval)
					Announcer.Instance.ShowMessage("Level UP!");
				previousSpawnInterval = spawnInterval;
			}
			if (spawnInterval < 0.1f)
			{
				spawnInterval = 0.1f;
			}
		}
	}
	
	void Spawn() {		
		GameObject tempObject = null;
		int type;
		int color = 0;
		Vector3 spawnPos;
		float xCoord, yCoord;
		Collider[] colliders;
		
		type = UnityEngine.Random.Range(0, Enum.GetNames(typeof(GemTypes)).Length);
		color = UnityEngine.Random.Range(0, Enum.GetNames(typeof(GameSettings.Colors)).Length);
				
		xCoord = UnityEngine.Random.Range(minXCoord, maxXCoord);
		yCoord = UnityEngine.Random.Range(minYCoord, maxYCoord);
		
		spawnPos = new Vector3(xCoord, 2.0f, yCoord);
		colliders = Physics.OverlapSphere(spawnPos, 1.2f);
				
		if(colliders.Length > 0)
		{
			return;
		}
		
		spawnPos.y = 1 * UIScaler.uiScale;
		
		tempObject = GameObject.Instantiate(gemPrefabs[type]) as GameObject;
		spawnedGems.Add(tempObject);
		tempObject.transform.position = spawnPos;
		if(tempObject != null)
		{
			PickUpGem gemComponent = tempObject.GetComponent<PickUpGem>();
			if(gemComponent != null)
			{
				if (GameManager.Manager.currentState == GameManager.GameState.CLASSICMODE)
				{
					if((GameSettings.Colors)color == GameSettings.Colors.COLOR_GREEN
						|| (GameSettings.Colors)color == GameSettings.Colors.COLOR_PURPLE
						|| (GameSettings.Colors)color == GameSettings.Colors.COLOR_RED
						|| (GameSettings.Colors)color == GameSettings.Colors.COLOR_ORANGE)
					{
						color = (int)GameSettings.Colors.COLOR_WHITE;
					}
				}
				gemComponent.gemColor = (GameSettings.Colors)color;
				gemComponent.bTimedValue = true;
			}
			MeshRenderer gemRender = tempObject.GetComponent<MeshRenderer>();
			if(gemRender != null)
				gemRender.sharedMaterial = gemMaterials[color];
		}
	}
}