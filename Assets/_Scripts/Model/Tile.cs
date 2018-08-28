﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    public enum TileType {
        Empty,
        Floor
    };

    TileType type = TileType.Empty;
    LooseObject looseObject;
    InstalledObject installedObject;

    World world;
    int x, y;

    public TileType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;

        }
    }

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
    }

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

}