using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour {
	public static GridMovement instance;

	[Tooltip("The tilemap that the player can move on")]
	public TileBase markerTile;

	Vector2Int pos = Vector2Int.zero;
	
	void Awake() {
		instance = this;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3Int mouseCellPos = References.groundTilemap.layoutGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			
			if (References.markerTilemap.HasTile(mouseCellPos)) {
				pos = (Vector2Int)mouseCellPos;
				DiceSelector.instance.DisableCurrent();
				DiceSelector.instance.Select(null);
				ClearMarkers();

				Pickups.instance.TryPickupPos(mouseCellPos);
				if (Pickups.instance.TryDoorPos(mouseCellPos)) {
					GameOver.Win();
				}

				Spikes.instance.TrySpikePos(mouseCellPos);
			}
		}

		transform.position = References.groundTilemap.layoutGrid.CellToWorld(new Vector3Int(pos.x, pos.y)) + new Vector3(0.5f, 0.5f);
	}

    List<Vector2Int> GetMovePossibilities(int distance) {
		List<Vector2Int> output = new ();

		Vector2Int movedPos;
		
		// Left
		movedPos = new Vector2Int(pos.x - distance, pos.y);
		if (References.groundTilemap.HasTile(new Vector3Int(movedPos.x, movedPos.y))) {
			output.Add(movedPos);
		}
		 
		// Right
		movedPos = new Vector2Int(pos.x + distance, pos.y);
		if (References.groundTilemap.HasTile(new Vector3Int(movedPos.x, movedPos.y))) {
			output.Add(movedPos);
		}
		 
		// Up
		movedPos = new Vector2Int(pos.x, pos.y + distance);
		if (References.groundTilemap.HasTile(new Vector3Int(movedPos.x, movedPos.y))) {
			output.Add(movedPos);
		}
		 		
		// Down
		movedPos = new Vector2Int(pos.x, pos.y - distance);
		if (References.groundTilemap.HasTile(new Vector3Int(movedPos.x, movedPos.y))) {
			output.Add(movedPos);
		}
		 		
		return output;
	}

	public void VisualiseDice(Dice dice) {
		// Clear all markers
		ClearMarkers();

		// Display new markers
		var possibilities = GetMovePossibilities(dice.value);

		foreach (var possibility in possibilities) {
			References.markerTilemap.SetTile(new Vector3Int(possibility.x, possibility.y), markerTile);
		}
	}

	public void ClearMarkers() {
		References.markerTilemap.ClearAllTiles();
	}
}