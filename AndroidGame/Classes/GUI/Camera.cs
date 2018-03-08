using Microsoft.Xna.Framework;
using AndroidGame.Geometry;
using AndroidGame.GameObjects.Base;

namespace AndroidGame.GUI
{
    class Camera
    {
        private PhysicalObject target;
        
        private Vector2 viewportCentre;
        private Vector2 cameraPosition;

        private float size;

        public Matrix TransformMatrix { get; private set; }

        private const float lerpValue = 3f;

        public Camera(Point screenSize, PhysicalObject targ)
        {
            size = 0.5f;
            viewportCentre = screenSize.ToVector2() / (2f * size);
            cameraPosition = targ.Position;
            target = targ;
        }

        public void MoveCamera(float delta)
        {
            cameraPosition = Functions.Lerp(cameraPosition, target.Position, lerpValue * delta);
            TransformMatrix = Matrix.CreateTranslation(viewportCentre.X - cameraPosition.X, viewportCentre.Y - cameraPosition.Y, 0) * Matrix.CreateScale(size);
        }
    }
}