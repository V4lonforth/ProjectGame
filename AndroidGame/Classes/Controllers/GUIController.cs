using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using AndroidGame.GUI;

namespace AndroidGame.Controllers
{
    class GUIController : IController
    {
        private Joystick[] joysticks;
        private Button[] buttons;

        public static Point screenSize;

        private static int joystickSize = 180;
        private static int offset = 5;
        private static Point activeZoneSize = new Point(400, 400);

        private const string joystickSpritesPath = "Sprites/Joystick";

        public GUIController(Action<Vector2> joystickActionLeft, Action<Vector2> joystickActionRight, ContentManager Content)
        {
            Texture2D background = Content.Load<Texture2D>(joystickSpritesPath + "Background"), 
                      foreground = Content.Load<Texture2D>(joystickSpritesPath + "Foreground");
            joysticks[0] = new Joystick(joystickSize, new Rectangle(offset, screenSize.Y - activeZoneSize.Y - offset, activeZoneSize.X, activeZoneSize.Y), background, foreground, joystickActionLeft);
            joysticks[1] = new Joystick(joystickSize, new Rectangle(screenSize.X - activeZoneSize.X - offset, screenSize.Y - activeZoneSize.Y - offset, activeZoneSize.X, activeZoneSize.Y), background, foreground, joystickActionRight);
        }

        public void Update(float deltaTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation touchLocation in touchCollection)
            {
                foreach (Joystick joystick in joysticks)
                    if (joystick.Touch(touchLocation, deltaTime))
                        break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Joystick joystick in joysticks)
                joystick.Draw(spriteBatch);
        }
    }
}