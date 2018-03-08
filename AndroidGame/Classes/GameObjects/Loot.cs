using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Physics;
using AndroidGame.GameObjects.Base;
using AndroidGame.Controllers;

namespace AndroidGame.GameObjects
{
    class Loot : PhysicalObjectAcc
    {
        private Drawable drawable;

        private float experience;

        private Vector2 startDirection;

        private LootController lootController;

        private new const float maxSpeed = 3000f;
        private new const float acceleration = 800f;

        private const float lootDistance = 30f;
        private static Vector2 size = new Vector2(50f);
        private static Color color = Color.White;


        public Loot(Texture2D sprite, float exp, Vector2 pos, Vector2 dir, Body baseBody, LootController controller) : base(maxSpeed, acceleration, PhysicalObjectType.Loot, pos, Vector2.Zero, dir, baseBody)
        {
            body.OnCollisionAction = OnTrigger;
            startDirection = dir;
            experience = exp;
            lootController = controller;
            drawable = new Drawable(sprite, pos, size, color, 0f);
        }
        
        private bool OnTrigger(Body body)
        {
            switch(body.Parent.Type)
            {
                case PhysicalObjectType.Ship:
                    isAccelerating = true;
                    MovementDirection = body.Parent.Position - Position;
                    float distance = MovementDirection.Length();
                    if (distance <= lootDistance)
                    {
                        ((Ship)body.Parent).GainExperience(experience);
                        lootController.RemoveLoot(this);
                        return true;
                    }
                    MovementDirection /= distance;
                    break;
            }
            return false;
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
            drawable.Position = Position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            drawable.Draw(spriteBatch);
        }
    }
}