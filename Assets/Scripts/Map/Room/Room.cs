using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerDeploy))]
public class Room : MonoBehaviour
{
    public static Room currentRoom;

    public UnityEvent<Tile> newTileHovered = new UnityEvent<Tile>();
    public UnityEvent<Tile> tileClicked = new UnityEvent<Tile>();

    public Tile hoveredTile;
    private Tile previousHoveredTile;

    private TurnSystem turnSystem;

	private void Awake() {
        currentRoom = this;
        turnSystem = GameObject.Find("Gameplay").GetComponent<TurnSystem>();
	}

    /// <summary>
    /// Disables this room's exits depending on the parameters
    /// </summary>
    /// <param name="hasNorthExit"></param>
    /// <param name="hasSouthExit"></param>
    /// <param name="hasWestExit"></param>
    /// <param name="hasEastExit"></param>
    public void SetExits(bool hasNorthExit, bool hasSouthExit, bool hasWestExit, bool hasEastExit) {
        foreach (TransitionTile transitionTile in GetComponentsInChildren<TransitionTile>()) {
            if (transitionTile.direction == Direction.NORTH && !hasNorthExit ||
                    transitionTile.direction == Direction.SOUTH && !hasSouthExit ||
                    transitionTile.direction == Direction.EAST && !hasEastExit ||
                    transitionTile.direction == Direction.WEST && !hasWestExit) {
                transitionTile.tag = "Tile";
                DestroyImmediate(transitionTile);
            }
		}
	}

    /// <summary>
    /// Initializes this room. 
    /// Registers the player and the enemies in the turn system, starts the deploy phase, then starts combat or exploration
    /// </summary>
    /// <param name="layoutIndex">The layout index to be used. If not set or set to -1, does not load any spawnLayout</param>
    /// <returns></returns>
    public IEnumerator Init(int layoutIndex = -1) {
        turnSystem.Clear();

        turnSystem.RegisterPlayer(FindObjectOfType<PlayerTurn>());

        if (layoutIndex != -1) {
            List<GameObject> spawnLayouts = new List<GameObject>();
            foreach (Transform spawnLayout in transform.Find("SpawnLayouts")) {
                spawnLayouts.Add(spawnLayout.gameObject);
            }
            GameObject chosenSpawnLayout = spawnLayouts[layoutIndex];

            foreach (SpawnPoint spawnPoint in chosenSpawnLayout.GetComponentsInChildren<SpawnPoint>()) {
                turnSystem.RegisterEnemy(spawnPoint.SpawnEntity());
            }
        }

        yield return GetComponent<PlayerDeploy>().DeployPlayer(FindObjectOfType<PlayerTurn>().transform);

        turnSystem.CheckForCombatStart();
    }

	/// <summary>
    /// Checks if a tile is hovered or clicked, and calls the relevant functions.
    /// </summary>
	void Update()
    {
        previousHoveredTile = hoveredTile;
        hoveredTile = Tile.GetHoveredTile();
        if (hoveredTile != previousHoveredTile) {
            newTileHovered.Invoke(hoveredTile);
        }
        if (Input.GetMouseButtonDown(0) && hoveredTile != null && hoveredTile.selectionType != Tile.SelectionType.NONE && !ActionManager.IsBusy) {
            tileClicked.Invoke(hoveredTile);
        }
    }
}