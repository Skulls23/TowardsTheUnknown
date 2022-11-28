using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private List<List<RoomInfo>> rooms = new List<List<RoomInfo>>();
    private GameObject player;
    private GameObject ui;

    private Room currentRoom = null;
    private Vector2Int currentRoomPosition = Vector2Int.zero;

	private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        ui = GameObject.FindGameObjectWithTag("UI");

        Debug.Log("GENERATING");
        rooms.Add(new List<RoomInfo>());
        rooms[0].Add(new RoomInfo(Resources.Load<Room>("Prefabs/Maps/Map2_Codir2"), 0));
        rooms[0].Add(new RoomInfo(Resources.Load<Room>("Prefabs/Maps/Map2_Codir2"), 1));
    }

	// Start is called before the first frame update
	void Start()
    {
        StartCoroutine(LoadFirstRoom());
    }

    /// <summary>
    /// Loads the first room. 
    /// TODO : remove a probable crash if the room does not contain combat and does not have a north exit
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadFirstRoom() {
        yield return LoadRoom(currentRoomPosition, Direction.NORTH);
        player.GetComponent<PlayerMove>().isMapTransitioning = false;
        FindObjectOfType<TurnSystem>().CheckForCombatStart();
    }

    /// <summary>
    /// Loads the room at <paramref name="pos"/> and deploys the player
    /// </summary>
    /// <param name="pos">The room's position in the map</param>
    /// <param name="fromDirection">The direction from which the player entered the room</param>
    /// <returns></returns>
    private IEnumerator LoadRoom(Vector2Int pos, Direction fromDirection) {
        currentRoom = rooms[pos.x][pos.y].LoadRoom(RoomExists(pos + Vector2Int.up), RoomExists(pos + Vector2Int.down), RoomExists(pos + Vector2Int.left), RoomExists(pos + Vector2Int.right));
        yield return currentRoom.GetComponent<PlayerDeploy>().DeployPlayer(FindObjectOfType<PlayerTurn>().transform, fromDirection);
    }

    /// <summary>
    /// Checks if the room at <paramref name="pos"/> exists
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool RoomExists(Vector2Int pos) {
        if (pos.x < 0 || pos.y < 0) return false;
        return rooms.Count > pos.x && rooms[pos.x].Count > pos.y && rooms[pos.x][pos.y] != null;
	}

    /// <summary>
    /// Moves the player to the adjacent room in the given direction
    /// </summary>
    /// <param name="direction"></param>
    public void MoveToAdjacentRoom(Direction direction) {
        StartCoroutine(MoveMapOnSide(direction));
    }

    /// <summary>
    /// Destroys the current room and loads the one on the chosen side.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator MoveMapOnSide(Direction direction) {
        currentRoom.enabled = false;

        yield return ui.GetComponent<UIFade>().FadeEnum(true);

        Destroy(currentRoom.gameObject);
        yield return new WaitForEndOfFrame();

        currentRoomPosition += DirectionConverter.DirToVect(direction);
        yield return LoadRoom(currentRoomPosition, DirectionConverter.GetOppositeDirection(direction));

        yield return ui.GetComponent<UIFade>().FadeEnum(false);


        player.GetComponent<PlayerMove>().isMapTransitioning = false;
        FindObjectOfType<TurnSystem>().CheckForCombatStart();
    }
}
