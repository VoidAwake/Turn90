using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsGeneration : MonoBehaviour {

	public GameObject straightTile;
	public GameObject curvedTile;
	public GameObject coinPrefab;

	private static Level[] levelList = new Level[6];

	void OnEnable () {
		levelList [0] = new Level(1, new int[,,] {{{0, 0}, {0, 0}, {0, 0}, {0, 0}}, {{0, 0}, {1, 90}, {1, 180}, {0, 0}}, {{0, 0}, {0, 0}, {0, 0}, {0, 0}}, {{0, 0}, {1, 0}, {1, 90}, {0, 0}}, {{0, 0}, {0, 0}, {0, 0}, {0, 0}}}, new float[,] {{2, -4}});
		levelList [1] = new Level(1, new int[,,] {{{0, 90}, {0, 0}, {0, 0}, {0, 90}}, {{0, 90}, {1, 0}, {1, 90}, {0, 90}}, {{1, 0}, {1, 90}, {1, 0}, {1, 90}}, {{1, 0}, {1, 0}, {1, 90}, {1, 90}}, {{0, 0}, {0, 90}, {0, 90}, {0, 0}}}, new float[,] {{1, -2.7f}, {2, -2.7f}, {2, -4.3f}});
		levelList [2] = new Level(0, new int[,,] {{{1, 90}, {1, -90}, {1, 0}, {0, 90}}, {{1, 0}, {1, 90}, {0, 0}, {1, 90}}, {{1, 180}, {1, 0}, {1, 180}, {1, 0}}, {{0, 90}, {1, 90}, {0, 0}, {1, -90}}, {{0, 90}, {1, 180}, {1, 0}, {1, 0}}}, new float[,] {{0, -0.7f}, {2, -1.7f}, {1, -2.3f}, {3, -3.7f}, {3, -4.3f}});
		levelList [3] = new Level(3, new int[,,] {{{0, 90}, {1, 180}, {1, -90}, {1, 90}}, {{0, 90}, {1, 180}, {0, 0}, {0, 90}}, {{0, 90}, {1, 0}, {0, 0}, {1, 0}}, {{1, 180}, {0, 0}, {1, 180}, {0, 0}}, {{1, 0}, {1, -90}, {1, 90}, {0, 90}}}, new float[,] {{0.7f, 0}, {3, -1}, {0, -1}, {0, -3.3f}, {1, -3}});
		levelList [4] = new Level(1, new int[,,] {{{0, 90}, {0, 0}, {1, -90}, {0, 90}}, {{1, 180}, {1, 0}, {1, 90}, {0, 90}}, {{1, 0}, {0, 0}, {1, 180}, {0, 90}}, {{0, 0}, {1, 0}, {1, 0}, {0, 0}}, {{1, 0}, {0, 90}, {0, 90}, {0, 90}}}, new float[,] {{2, -0.3f}, {1, -0.7f}, {2.7f, -1}, {3, -2}, {2, -4}});
		levelList [5] = new Level(1, new int[,,] {{{1, 0}, {1, 90}, {0, 90}, {1, -90}}, {{1, 0}, {1, -90}, {0, 90}, {1, 90}}, {{0, 0}, {1, -90}, {0, 0}, {1, 0}}, {{1, -90}, {0, 90}, {0, 0}, {1, 90}}, {{1, 0}, {1, 180}, {0, 0}, {1, 180}}}, new float[,] {{3, -0.3f}, {3, -1.3f}, {1, -1.7f}, {-0.3f, -3}, {0.3f, -3}, {2, -4}});
	}

	public int[] generateLevel (int levelId) {
		Level level = levelList [levelId];

		for (int j = 0; j < 5; j ++) {
			for (int k = 0; k < 4; k ++) {
				Instantiate (level.tileGrid[j][k].tileType == 0 ? straightTile : curvedTile, new Vector2 (k, -j), Quaternion.Euler (0, 0, level.tileGrid[j][k].tileRotation));
			}
		}

		for (int j = 0; j < level.coinArray.Count; j ++) {
			Instantiate (coinPrefab, new Vector3 (level.coinArray[j].position.x, level.coinArray[j].position.y, 3), Quaternion.Euler (0, 0, 0));
		}

		return new int[] {level.coinArray.Count, level.inPosition};
	}
}

class Level
{
	public int inPosition;

	public Tile[][] tileGrid = 
	{
		new Tile[4],
		new Tile[4],
		new Tile[4],
		new Tile[4],
		new Tile[4]
	};

	public List<Coin> coinArray = new List<Coin>();

	public Level (int ballStartPosition, int[,,] tileInputArray, float[,] coinInputArray) {
		inPosition = ballStartPosition;

		for (int j = 0; j < tileInputArray.GetLength(0); j ++) {
			for (int k = 0; k < tileInputArray.GetLength(1); k ++) {
				tileGrid [j][k] = new Tile (tileInputArray[j,k,0], tileInputArray[j,k,1]);
			}
		}

		for (int j = 0; j < coinInputArray.GetLength(0); j ++) {
			coinArray.Add(new Coin (coinInputArray[j,0], coinInputArray[j,1]));
		}
	}
}

class Tile
{
	public int tileType;
	public int tileRotation;

	public Tile (int tileTypeInput, int tileRotationInput) {
		tileType = tileTypeInput;
		tileRotation = tileRotationInput;
	}
}