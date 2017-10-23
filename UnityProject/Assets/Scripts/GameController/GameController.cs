using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using mattmc3.Common.Collections.Generic;

/// <summary>
/// Contains all static data for the game scenes.
/// </summary>
/// 
/// <author>Will Molloy</author>
public static class GameController
{
    // Scene.name :: Object.name :: Position, For persisting given scene objects
    private static Dictionary<PlayableScene, Dictionary<string, Vector3>> SavedScenePositions = new Dictionary<PlayableScene, Dictionary<string, Vector3>>();

    // For resseting scene e.g. level restart
    private static Dictionary<PlayableScene, Dictionary<string, Vector3>> InitialScenePositions = new Dictionary<PlayableScene, Dictionary<string, Vector3>>();
    private static Dictionary<PlayableScene, bool> SceneShouldBeReset = new Dictionary<PlayableScene, bool>();

    // Scene :: Item ID :: ItemWasPickedUp, For item persisting items in scenes/inventory
    private static Dictionary<PlayableScene, OrderedDictionary<int, bool>> GeneratedItemsForScene = new Dictionary<PlayableScene, OrderedDictionary<int, bool>>();
    // For maintaining item pick up order
    private static List<int> InventoryItemsInPickUpOrder = new List<int>();

    // For maintaining levers 
    private static Dictionary<string, bool> LeverInFinalPos = new Dictionary<string, bool>();
    private static Dictionary<string, Vector3> LeverPlateDirection = new Dictionary<string, Vector3>();

    static GameController()
    {
        foreach (PlayableScene playableScene in Enum.GetValues(typeof(PlayableScene)))
        {
            SavedScenePositions[playableScene] = new Dictionary<string, Vector3>();
            InitialScenePositions[playableScene] = new Dictionary<string, Vector3>();
            SceneShouldBeReset[playableScene] = false;
            GeneratedItemsForScene[playableScene] = new OrderedDictionary<int, bool>();
        }
    }

    #region Scenes and Levels

    /// <summary>
    /// Scenes the player can access, the scene files must be included in the build path.
    /// </summary>
    public enum PlayableScene
    {
        [Level(Level.None), FileName("")] // Nullable
        None,

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
        [Level(Level.MiniGame), FileName("miniGame1exp")]
        miniGame1exp,
        [Level(Level.Test), FileName("TEST-LEVERS-room2")]
        TestLeverRoom2,
        [Level(Level.Test), FileName("TEST-LEVERS-room3")]
        TestLeverRoom3,
    }

    /// <summary>
    /// Level attribute for the game scenes.
    /// </summary>
    public enum Level { Level1, Level2, MiniGame, Test, None }

    #endregion

    #region SceneAttributes 

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

    public static string ToString(this PlayableScene scene)
    {
        return scene.GetFileName();
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
    public static List<PlayableScene> GetScenes(this Level levelToRetrieve)
    {
        PlayableScene[] scenes = (PlayableScene[])Enum.GetValues(typeof(PlayableScene));
        return scenes.Where(scene => scene.GetAttribute<LevelAttribute>().level.Equals(levelToRetrieve)).ToList();
    }

    /// <summary>
    /// Clears the persisted data in all scenes for the given level
    /// E.g. use when restarting level from pause menu or on losing all lives.
    /// 
    /// Item Spawns are reset.
    /// </summary>
    public static void ClearScenesIncludingItems(this Level level)
    {
        level.GetScenes().ForEach(scene =>
        {
            scene.ClearPersistedDataForScene();
            GeneratedItemsForScene[scene] = new OrderedDictionary<int, bool>();
            InventoryItemsInPickUpOrder = new List<int>();
        });
    }

    #endregion

    #region SceneObjects

    /// <summary>
    /// Clears the persisted data for the given scene.
    /// E.g. use with reset button.
    /// 
    /// Item spawns ARE NOT reset
    /// </summary>
    public static void ClearPersistedDataForScene(this PlayableScene sceneName)
    {
        SavedScenePositions[sceneName] = new Dictionary<string, Vector3>();
    }

    /// <summary>
    /// Saves the positions of the given objects for the given scene.
    /// </summary>
    public static void SaveObjectPositions(this PlayableScene sceneName, List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            SavedScenePositions[sceneName][obj.name] = obj.transform.position;
            if (!InitialScenePositions[sceneName].ContainsKey(obj.name)) // write once
                InitialScenePositions[sceneName][obj.name] = obj.transform.position;
        }
    }

    public static Dictionary<string, Vector3> GetSavedObjectPositons(this PlayableScene sceneName)
    {
        return SavedScenePositions[sceneName];
    }

    public static Dictionary<string, Vector3> GetInitialObjectPositions(this PlayableScene sceneName)
    {
        return InitialScenePositions[sceneName];
    }

    public static bool GetShouldBeReset(this PlayableScene sceneName)
    {
        return SceneShouldBeReset[sceneName];
    }

    public static void SetShouldBeReset(this PlayableScene sceneName, bool resetScene)
    {
        SceneShouldBeReset[sceneName] = resetScene;
    }

    #endregion

    #region SceneItems

    /// <summary>
    /// Set the generated the order of item Ids for a scene
    /// </summary>
    public static void AddGeneratedItems(this PlayableScene scene, List<int> itemIds)
    {
        itemIds.ForEach(itemId =>
        {
            if (!GeneratedItemsForScene[scene].ContainsKey(itemId))
                GeneratedItemsForScene[scene].Add(itemId, false);
        });
    }

    /// <summary>
    /// Returns the generated item Id set for the given scene
    /// </summary>
    public static OrderedDictionary<int, bool> GetItemsInScene(this PlayableScene scene)
    {
        return GeneratedItemsForScene[scene];
    }

    /// <summary>
    /// Add the given item from the given scene to the global inventory.
    /// 
    /// If you want duplicates have them in the item database twice with a different key.
    /// </summary>
    public static void AddItemToPersistedInventory(this PlayableScene scene, int itemId)
    {
        GeneratedItemsForScene[scene].SetValue(itemId, true);
        if (!InventoryItemsInPickUpOrder.Contains(itemId))
            InventoryItemsInPickUpOrder.Add(itemId);
    }

    /// <summary>
    /// Remove the given item from the given scene from the global inventory.
    /// </summary>
    public static void RemoveItemFromPersistedInventory(this PlayableScene scene, int itemId)
    {
        GeneratedItemsForScene[scene].SetValue(itemId, false);
        InventoryItemsInPickUpOrder.Remove(itemId);
    }

    /// <summary>
    /// Retrieve all items picked up from all scenes.
    /// </summary>
    public static List<int> GetItemsPickedUp()
    {
        return InventoryItemsInPickUpOrder;
    }

    #endregion

    #region Lever-Persistence

    public static bool ActivateLever(this Lever lever)
    {
        var leverName = lever.name;
        if (!LeverInFinalPos.ContainsKey(leverName))
            LeverInFinalPos.Add(leverName, true);
        else
            LeverInFinalPos[leverName] = !LeverInFinalPos[leverName];
        return LeverInFinalPos[leverName];
    }

    public static bool GetLeverInFinalPos(this Lever lever)
    {
        var leverName = lever.name;
        if (!LeverInFinalPos.ContainsKey(leverName))
            return false;
        else
            return LeverInFinalPos[leverName];
    }

    public static void ReversePlateDirection(this PlateScript plate)
    {
        LeverPlateDirection[plate.name] *= -1;
    }

    public static Vector3 GetLeverPlateDirection(this PlateScript plate)
    {
        return LeverPlateDirection[plate.name];
    }

    public static void SetLeverPlateDirection(this PlateScript plate, Vector3 vector)
    {
        if (!LeverPlateDirection.ContainsKey(plate.name))
            LeverPlateDirection[plate.name] = vector;
    }

    #endregion
}

