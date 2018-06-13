using System.Collections.Generic;

namespace GameLib.Physics
{
    public class PhysicsController
    {
        private CollisionChecker collisionChecker;
        private List<Body>[] bodies;

        private bool[,] collisionPermission;

        private int physicalTypesCount;

        public PhysicsController()
        {
            collisionChecker = new CollisionChecker();

            physicalTypesCount = 3;
            bodies = new List<Body>[physicalTypesCount];
            for (int i = 0; i < physicalTypesCount; i++)
                bodies[i] = new List<Body>();

            collisionPermission = new bool[,]
            {
                { false, true, true },
                { true, false, false },
                { true, false, false }
            };
        }
        public PhysicsController(int physicalTypesCount, bool[,] collisionPermission)
        {
            collisionChecker = new CollisionChecker();

            this.physicalTypesCount = physicalTypesCount;
            this.collisionPermission = collisionPermission;

            bodies = new List<Body>[physicalTypesCount];
            for (int i = 0; i < physicalTypesCount; i++)
                bodies[i] = new List<Body>();
        }

        public void AddBody(Body body, PhysicalType type)
        {
            bodies[(int)type].Add(body);
        }
        public void RemoveBody(Body body, int type)
        {
            bodies[type].Remove(body);
        }

        public void CheckCollisions()
        {
            for (int i = 0; i < physicalTypesCount; i++)
            {
                for (int j = i + 1; j < physicalTypesCount; j++)
                {
                    if (collisionPermission[i, j])
                    {
                        CheckBodyListsCollisions(i, j);
                    }
                }
            }
        }
        private void CheckBodyListsCollisions(int i, int j)
        {
            for (int k = 0; k < bodies[i].Count; k++)
            {
                for (int l = 0; l < bodies[j].Count; l++)
                {
                    if (bodies[i][k].CheckCollision(bodies[j][l], collisionChecker))
                    {
                        if (bodies[i][k].OnCollisionAction(bodies[j][l]))
                        {
                            RemoveBody(bodies[i][k], i);
                            k--;
                        }
                        if (bodies[j][l].OnCollisionAction(bodies[i][k]))
                        {
                            RemoveBody(bodies[j][l], j);
                            l--;
                        }
                    }
                }
            }
        }
    }
}