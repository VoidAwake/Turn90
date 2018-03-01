using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteGeneration : MonoBehaviour {

	public GameObject straightTile;
	public GameObject curvedTile;
	public GameObject coinPrefab;

	private int verticalLocation;
	private int lastModuleOutPosition;

	private int[] rotations = new int[4] {0, 90, 180, -90};

	private Module[] moduleList = new Module[3];

	private Module module; // Module to be generated, randomised by generateModule

	private GameObject[] tileBox = new GameObject[72];
	private GameObject[] coinBox = new GameObject[24];
		
	void OnEnable () {
		lastModuleOutPosition = 0;

		moduleList [0] = new Module(new int[,,] {{{1, 0}, {0, 0}, {1, 3}, {1, 0}}, {{0, 0}, {1, 0}, {1, 1}, {0, 0}}, {{0, 0}, {1, 0}, {1, 0}, {1, 0}}}, 3, new float[,] {{1, 0}, {1, -1.3f}, {2, -0.7f}, {3.3f, -2}});
		moduleList [1] = new Module(new int[,,] {{{0, 0}, {1, 0}, {0, 0}, {1, 0}}, {{1, 0}, {0, 0}, {0, 0}, {0, 0}}, {{1, 0}, {1, 0}, {1, 0}, {0, 0}}}, 1, new float[,] {{0, 0}, {1, -1}, {3, -1}, {0.7f, -2}});
		moduleList [2] = new Module(new int[,,] {{{1, 0}, {1, 0}, {0, 0}, {1, 0}}, {{1, 0}, {1, 0}, {1, 0}, {0, 0}}, {{1, 0}, {0, 0}, {1, 0}, {1, 0}}}, 2, new float[,] {{1, -0.3f}, {1.3f, -1}, {1, -2}, {3, -1}});

		generateModule (true, 0);
		generateModule (false, -1);
	}

	public void generateModule (bool setRotation, int moduleId) {
		for (int i = 0; i < 12; i++) {
			Destroy (tileBox [i]);
		}

		for (int i = 0; i < 60; i++) {
			tileBox [i] = tileBox[12 + i];
		}

		for (int i = 0; i < 4; i++) {
			Destroy (coinBox [i]);
		}
			
		for (int i = 0; i < 20; i++) {
			coinBox [i] = coinBox[4 + i];
		}

		if (moduleId == -1) {
			module = moduleList [Random.Range (0, moduleList.Length)];
		} else {
			module = moduleList [moduleId];
		}

		for (int j = 0; j < module.tileGrid.Length; j ++) {
			for (int k = 0; k < module.tileGrid[j].Length; k ++) {
				int xPosition = k + lastModuleOutPosition;
				if (setRotation) {
					tileBox[60 + j * 4 + k] = Instantiate (module.tileGrid[j][k].tileType == 0 ? straightTile : curvedTile, new Vector2 (xPosition < 4 ? xPosition : xPosition - 4, -j - verticalLocation * 3), Quaternion.Euler (0, 0, rotations[module.tileGrid[j][k].tileRotation]));
				} else {
					tileBox[60 + j * 4 + k] = Instantiate (module.tileGrid[j][k].tileType == 0 ? straightTile : curvedTile, new Vector2 (xPosition < 4 ? xPosition : xPosition - 4, -j - verticalLocation * 3), Quaternion.Euler (0, 0, rotations[Random.Range(0, 4)]));
				}
			}
		}

		for (int j = 0; j < module.coinArray.GetLength(0); j ++) {
			float xPosition = module.coinArray[j].position.x + lastModuleOutPosition;

			coinBox[20 + j] = Instantiate (coinPrefab, new Vector3 (xPosition < 4 ? xPosition : xPosition - 4, module.coinArray[j].position.y - verticalLocation * 3, 3), Quaternion.Euler (0, 0, 0));
		}

		lastModuleOutPosition = module.outPosition;

		verticalLocation++;
	}
}

public class Module
{
	public ModuleTile[][] tileGrid = 
	{
		new ModuleTile[4],
		new ModuleTile[4],
		new ModuleTile[4]
	};

	public int outPosition;

	public Coin[] coinArray = new Coin[4];

	public Module (int[,,] tileInputArray, int outPos, float[,] coinInputArray) {
		for (int j = 0; j < tileInputArray.GetLength(0); j ++) {
			for (int k = 0; k < tileInputArray.GetLength(1); k ++) {
				tileGrid [j][k] = new ModuleTile (tileInputArray[j,k,0], tileInputArray[j,k,1]);
			}
		}

		outPosition = outPos;

		for (int j = 0; j < coinInputArray.GetLength(0); j ++) {
			coinArray [j] = new Coin (coinInputArray[j,0], coinInputArray[j,1]);
		}
	}
}

public class ModuleTile
{
	public int tileType;
	public int tileRotation;

	public ModuleTile (int tileTypeInput, int tileRotationInput) {
		tileType = tileTypeInput;
		tileRotation = tileRotationInput;
	}
}