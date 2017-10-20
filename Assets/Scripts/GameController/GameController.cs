using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Contains all static data about game scenes.
 */
public static class GameController
{
    // Scene.name :: Object.name :: Position, For persisting given scene objects
    private static Dictionary<PlayableScene, Dictionary<string, Vector3>> savedScenePositions;

    // For resseting scene e.g. level restart
    private static Dictionary<PlayableScene, Dictionary<string, Vector3>> initialScenePositions;
    private static Dictionary<PlayableScene, bool> resetSceneDict;

    static GameController()
    {
        initialScenePositions = new Dictionary<PlayableScene, Dictionary<string, Vector3>>();
        savedScenePositions = new Dictionary<PlayableScene, Dictionary<string, Vector3>>();
        resetSceneDict = new Dictionary<PlayableScene, bool>();
        foreach (PlayableScene playableScene in Enum.GetValues(typeof(PlayableScene)))
        {            
            savedScenePositions[playableScene] = new Dictionary<string, Vector3>(); ;
            initialScenePositions[playableScene] = new Dictionary<string, Vector3>(); ;
            resetSceneDict[playableScene] = false;
        }
    }

    public enum PlayableScene
    {
        [Level(Level.Level1)]
        level1room1,
        [Level(Level.Level1)]
        level1room2,
        [Level(Level.Level1)]
        level1room3,
        [Level(Level.Level2)]
        level2room1,
        [Level(Level.Level2)]
        level2room2,
        [Level(Level.None)]
        WelcomeScreen,
        [Level(Level.None)]
        ExitScene,
    }

    public enum Level {  Level1, Level2, None }

    public class LevelAttribute : Attribute
    {
        public Level level { get; set; }

        public LevelAttribute(Level level)
        {
            this.level = level;
        }
    }

    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }

    public static List<PlayableScene> getScenesForLevel(Level level)
    {
        PlayableScene[] scenes = (PlayableScene[])Enum.GetValues(typeof(PlayableScene));
        List<PlayableScene> filteredScenes = new List<PlayableScene>();
        foreach (PlayableScene s in scenes){
            if (s.GetAttribute<LevelAttribute>().level.Equals(level))
            {
                filteredScenes.Add(s);
            }
        }

        return filteredScenes;
    }

    public static void clearScenesForLevel(Level level)
    {
        getScenesForLevel(level).ForEach(scene => ClearPersistedDataForScene(scene));
    }

    public static void ClearPersistedDataForScene(PlayableScene sceneName)
    {
        savedScenePositions[sceneName] = new Dictionary<string, Vector3>();
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
            if (!initialScenePositions[sceneName].ContainsKey(obj.name))
            {
                // write once
                Debug.Log("Saved initial: " + obj.name);
                initialScenePositions[sceneName][obj.name] = obj.transform.position;
            }
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

    public static bool getResetSceneAttributeFor(PlayableScene sceneName)
    {
        return resetSceneDict[sceneName];
    }

    public static void setResetSceneAttributeFor(PlayableScene sceneName, bool resetScene)
    {
        resetSceneDict[sceneName] = resetScene;
    }

    internal static void AddItem(Item item)
    {
        Debug.Log("Not implemeneted");
    }
}
