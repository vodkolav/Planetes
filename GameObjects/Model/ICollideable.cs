using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    //not quite sure it's supposed to be interface. maybe abstract class
    public interface ICollideable
    {
        bool Collides(Jet j);

        bool Collides(Astroid a);
        PolygonCollisionResult Collides(Wall w);
    }
}