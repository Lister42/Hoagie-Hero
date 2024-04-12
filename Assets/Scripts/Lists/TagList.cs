

public class TagList
{
    public const string MANAGER = "Manager";
    public const string INPUT = "Input";
    public const string HIGHLIGHTER = "Highlighter";
    public const string NETWORK_MANAGER = "NetworkManager";
    #region Player Prefs
    public const string SINGLE_PLAYER = "singlePlayer";
    public const string NUM_PLAYERS = "numPlayers";
    public const string NETWORKED = "networked";
    public const string LEVEL = "level";
    public const string CHEF_2_ID = "chef2id";
    public const string CHEF_3_ID = "chef3id";
    public const string CHEF_4_ID = "chef4id";
    #endregion

    #region Level Names
    public const string LEVEL_1_1P = "Level1Scene";
    public const string LEVEL_1_2P = "Level1Scene";
    public const string LEVEL_1_3P = "Level1_3P_Scene";
    public const string LEVEL_1_4P = "Level1_4P_Scene";
    public const string LEVEL_2_1P = "Level2Scene";
    public const string LEVEL_2_2P = "Level2Scene";
    public const string LEVEL_2_3P = "Level2_3P_Scene";
    public const string LEVEL_2_4P = "Level2_4P_Scene";
    public const string LEVEL_3_1P = "Level3Scene";
    public const string LEVEL_3_2P = "Level3Scene";
    public const string LEVEL_3_3P = "Level3_3P_Scene";
    public const string LEVEL_3_4P = "Level3_4P_Scene";
    #endregion

    #region Player Detectors
    public const string CHEF_RIGHT_DETECTOR = "ChefRightDetector";
    public const string CHEF_BOTTOM_DETECTOR = "ChefBottomDetector";
    public const string CHEF_LEFT_DETECTOR = "ChefLeftDetector";
    public const string CHEF_ABOVE_DETECTOR = "ChefAboveDetector";

    public const string CHEF_ONE_RIGHT_DETECTOR = "ChefOneRightDetector";
    public const string CHEF_ONE_BOTTOM_DETECTOR = "ChefOneBottomDetector";
    public const string CHEF_ONE_LEFT_DETECTOR = "ChefOneLeftDetector";
    public const string CHEF_ONE_ABOVE_DETECTOR = "ChefOneAboveDetector";

    public const string CHEF_TWO_RIGHT_DETECTOR = "ChefTwoRightDetector";
    public const string CHEF_TWO_BOTTOM_DETECTOR = "ChefTwoBottomDetector";
    public const string CHEF_TWO_LEFT_DETECTOR = "ChefTwoLeftDetector";
    public const string CHEF_TWO_ABOVE_DETECTOR = "ChefTwoAboveDetector";
    #endregion

    #region Foodstuffs
    public const string PAN = "Pan";
    public const string POT = "Pot";
    public const string CHEESE = "Cheese";
    public const string HAM = "Ham";
    public const string BACON = "Bacon";
    public const string LETTUCE = "Lettuce";
    public const string TOMATO = "Tomato";
    public const string BREAD = "Bread";
    public const string MEATBALLS = "Meatballs";
    public const string TOMATO_SAUCE = "Tomato Sauce";
    #endregion

    //Could also be determine by number of current players from player prefs on where to go
    #region Level Names 
    public const string LEVEL_ONE = "Level1Manager";
    public const string LEVEL_TWO = "Level2Manager";
    public const string LEVEL_THREE = "Level3Manager";
    #endregion

    #region Chefs
    public const string CHEF_ONE = "ChefOne";
    public const string CHEF_TWO = "ChefTwo";
    public const string CHEF_THREE = "ChefThree";
    public const string CHEF_FOUR = "ChefFour";

    public static readonly string[] CHEFS = { CHEF_ONE, CHEF_TWO, CHEF_THREE, CHEF_FOUR };
    #endregion
}
