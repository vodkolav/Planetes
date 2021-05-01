namespace PolygonCollision
{ 
    // We need this layer because we can't have static Interface
    public static class DrawingContext
    {       
        public static IGraphicsContainer GraphicsContainer { get; set; }
    }
}