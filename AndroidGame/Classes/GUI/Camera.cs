using Microsoft.Xna.Framework;
using GameLib.Geometry;
using GameLib.GameObjects.Base;

namespace AndroidGame.GUI
{
    public class Camera
    {
        public PhysicalObject Target
        {
            private get { return target; }
            set
            {
                target = value;
                cameraPosition = target.Position;
                Update(0f);
            }
        }

        private PhysicalObject target;

        private Vector2 viewportCentre;
        private Vector2 cameraPosition;

        private float size;
        
        public Matrix TransformMatrix { get; private set; }

        private const float lerpValue = 3f;

        public Camera(Point screenSize)
        {
            size = 0.35f;
            viewportCentre = screenSize.ToVector2() / (2f * size);
        }

        public void Update(float deltaTime)
        {
            if (target != null)
                cameraPosition = Functions.Interpolate(cameraPosition, Target.Position, lerpValue * deltaTime);
            TransformMatrix = Matrix.CreateTranslation(viewportCentre.X - cameraPosition.X, viewportCentre.Y - cameraPosition.Y, 0) * Matrix.CreateScale(size);
            Vector3 vec = Vector3.Transform(new Vector3(360, 640, 0), TransformMatrix);
        }
    }
}