public enum TargetingPriorities
{
    Units, // closest opposing unit
    Towers, // closest tower. Can only be used by enemies
    Castle, // player's castle. Can only be used by enemies
    HighestHealth // opposing unit with highest health
}