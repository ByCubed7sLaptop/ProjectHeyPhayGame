using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

[Tool]
public partial class Tool_TileMapAutoFoliage : Node
{
    [Export] public TileMap TileMap { get; set; } = null;

    // The layer to get the tile data from
    [Export] public int TargetLayerID { get; set; } = -1;

    // The layer to put foliage on
    [Export] public int FoliageLayerID { get; set; } = -1;
    [Export] public int TerrianSet { get; set; } = -1;
    [Export] public int TerrianID { get; set; } = -1;

    [Export] public int Seed { get; set; } = 42;

    [Export]
    public bool Run
    {
        get => run;
        set
        {
            if (value)
                RunTool();
            run = false;
        }
    }
    private bool run;

    [Export]
    public bool Debug
    {
        get => debug;
        set
        {
            if (value)
                DebugTool();
            debug = false;
        }
    }
    private bool debug;

    private Random random;
    public override void _Ready()
    {
        if (!Engine.IsEditorHint())
        {
            QueueFree();
            //GetParent().RemoveChild(this);
            return;
        }

        random = new Random(Seed);
    }

    private void RunTool()
    {
        if (TileMap is null)
            return;

        if (TargetLayerID == -1)
            return;

        if (FoliageLayerID == -1)
            return;

        if (TerrianSet == -1)
            return;

        if (TerrianID == -1)
            return;

        // Clear old foliage
        foreach (var tile in TileMap.GetUsedCells(FoliageLayerID))
            TileMap.SetCell(FoliageLayerID, tile);


        bool IsTile(Vector2I tile) => TileMap.GetCellSourceId(TargetLayerID, tile) != -1;
        bool IsNoTile(Vector2I tile) => TileMap.GetCellSourceId(TargetLayerID, tile) == -1;


        var tiles = TileMap.GetUsedCells(TargetLayerID);

        HashSet<Vector2I> tops = new HashSet<Vector2I>();
        HashSet<Vector2I> diagonals = new HashSet<Vector2I>();
        HashSet<Vector2I> bottoms = new HashSet<Vector2I>();
        HashSet<Vector2I> bottomDiagonals = new HashSet<Vector2I>();
        foreach (var tile in tiles)
        {
            var north = tile + new Vector2I(0, -1);
            var south = tile + new Vector2I(0, 1);
            var east = tile + new Vector2I(1, 0);
            var west = tile + new Vector2I(-1, 0);

            var northeast = north + new Vector2I(1, 0);
            var northwest = north + new Vector2I(-1, 0);
            var southeast = south + new Vector2I(1, 0);
            var southwest = south + new Vector2I(-1, 0);

            // Check surrounding tiles
            // If original tile has an edge, add to
            if (IsNoTile(north))
                tops.Add(tile);

            else if (IsNoTile(northeast) && IsTile(east))
                diagonals.Add(tile);

            else if (IsNoTile(northwest) && IsTile(west))
                diagonals.Add(tile);

            else if (IsNoTile(south))
            {
                bottoms.Add(tile);
                //if (IsTile(east))
                //    bottoms.Add(east);
                //if (IsTile(west))
                //    bottoms.Add(west);

                if ((IsNoTile(east) && IsNoTile(northeast)) || (IsNoTile(west) && IsNoTile(northwest)))
                    bottoms.Add(north);
            }

            else if (IsTile(east) || IsTile(west))
            {
                if (IsNoTile(southeast) || IsNoTile(southwest))
                {
                    bottomDiagonals.Add(tile);
                    bottoms.Add(south);
                }
            }
        }

        var bottomArray = new Godot.Collections.Array<Vector2I>();
        foreach (var position in bottoms)
            bottomArray.Add(position);
        foreach (var position in bottomDiagonals)
            bottomArray.Add(position);

        TileMap.SetCellsTerrainConnect(FoliageLayerID, bottomArray, TerrianSet, TerrianID + 1);


        var topArray = new Godot.Collections.Array<Vector2I>();
        foreach (var position in tops)
            topArray.Add(position);
        foreach (var position in diagonals)
            topArray.Add(position);

        TileMap.SetCellsTerrainConnect(FoliageLayerID, topArray, TerrianSet, TerrianID);
    }

    public void DebugTool()
    {
        if (TileMap is null)
            GD.Print("Tilemap is null.");
        else
            GD.Print($"TileMap is {TileMap.GetPath()}");

        GD.Print($"TargetLayerID is {TileMap.GetLayerName(TargetLayerID)}.");
        GD.Print($"FoliageLayerID is {TileMap.GetLayerName(FoliageLayerID)}.");
        GD.Print($"Terrain is {TileMap.TileSet.GetTerrainName(TerrianSet, TerrianID)}.");


    }
}
