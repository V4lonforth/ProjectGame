﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AndroidGame.GameObjects;
using AndroidGame.Physics;
using AndroidGame.Geometry;

namespace AndroidGame.Controllers
{
    class LootController : IController
    {
        private List<Loot> lootList;

        private Body baseBody;

        private Texture2D lootSprite;

        private const float maxLootDistanceSpawn = 50f;
        private const float minLootDistanceSpawn = 5f;

        private const float triggerRadius = 100f;

        private const int type = 1;

        private const string lootSpritePath = "";

        public LootController(ContentManager Content)
        {
            lootList = new List<Loot>();
            baseBody = new Body(new Shape[] { new Circle(Vector2.Zero, triggerRadius) }, triggerRadius * 2f, Vector2.Zero, Vector2.Zero, PhysicalType.Loot, null, false);
            lootSprite = Content.Load<Texture2D>(lootSpritePath);
        }
        public void AddLoot(float experience, Vector2 position)
        {
            if (experience == 0f)
                return;
            float droppingExp = (float)Math.Sqrt(experience);
            int lootCount;
            if (droppingExp < 150f)
                lootCount = (int)Math.Ceiling(droppingExp / 15f);
            else
                lootCount = 10;
            droppingExp /= lootCount;
            for (int i = 0; i < lootCount; i++)
            {
                Vector2 pos = Functions.RandomVector2(minLootDistanceSpawn, maxLootDistanceSpawn) + position;
                Vector2 dir = Functions.RandomVector2();
                Loot loot = new Loot(lootSprite, droppingExp, pos, dir, baseBody, this);
                lootList.Add(loot);
            }
        }
        public void RemoveLoot(Loot loot)
        {
            lootList.Remove(loot);
        }

        public void Update(float deltaTime)
        {
            foreach (Loot loot in lootList)
                loot.Update(deltaTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Loot loot in lootList)
                loot.Draw(spriteBatch);
        }
    }
}