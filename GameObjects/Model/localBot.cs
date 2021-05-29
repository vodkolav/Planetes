using Newtonsoft.Json;

namespace GameObjects
{
    /// <summary>
    /// Bot that operates locally inside server
    /// </summary>
    [JsonObject(IsReference = true)]
    public class localBot : Player, IBot
    {
        public AI Ai { get; set; }

        public Player Me => this;

        public localBot()
        {
        }

        public localBot(AI ai, GameState gS) : base("0", "Bot", gS)
        {
            Ai = ai;
            Ai.Bot = this;
            Name = Color.ToString().Substring(6) + " " + ai.GetType().Name;
        }
    }
}
