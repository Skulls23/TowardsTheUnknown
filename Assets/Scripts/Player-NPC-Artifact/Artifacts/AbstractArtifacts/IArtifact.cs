using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArtifact
{
    /// <summary>
    /// Manages the artifact's targets then applies its effects
    /// </summary>
    /// <param name="source">The entity using the artifact</param>
    /// <param name="tile">The targeted tile</param>
    /// <param name="animator"></param>
    public void Launch(PlayerStats source, Tile tile, Animator animator);

    /// <summary>
    /// Applies the artifact's effects to a specific target
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public void ApplyEffects(PlayerStats source, EntityStats target);

    /// <summary>
    /// Tells if a tile is valid to be targeted
    /// </summary>
    public bool CanTarget(Tile tile);

    /// <summary>
    /// Get the artifact's attack range
    /// </summary>
    /// <returns></returns>
    public AreaInfo GetRange();

    /// <summary>
    /// Applies start of turn effects to the artifact
    /// </summary>
    public void TurnStart();


    /// <summary>
    /// Tells if the artifact can be cast by the source entity
    /// </summary>
    public bool CanUse(PlayerStats source);

    /// <summary>
    /// Gets the tiles targetted by the artifact
    /// </summary>
    public List<Tile> GetTargets(Tile targetedTile);
}
