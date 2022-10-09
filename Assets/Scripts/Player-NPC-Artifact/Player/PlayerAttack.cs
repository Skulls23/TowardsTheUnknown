using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : TacticsAttack
{
    private bool isAttacking = false;

    private Inventory inventory;
    private bool isAnimationRunning;
    private PlayerStats playerStats;
    private PlayerTurn playerTurn;

    private IArtifact currentArtifact;
    

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerStats = GetComponent<PlayerStats>();
        playerTurn = GetComponent<PlayerTurn>();
        isAnimationRunning = false;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            FindSelectibleTiles();
            InputListener();
        }
    }

    /// <summary>
    /// Handler for the <c>Inputs</c>
    /// </summary>
    private void InputListener()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerTerrain = LayerMask.NameToLayer("Terrain");

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << layerTerrain))
            {
                Tile t = hit.collider.GetComponent<Tile>();

                if (t.isSelectable)
                    Attack(hit);
            }
        }
    }

    /// <summary>
    /// Launch the attack with the selected <c>Artifact</c>
    /// </summary>
    /// <param name="hitTerrain">The position where the player clicked</param>
    public void Attack(RaycastHit hitTerrain)
    {
        if(currentArtifact.IsRaycastHitAccepted(hitTerrain))
        {
            currentArtifact.Launch(playerStats, hitTerrain, animator);
            isAnimationRunning = true;
        }
        CheckIfArtifactIsValid();
    }

    /// <summary>
    /// Repaint the map with 0 attack distance <br/>
    /// used to reset the <c>Tile</c> color before switching to move mode
    /// </summary>
    public void RepaintMapWithZero()
    {
        int tempMoveRemaining = maxAttackDistance;
        maxAttackDistance = 0;
        FindSelectibleTiles();
        maxAttackDistance = tempMoveRemaining;
    }

    /// <summary>
    /// Set the attacking bool to it's opposite
    /// </summary>
    /// <param name="numArtifact">the number of the <c>Artifact</c> call to attack</param>
    public void SetAttackingArtifact(int numArtifact)
    {
        if (numArtifact >= inventory.LArtifacts.Count) {
            playerTurn.SetState(PlayerTurn.PlayerState.MOVE);
        }
        else {
            currentArtifact = inventory.LArtifacts[numArtifact];
            maxAttackDistance = currentArtifact.GetMaxDistance();
            minAttackDistance = currentArtifact.GetMinDistance();
            CheckIfArtifactIsValid();
        }
    }

    /// <summary>
    /// Checks if the currentArtifact can still be cast, and goes to move state if not.
    /// </summary>
    private void CheckIfArtifactIsValid() {
        if (currentArtifact.GetCost() > playerStats.CurrentEnergy) playerTurn.SetState(PlayerTurn.PlayerState.MOVE);
	}

    /// <summary>
    /// Repaint the map with 0 attack distance <br/>
    /// used to reset the <c>Tile</c> color before switching to attack mode
    /// </summary>
    public void SetAttackingState(bool state)
    {
        isAttacking = state;
        if (!state)
            RepaintMapWithZero();
    }

    public bool GetAttackingState()
    {
        return isAttacking;
    }

    public bool IsAnimationRunning { get => isAnimationRunning; set => isAnimationRunning = value; }
}
