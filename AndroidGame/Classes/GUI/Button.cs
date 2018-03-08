using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using AndroidGame.GameObjects.Base;

namespace AndroidGame.GUI
{
    class Button : Drawable
    {
        private Func<bool> Action;

        private Rectangle touchZone;

        private static Texture2D[] sprites;

        private int touchId;

        public Button(int spriteIndex, Func<bool> action, Rectangle destRect, Rectangle? zone = null, Color? col = null, float rot = 0f, float dep = 1f) : base(sprites[spriteIndex], destRect, col, rot, dep)
        {
            Action = action;
            if (zone.HasValue)
                touchZone = zone.Value;
            else
                touchZone = destRect;
            touchId = -1;
        }
        public Button(int spriteIndex, Func<bool> action, Vector2 pos, Vector2? size = null, Rectangle? zone = null, Color? col = null, float rot = 0f, float dep = 1f) : base(sprites[spriteIndex], pos, size, col, rot, dep)
        {
            Action = action;
            if (zone.HasValue)
                touchZone = zone.Value;
            else
                touchZone = new Rectangle(pos.ToPoint(), size.Value.ToPoint());
            touchId = -1;
        }
        
        public bool Touch(TouchLocation touchLocation)
        {
            if (touchLocation.State == TouchLocationState.Pressed)
            {
                if (InTouchZone(touchLocation.Position))
                {
                    Action();
                    touchId = touchLocation.Id;
                    return true;
                }
            }
            else if (touchLocation.Id == touchId)
                return true;
            return false;
        }
        
        private bool InTouchZone(Vector2 pos)
        {
            return touchZone.Left <= pos.X && pos.X <= touchZone.Right && touchZone.Top <= pos.Y && pos.Y <= touchZone.Bottom;
        }
    }
}