using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameLib.GameObjects;
using GameLib.Physics;
using GameLib.Geometry;
using GameLib.Info;

namespace GameLib.Controllers
{
    public abstract class BaseLootController : IController
    {
        protected List<ILoot> inactiveLootList;
        protected List<ILoot> activeLootList;

        private BodyInfo bodyInfo;

        private int startLootCount;

        private const float maxLootDistanceSpawn = 50f;
        private const float minLootDistanceSpawn = 5f;

        private const float triggerRadius = 100f;

        private const int type = 1;
        
        public BaseLootController(int baseCount = 200)
        {
            startLootCount = baseCount;
            inactiveLootList = new List<ILoot>(baseCount);
            activeLootList = new List<ILoot>(baseCount);
            bodyInfo = new BodyInfo(PhysicalType.Loot, new Shape[] 
            {
                new Circle(Vector2.Zero, triggerRadius)
            });
        }
        protected void FillLoot()
        {
            for (int i = 0; i < startLootCount; i++)
                inactiveLootList.Add(CreateLoot(bodyInfo));
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

                ILoot loot = inactiveLootList[inactiveLootList.Count - 1];
                inactiveLootList.RemoveAt(inactiveLootList.Count - 1);
                activeLootList.Add(loot);
                loot.Launch(droppingExp, pos, dir);
            }
        }
        protected abstract ILoot CreateLoot(BodyInfo bodyInfo);

        public void Update(float deltaTime)
        {
            for (int i = 0; i < activeLootList.Count; i++) 
            {
                if (activeLootList[i].IsActive)
                    activeLootList[i].Update(deltaTime);
                else
                {
                    inactiveLootList.Add(activeLootList[i]);
                    activeLootList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}