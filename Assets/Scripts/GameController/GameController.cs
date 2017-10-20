using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class GameController
{
    // Scene.name :: Object.name :: Position, For persisting given scene objects
    private static Dictionary<PlayableScene, Dictionary<string, Vector3>> savedScenePositions;

    // For resseting scene e.g. level restart
    private static Dictionary<PlayableScene, Dictionary<string, Vector3>> initialScenePositions;
    static GameController()
    {
        initialScenePositions = new Dictionary<PlayableScene, Dictionary<string, Vector3>>();
        savedScenePositions = new Dictionary<PlayableScene, Dictionary<string, Vector3>>();
        foreach (PlayableScene playableScene in Enum.GetValues(typeof(PlayableScene)))
        {
            Dictionary<String, Vector3> scenePos = new Dictionary<string, Vector3>();
            savedScenePositions[playableScene] = scenePos;
            initialScenePositions[playableScene] = scenePos;
        }
    }

    class LevelAttribute : Attribute
    {
        public enum Level { Level1, Level2, Level3 }
        public LevelAttribute(Level level)
        {
            this.level = level;
        }
        public Level level { get; set; }
    }

    public enum PlayableScene
    {
        [Level(LevelAttribute.Level.Level1)]
        level1room1,
        [Level(LevelAttribute.Level.Level1)]
        level1room2,
        [Level(LevelAttribute.Level.Level1)]
        level1room3,
        [Level(LevelAttribute.Level.Level2)]
        level2room1,
        [Level(LevelAttribute.Level.Level2)]
        level2room2,
}
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }

    public static List<PlayableScene> getScenesForLevel(Attribute level)
    {
        PlayableScene[] scenes = (PlayableScene[])Enum.GetValues(typeof(PlayableScene));
        return scenes.Where(x => GetAttribute<LevelAttribute>(x) == level).ToList();
    }

    public static T GetAttribute<T>(this PlayableScene value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return (T)attributes[0];
    }

    public static void SaveObjsPosFor(PlayableScene sceneName, List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            savedScenePositions[sceneName][obj.name] = obj.transform.position;
            if (!initialScenePositions[sceneName].ContainsKey(obj.name)) // write once
                initialScenePositions[sceneName][obj.name] = obj.transform.position;
        }
    }

    public static Dictionary<string, Vector3> getSavedObjsPosFor(PlayableScene sceneName)
    {
        return savedScenePositions[sceneName];
    }

    public static Dictionary<string, Vector3> getInitialObjsPosFor(PlayableScene sceneName)
    {
        return initialScenePositions[sceneName];
    }

    internal static void AddItem(Item item)
    {
        Debug.Log("Not implemeneted");
    }
}
