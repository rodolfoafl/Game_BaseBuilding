﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LooseObjectSpriteController : MonoBehaviour {

    [SerializeField] GameObject _looseObjectUIPrefab;

    Dictionary<LooseObject, GameObject> _looseObjectGameObjectMap;
    Dictionary<string, Sprite> _stringLooseObjectSpritesMap;

    World _world;

    void Start()
    {
        LoadSprites();

        _world = WorldController.Instance.World;

        _looseObjectGameObjectMap = new Dictionary<LooseObject, GameObject>();

        _world.RegisterLooseObjectCreated(OnLooseObjectCreated);

        foreach(string objType in _world.LooseObjectManager.StringLooseObjectMap.Keys)
        {
            foreach (LooseObject obj in _world.LooseObjectManager.StringLooseObjectMap[objType])
            {
                OnLooseObjectCreated(obj);
            } 
        }
    }

    void LoadSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("_Sprites/NewSprites/LooseObjects/");
        _stringLooseObjectSpritesMap = new Dictionary<string, Sprite>();

        foreach (Sprite s in sprites)
        {
            _stringLooseObjectSpritesMap[s.name] = s;
        }
    }

    public void OnLooseObjectCreated(LooseObject obj)
    {
        GameObject obj_go = new GameObject();

        _looseObjectGameObjectMap.Add(obj, obj_go);

        obj_go.name = obj.ObjectType;
        obj_go.transform.position = new Vector3(obj.Tile.X, obj.Tile.Y, 0);
        obj_go.transform.SetParent(this.transform, true);

        SpriteRenderer sr = obj_go.AddComponent<SpriteRenderer>();
        sr.sprite = _stringLooseObjectSpritesMap[obj.ObjectType];
        sr.sortingLayerName = "LooseObjects";

        if(obj.MaxStackSize > 1)
        {
            GameObject ui_go = Instantiate(_looseObjectUIPrefab);
            ui_go.transform.SetParent(obj_go.transform);
            ui_go.transform.localPosition = Vector3.zero;
            ui_go.GetComponentInChildren<Text>().text = obj.StackSize.ToString();
        }

        obj.RegisterLooseObjectChangedCallback(OnLooseObjectChanged);
    }

    void OnLooseObjectChanged(LooseObject obj)
    {
        GameObject obj_go;
        if (!_looseObjectGameObjectMap.TryGetValue(obj, out obj_go))
        {
            Debug.LogError("_looseObjectGameObjectMap doesn't contain the looseObject!");
            return;
        }
        //obj_go.transform.position = new Vector3(obj.Tile.X, obj.Tile.Y, 0f);

        if (obj.StackSize > 0)
        {
            Text text = obj_go.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = obj.StackSize.ToString();
            }
        }
        else
        {
            Destroy(obj_go);
            _looseObjectGameObjectMap.Remove(obj);
            obj.UnregisterLooseObjectChangedCallback(OnLooseObjectChanged);
        }
    }
}
