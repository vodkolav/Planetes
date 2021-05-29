using PolygonCollision;

namespace GameObjects
{
    /// <summary>
    /// Basic resources required for an AI to operate a bot
    /// </summary>
    public interface IBot 
    {       
        AI Ai { get; set; }

        Player Me { get; }

        void Press(HOTAS h);

        void Release(HOTAS h);

        void Aim(Vector at);       
    }
}
