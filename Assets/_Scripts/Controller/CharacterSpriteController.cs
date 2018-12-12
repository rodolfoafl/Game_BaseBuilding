﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour {

    Dictionary<Character, GameObject> _characterGameObjectMap;
    Dictionary<string, Sprite> _stringCharacterSpritesMap;

    World _world;

    void Start()
    {
        LoadSprites();

        _world = WorldController.Instance.World;

        _characterGameObjectMap = new Dictionary<Character, GameObject>();

        _world.RegisterCharacterCreated(OnCharacterCreated);

        _world.CreateCharacter(_world.GetTileAt(_world.Width / 2, _world.Height / 2));
    }

    void LoadSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("_Sprites/NewSprites/Characters/");
        _stringCharacterSpritesMap = new Dictionary<string, Sprite>();

        foreach (Sprite s in sprites)
        {
            _stringCharacterSpritesMap[s.name] = s;
        }
    }

    public void OnCharacterCreated(Character character)
    {
        GameObject char_go = new GameObject();

        _characterGameObjectMap.Add(character, char_go);

        char_go.name = "Character";
        char_go.transform.position = new Vector3(character.CurrentTile.X, character.CurrentTile.Y, 0);
        char_go.transform.SetParent(this.transform, true);

        SpriteRenderer sr = char_go.AddComponent<SpriteRenderer>();
        sr.sprite = _stringCharacterSpritesMap["Character"];
        sr.sortingLayerName = "Characters";
    }
}