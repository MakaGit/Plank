using UnityEngine;
using UnityEngine.UI;

namespace Plank
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance = null;

        [SerializeField] private Text textScore = null;
        [SerializeField] private Text textNumberOfChunks = null;

        public int CurrentScore { get; private set; }
        public int CurrentNumber { get; private set; }
        public void ModifyScore(int points)
        {
            CurrentScore += points;
            textScore.text = CurrentScore.ToString();
        }

        public void ModifyNumber(int points)
        {
            CurrentNumber += points;
            textNumberOfChunks.text = CurrentNumber.ToString();
        }
        private void Start()
        {
            Instance = this;
        }
    }
}