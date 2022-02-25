using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class gather all the moving features need for a moving entity as the player or an ennemy
/// </summary>
public class TacticsMove : MonoBehaviour
{
    List<Tile> lSelectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>(); //Last In First Out
    Tile currentTile;

    public bool  moving       = false;
    public int   moveDistance = 5;
    public float climbHeight  = 0.4f;
    public float moveSpeed    = 2;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0; //half height of the moving entity

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y; 
    }

    /// <summary>
    /// Get the <c>Tile</c> under the current <c>GameObject</c>
    /// </summary>
    private void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.isCurrent = true;
    }

    /// <summary>
    /// Get the <c>Tile</c> under the target
    /// </summary>
    /// <param name="target">We will look under this GameObject</param>
    /// <returns></returns>
    private Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile t = null;

        if (Physics.Raycast(target.transform.position, Vector3.down, out hit, GetComponent<Collider>().bounds.size.y));
            t = hit.collider.GetComponent<Tile>();

        return t;
    }

    /// <summary>
    /// Store all 4 adjacents <c>Tile</c>
    /// </summary>
    private void ComputeLAdjacent()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(climbHeight);
        }
    }

    /// <summary>
    /// BFS (Breadth First Search) algorithm. It allow us to search which path is the best.<br/>
    /// It will select the current <c>Tile</c>, store the distance of all his neigbhours and will do the same with them<br/>
    /// <seealso cref="wikipedia" href="https://en.wikipedia.org/wiki/Breadth-first_search"/>
    /// </summary>
    public void FindSelectibleTiles()
    {
        ComputeLAdjacent();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>(); //First In First Out

        process.Enqueue(currentTile);
        currentTile.isVisited = true;
        
        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            lSelectableTiles.Add(t);
            t.isSelectable = true;

            if (t.distance < moveDistance)
                foreach (Tile tile in t.lAdjacent)
                    if (! tile.isVisited)
                    {
                        tile.parent = t;
                        tile.isVisited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
        }
    }

    /// <summary>
    /// Define a path
    /// </summary>
    /// <param name="tile"></param>
    public void MoveToTile(Tile destination)
    {
        path.Clear();
        destination.isTarget = true;
        moving = true;

        Tile next = destination;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    /// <summary>
    /// Move the entity toward the destination
    /// </summary>
    public void Move()
    {
        if(path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //calculate the unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading; //face the direction
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //repositionning to avoid the non centered position
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectibleTiles();
            moving = false;
        }
    }

    /// <summary>
    /// Reset all selectible <Tile>
    /// </summary>
    private void RemoveSelectibleTiles()
    {
        if(currentTile != null)
        {
            currentTile.isCurrent= false;
            currentTile = null;
        }

        foreach (Tile tile in lSelectableTiles)
            tile.Reset();

        lSelectableTiles.Clear();
    }

    /// <summary>
    /// Update the heading toward the target (not the Unknown)
    /// </summary>
    /// <param name="target">Destination</param>
    private void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    /// <summary>
    /// Set the velocity
    /// </summary>
    private void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }
}