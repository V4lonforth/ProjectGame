using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using AndroidGame.GameObjects.Base;

namespace AndroidGame.GUI
{
    class Joystick
    {
        private Action<Vector2> Action;

        private Drawable backgroundDrawable;
        private Drawable foregroundDrawable;

        private Vector2 centre;
        private Rectangle activeZone;

        private int touchID;
        private float radius;
        private bool isActive;

        private float timeToInactiveLeft;
        private const float timeToInactive = 0.5f;

        public Joystick(int size, Rectangle zone, Texture2D background, Texture2D foreground, Action<Vector2> action)
        {
            backgroundDrawable = new Drawable(background, new Rectangle(0, 0, size, size));
            foregroundDrawable = new Drawable(foreground, new Rectangle(0, 0, size, size));

            radius = size / 2f;
            activeZone = zone;
            Action = action;
        }

        private bool IsTouched(Vector2 point)
        {
            if (activeZone.Left <= point.X && point.X <= activeZone.Right && activeZone.Top <= point.Y && point.Y <= activeZone.Bottom)
                return true;
            return false;
        }

        private void StartTouching(Vector2 point)
        {
            float buf;
            if (point.X < (buf = activeZone.Left + radius))
                point.X = buf;
            else if (point.X > (buf = activeZone.Right - radius))
                point.X = buf;
            if (point.Y < (buf = activeZone.Top + radius))
                point.Y = buf;
            else if (point.Y > (buf = activeZone.Bottom - radius))
                point.Y = buf;
            centre = point;
            foregroundDrawable.Position = point;
            backgroundDrawable.Position = point;
            timeToInactiveLeft = timeToInactive;
            isActive = true;
        }
        private void ContinueTouching(Vector2 point)
        {
            point -= centre;
            float distance = point.Length();
            if (distance != 0)
                point /= distance;
            if (distance < radius)
                foregroundDrawable.Position = centre + point * distance;
            else
                foregroundDrawable.Position = centre + point * radius;
            Action(point);
        }
        private void EndTouching()
        {
            foregroundDrawable.Position = centre;
            Action(Vector2.Zero);
            isActive = false;
        }

        public bool Touch(TouchLocation touchLocation, float deltaTime)
        {
            if (!isActive)
            {
                timeToInactiveLeft -= deltaTime;
                if (touchLocation.State == TouchLocationState.Pressed && IsTouched(touchLocation.Position))
                {
                    StartTouching(touchLocation.Position);
                    touchID = touchLocation.Id;
                    return true;
                }
            }
            else if (touchLocation.Id == touchID)
            {
                switch (touchLocation.State)
                {
                    case TouchLocationState.Moved:
                        ContinueTouching(touchLocation.Position);
                        break;
                    case TouchLocationState.Released:
                        EndTouching();
                        break;
                }
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (timeToInactiveLeft > 0.0f)
            {
                backgroundDrawable.Draw(spriteBatch);
                foregroundDrawable.Draw(spriteBatch);
            }
        }
    }
}