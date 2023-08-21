using UnityEngine;

namespace Chess
{

    [CreateAssetMenu (menuName = "Settings/Game")]
    public class GameSettings : ScriptableObject
    {
        public bool defaultAIAssistance;
    }
}