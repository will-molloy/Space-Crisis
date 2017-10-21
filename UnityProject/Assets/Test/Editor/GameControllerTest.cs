using NUnit.Framework;
using System.Collections.Generic;

public class GameControllerTest
{

    /// <summary>
    /// Ensures scenes in level are retrieved with no extra or missing scenes and in correct order.
    /// </summary>
    [Test]
    public void TestSceneLevelRetrieval()
    {
        List<GameController.PlayableScene> expectedScenesNoLevel = new List<GameController.PlayableScene>
        {
            GameController.PlayableScene.WelcomeScreen,
            GameController.PlayableScene.ExitScene,
        };

        List<GameController.PlayableScene> expectedScenesLevel1 = new List<GameController.PlayableScene>
        {
            GameController.PlayableScene.Level1Room1,
            GameController.PlayableScene.Level1Room2,
            GameController.PlayableScene.Level1Room3,
        };

        List<GameController.PlayableScene> expectedScenesLevel2 = new List<GameController.PlayableScene>
        {
            GameController.PlayableScene.Level2Room1,
            GameController.PlayableScene.Level2Room2,
        };

        Assert.AreEqual(expectedScenesLevel1, GameController.getScenesForLevel(GameController.Level.Level1));
        Assert.AreEqual(expectedScenesLevel2, GameController.getScenesForLevel(GameController.Level.Level2));
        Assert.AreEqual(expectedScenesNoLevel, GameController.getScenesForLevel(GameController.Level.None));
    }
}
