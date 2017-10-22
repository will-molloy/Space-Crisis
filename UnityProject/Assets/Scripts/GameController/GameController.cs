using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains all static data for the game scenes.
/// </summary>
/// 
/// <author>Will Molloy</author>
public static class GameController
{
    // Scene.name :: Object.name :: Position, For persisting given scene objects
    private static Dictionary<PlayableScene, Dictionary<string, Vector3>> SavedScenePositions;

    // For resseting scene e.g. level restart
    private static Dictionary<PlayableScene, Dictionary<string, Vector3>> InitialScenePositions;
    private static Dictionary<PlayableScene, bool> SceneShouldBeReset;

    // Scene :: Item location :: Item, For item persisting items in scene
    private static Dictionary<PlayableScene, Dictionary<Vector3, PickUpItem>> ItemsInScene; 

    static GameController()
    {
        InitialScenePositions = new Dictionary<PlayableScene, Dictionary<string, Vector3>>();
        SavedScenePositions = new Dictionary<PlayableScene, Dictionary<string, Vector3>>();
        SceneShouldBeReset = new Dictionary<PlayableScene, bool>();
        ItemsInScene = new Dictionary<PlayableScene, Dictionary<Vector3, PickUpItem>>();
        foreach (PlayableScene playableScene in Enum.GetValues(typeof(PlayableScene)))
        {            
            SavedScenePositions[playableScene] = new Dictionary<string, Vector3>(); ;
            InitialScenePositions[playableScene] = new Dictionary<string, Vector3>(); ;
            SceneShouldBeReset[playableScene] = false;
            ItemsInScene[playableScene] = new Dictionary<Vector3, PickUpItem> ();
        }
    }

    /// <summary>
    /// Scenes the player can access, the scene files must be included in the build path.
    /// </summary>
    public enum PlayableScene
    {
        [Level(Level.Level1), FileName("level1room1")]
        Level1Room1,
        [Level(Level.Level1), FileName("level1room2")]
        Level1Room2,
        [Level(Level.Level1), FileName("level1room3")]
        Level1Room3,
        [Level(Level.Level2), FileName("level2room1")]
        Level2Room1,
        [Level(Level.Level2), FileName("level2room2")]
        Level2Room2,
        [Level(Level.Level2), FileName("level2room3")]
        Level2Room3,
        [Level(Level.None), FileName("WelcomeScreen")]
        WelcomeScreen,
        [Level(Level.None), FileName("ExitScene")]
        ExitScene,
    }

    /// <summary>
    /// Level attribute for the game scenes.
    /// </summary>
    public enum Level {  Level1, Level2, None }

    public class LevelAttribute : Attribute
    {
        public readonly Level level;

        public LevelAttribute(Level level)
        {
            this.level = level;
        }
    }

    public class FileNameAttribute : Attribute
    {
        public readonly string fileName;

        public FileNameAttribute(string fileName)
        {
            this.fileName = fileName;
        }
    }

    /// <summary>
    /// Retrives the file name for the given scene.
    /// </summary>
    public static string GetFileName(this PlayableScene scene)
    {
        return scene.GetAttribute<FileNameAttribute>().fileName;
    }

    /// <summary>
    /// Extension method for retrieving the attributes from the PlayableScene enum.
    /// </summary>
    private static T GetAttribute<T>(this PlayableScene value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return (T)attributes[0];
    }

    /// <summary>
    /// Gets all the scenes assigned to the given level
    /// </summary>
    public static List<PlayableScene> getScenesForLevel(Level levelToRetrieve)
    {
        PlayableScene[] scenes = (PlayableScene[])Enum.GetValues(typeof(PlayableScene));
        return scenes.Where(scene => scene.GetAttribute<LevelAttribute>().level.Equals(levelToRetrieve)).ToList();
    }

    /// <summary>
    /// Clears the persisted data in all scenes for the given level
    /// E.g. use when restarting level from pause menu or on losing all lives.
    /// </summary>
    public static void clearScenesForLevel(Level level)
    {
        getScenesForLevel(level).ForEach(scene => ClearPersistedDataForScene(scene));
    }

    /// <summary>
    /// Clears the persisted data for the given scene.
    /// E.g. use with reset button.
    /// </summary>
    public static void ClearPersistedDataForScene(PlayableScene sceneName)
    {
        SavedScenePositions[sceneName] = new Dictionary<string, Vector3>();
        ItemsInScene[sceneName] = new Dictionary<Vector3, PickUpItem>();
    }

    /// <summary>
    /// Saves the positions of the given objects for the given scene.
    /// </summary>
    public static void SaveObjectPositions(PlayableScene sceneName, List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            SavedScenePositions[sceneName][obj.name] = obj.transform.position;
            if (!InitialScenePositions[sceneName].ContainsKey(obj.name)) // write once
                InitialScenePositions[sceneName][obj.name] = obj.transform.position;
        }
    }

    public static Dictionary<string, Vector3> GetSavedObjectPositons(PlayableScene sceneName)
    {
        return SavedScenePositions[sceneName];
    }

    public static Dictionary<string, Vector3> GetInitialObjectPositions(PlayableScene sceneName)
    {
        return InitialScenePositions[sceneName];
    }

    public static bool GetShouldBeReset(PlayableScene sceneName)
    {
        return SceneShouldBeReset[sceneName];
    }

    public static void SetShouldBeReset(PlayableScene sceneName, bool resetScene)
    {
        SceneShouldBeReset[sceneName] = resetScene;
    }

    public static void AddItemToScene(PlayableScene scene, Vector3 itemPos, PickUpItem item)
    {
        Debug.Log("Saved item: " + item.name);
        ItemsInScene[scene][itemPos] = item;
    }

    public static Dictionary<Vector3, PickUpItem> GetItemsFor(PlayableScene scene)
    {
        return ItemsInScene[scene];
    }
}

