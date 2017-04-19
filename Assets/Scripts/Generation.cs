using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour {

	public GameObject straightTile;
	public GameObject curvedTile;
	public GameObject coinPrefab;

	private int verticalLocation;
	private int lastModuleOutPosition;

	private GameObject[] tileList = new GameObject[12];
	private GameObject[] coinList = new GameObject[4];
	private Module[] moduleList = new Module[3];

	public class Tile
	{
		public int tileType;
		public int tileRotation;

		public Tile (int tileTypeInput, int tileRotationInput) {
			tileType = tileTypeInput;
			tileRotation = tileRotationInput;
		}
	}

	public class Coin
	{
		public Vector2 position;

		public Coin (float xInput, float yInput) {
			position = new Vector2 (xInput, yInput);
		}
	}

	public class Module
	{
		public Tile[][] tileGrid = 
		{
			new Tile[4],
			new Tile[4],
			new Tile[4]
		};

		public int outPosition;

		public Coin[] coinArray = new Coin[4];

		public Module (int[,,] tileInputArray, int outPos, float[,] coinInputArray) {
			for (int j = 0; j < tileInputArray.GetLength(0); j ++) {
				for (int k = 0; k < tileInputArray.GetLength(1); k ++) {
					tileGrid [j][k] = new Tile (tileInputArray[j,k,0], tileInputArray[j,k,1]);
				}
			}

			outPosition = outPos;

			for (int j = 0; j < coinInputArray.GetLength(0); j ++) {
				coinArray [j] = new Coin (coinInputArray[j,0], coinInputArray[j,1]);
			}
		}
	}
		
	void Start () {
		lastModuleOutPosition = 0;

		Module easy1 = new Module(new int[,,] {{{1, 180}, {0, 0}, {1, 90}, {1, 0}}, {{0, 0}, {1, 90}, {1, 180}, {0, 90}}, {{0, 0}, {1, 0}, {1, 90}, {1, 180}}}, 3, new float[,] {{1, 0}, {1, -1.3f}, {2, -0.7f}, {3.3f, -2}});
		Module easy2 = new Module(new int[,,] {{{0, 90}, {1, 90}, {0, 0}, {1, 0}}, {{1, -90}, {0, 0}, {0, 90}, {0, 0}}, {{1, 90}, {1, 90}, {1, 0}, {0, 0}}}, 1, new float[,] {{0, 0}, {1, -1}, {3, -1}, {0.7f, -2}});
		Module hard1 = new Module(new int[,,] {{{1, 180}, {1, 0}, {0, 0}, {1, 90}}, {{1, 0}, {1, 90}, {1, 180}, {0, 0}}, {{1, -90}, {0, 90}, {1, 0}, {1, 90}}}, 2, new float[,] {{1, -0.3f}, {1.3f, -1}, {1, -2}, {3, -1}});

		moduleList [0] = easy1;
		moduleList [1] = easy2;
		moduleList [2] = hard1;

		generateModule (moduleList[Random.Range(0, moduleList.Length)]);

		generateModule (moduleList[Random.Range(0, moduleList.Length)]);
	}

	void generateModule (Module module) {
		for (int j = 0; j < module.tileGrid.Length; j ++) {
			for (int k = 0; k < module.tileGrid[j].Length; k ++) {
				int xPosition = k + lastModuleOutPosition;

				//tileList[j * 4 + k] = 
				Instantiate (module.tileGrid[j][k].tileType == 0 ? straightTile : curvedTile, new Vector2 (xPosition < 4 ? xPosition : xPosition - 4, -j - verticalLocation * 3), Quaternion.Euler (0, 0, module.tileGrid[j][k].tileRotation));
			}
		}

		for (int j = 0; j < module.coinArray.GetLength(0); j ++) {
			float xPosition = module.coinArray[j].position.x + lastModuleOutPosition;

			//coinList[j] =
			Instantiate (coinPrefab, new Vector2 (xPosition < 4 ? xPosition : xPosition - 4, module.coinArray[j].position.y - verticalLocation * 3), Quaternion.Euler (0, 0, 0));


		}

		lastModuleOutPosition = module.outPosition;

		verticalLocation++;
	}

	void OnTriggerEnter2D () {
		Debug.Log ("yes");

		generateModule (moduleList[Random.Range(0, moduleList.Length)]);

		gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, gameObject.GetComponent<BoxCollider2D> ().offset.y - 3);
	}
}
