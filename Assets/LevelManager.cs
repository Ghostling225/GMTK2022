using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour {
	public Grid[] levels;
	public int[] pickupCounts;
	public Tilemap markerTilemap;

	public static int id;

	void Awake() {
		References.markerTilemap = markerTilemap;

		id = PlayerPrefs.GetInt("levelID", 0);
		LoadLevel(id);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			id++;
			LoadLevel(id);
		}
	}

	void LoadLevel(int id) {
		print("loadlevel");

		foreach (var level in levels) {
			level.gameObject.SetActive(false);
		}
		levels[id].gameObject.SetActive(true);
		var tilemaps = levels[id].GetComponentsInChildren<Tilemap>();

		foreach (var tilemap in tilemaps) {
			if (tilemap.gameObject.name == "Door") References.doorTilemap = tilemap;
			else if (tilemap.gameObject.name == "Pickups") References.pickupTilemap = tilemap;
			else if (tilemap.gameObject.name == "Ground") References.groundTilemap = tilemap;
			else if (tilemap.gameObject.name == "Spikes") References.spikeTilemap = tilemap;
		}

		Pickups.collected = 0;
		Pickups.total = pickupCounts[id];
	}
}