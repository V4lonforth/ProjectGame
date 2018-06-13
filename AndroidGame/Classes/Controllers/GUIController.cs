using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using AndroidGame.GUI;
using GameLib.Controllers;

namespace AndroidGame.Controllers
{
    class GUIController : IController
    {
        private Texture2D backgroundJoystickTexture;
        private Texture2D foregroundJoystickTexture;
        private Joystick[] joysticks;
        private Button[] buttons;

        public static Point screenSize;

        private static int joystickSize = 180;
        private static int offset = 5;
        private static Point activeZoneSize = new Point(400, 400);

        private const string joystickSpritesPath = "Sprites/Joystick/";

        public GUIController(ContentManager Content)
        {
            backgroundJoystickTexture = Content.Load<Texture2D>(joystickSpritesPath + "Background");
            foregroundJoystickTexture = Content.Load<Texture2D>(joystickSpritesPath + "Foreground");
        }
        public void CreateJoysticks(Action<Vector2> joystickActionLeft, Action<Vector2> joystickActionRight)
        {
            joysticks = new Joystick[]
            {
                new Joystick(joystickSize, new Rectangle(offset, screenSize.Y - activeZoneSize.Y - offset, activeZoneSize.X, activeZoneSize.Y), backgroundJoystickTexture, foregroundJoystickTexture, joystickActionLeft),
                new Joystick(joystickSize, new Rectangle(screenSize.X - activeZoneSize.X - offset, screenSize.Y - activeZoneSize.Y - offset, activeZoneSize.X, activeZoneSize.Y), backgroundJoystickTexture, foregroundJoystickTexture, joystickActionRight)
            };
        }

        public void Update(float deltaTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            if (joysticks != null)
                foreach (Joystick joystick in joysticks)
                    joystick.Update(deltaTime);

            foreach (TouchLocation touchLocation in touchCollection)
            {
                foreach (Joystick joystick in joysticks)
                    if (joystick.Touch(touchLocation))
                        break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (joysticks != null)
                foreach (Joystick joystick in joysticks)
                    joystick.Draw(spriteBatch);
        }
    }
}