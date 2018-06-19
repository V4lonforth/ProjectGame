using GameLib.GameObjects;
using GameLib.GameObjects.Base;
using Microsoft.Xna.Framework;
using NetworkLib.Data;

namespace GameLib.Controllers
{
    public class ShipController
    {
        private ShipStateData[] shipStatesData;
        
        public BaseShip Ship
        {
            get;
            private set;
        }
        private BaseShip testShip;
        private int shipCurrentStateNumber;
        
        private int count;

        public int Id
        {
            get { return Ship.Id; }
        }
        public double Time
        {
            get { return shipStatesData[FirstElementIndex].timeData.time; }
        }
        private double lastTimeDataSent;

        public int FirstElementNumber
        {
            private set;
            get;
        }
        private int LastElementNumber
        {
            get
            {
                int number = FirstElementNumber - count + 1;
                if (number < 0)
                    return 0;
                return number;
            }
        }

        private int FirstElementIndex
        {
            get { return FirstElementNumber % count; }
        }
        private int LastElementIndex
        {
            get { return LastElementNumber % count; }
        }

        private const double maxTimeDiff = 0.5d;
        private const double iterationTime = 1d / 60d;

        public ShipController(int bufferSize = 100)
        {
            count = bufferSize;
            
            FirstElementNumber = 0;
        }
        public void Update()
        {
            int index = (FirstElementNumber - 1) % count;
            if (index < 0)
            {
                index = 0;
            }
            Ship.SetShipState(ref shipStatesData[index].shipData, ref shipStatesData[index].inputData);
            ((IShip)Ship).Update((float)(shipStatesData[FirstElementIndex].timeData.time - shipStatesData[index].timeData.time));
        }
        public void GetShipState(int number)
        {
            if (number > 0)
                number = (number - 1) % count;
            else
                number = 0;
            Ship.SetShipState(ref shipStatesData[number].shipData, ref shipStatesData[number].inputData);
            ((IShip)Ship).Update((float)(shipStatesData[(number + 1) % count].timeData.time - shipStatesData[number].timeData.time));
        }
        public ShipStateData GetShipState(double time)
        {
            int number = FirstElementNumber;
            while (number > LastElementNumber && shipStatesData[number % count].timeData.time > time)
                number--;
            number %= count;
            Ship.SetShipState(ref shipStatesData[number].shipData, ref shipStatesData[number].inputData);
            ((IShip)Ship).Update((float)(time - shipStatesData[number].timeData.time));
            if (number == FirstElementIndex)
            {
                FirstElementNumber++;
                shipStatesData[FirstElementIndex].shipData = Ship.GetShipData();
                shipStatesData[FirstElementIndex].inputData = new InputData();
                shipStatesData[FirstElementIndex].timeData.time = time;
                shipStatesData[FirstElementIndex].timeData.dataNumber = shipStatesData[(FirstElementNumber - 1) % count].timeData.dataNumber + 1;
            }
            shipCurrentStateNumber = -1;
            lastTimeDataSent = time;
            return new ShipStateData()
            {
                shipId = Ship.Id,
                inputData = shipStatesData[number].inputData,
                shipData = testShip.GetShipData(),
                timeData = new TimeData()
                {
                    time = time,
                    dataNumber = shipStatesData[number].timeData.dataNumber
                }
            };
        }
        public CreateShipActionData GetCreateShipActionData()
        {
            return new CreateShipActionData()
            {
                position = Ship.Position,
                id = Id,
                owner = ShipOwner.AI,
                shipType = Ship.ShipType,
                team = Ship.Team
            };
        }
        public void CheckShipData(ref ShipStateData shipStateData)
        {
            /*shipStatesData[shipStateData.timeData.dataNumber % count] = shipStateData;
            if (shipStateData.timeData.dataNumber > FirstElementNumber)
                FirstElementNumber = shipStateData.timeData.dataNumber;
            return;*/
            if (shipStateData.timeData.dataNumber <= LastElementNumber)
                return;
            if (shipStateData.timeData.dataNumber == FirstElementNumber + 1)
            {
                shipStatesData[shipStateData.timeData.dataNumber % count] = shipStateData;
                FirstElementNumber = shipStateData.timeData.dataNumber;
                //AddInputData(ref shipStateData.timeData, ref shipStateData.inputData);
                //shipStatesData[shipStateData.timeData.dataNumber % count].shipData = shipStateData.shipData;
            }
            else
            {
                shipStatesData[shipStateData.timeData.dataNumber % count] = shipStateData;
                if (shipStateData.timeData.dataNumber > FirstElementNumber)
                {
                    CalculateShipDataStates(FirstElementNumber, shipStateData.timeData.dataNumber);
                    FirstElementNumber = shipStateData.timeData.dataNumber;
                }
                else
                    CalculateShipDataStates(shipStateData.timeData.dataNumber, FirstElementNumber);
            }
        }
        public void AddInputData(ref TimeData timeData, ref InputData inputData)
        {
            if (timeData.dataNumber == FirstElementNumber + 1)
            {
                if (timeData.time - Time > maxTimeDiff)
                {
                    SetInactive(FirstElementIndex);
                    shipStatesData[FirstElementIndex].shipData = testShip.GetShipData();
                    shipStatesData[FirstElementIndex].inputData = inputData;
                    shipStatesData[FirstElementIndex].timeData = timeData;
                }
                else if (timeData.time < Time)
                    return;
                FirstElementNumber++;
                shipStatesData[FirstElementIndex].inputData = inputData;
                shipStatesData[FirstElementIndex].timeData = timeData;
                CalculateShipDataState(FirstElementNumber);
            }
            else if (timeData.dataNumber < LastElementNumber)
                return;
            else if (timeData.dataNumber >= FirstElementNumber + count)
            {
                SetInactive(FirstElementIndex);
                FirstElementNumber = timeData.dataNumber;
                shipStatesData[FirstElementIndex].shipData = testShip.GetShipData();
                shipStatesData[FirstElementIndex].timeData = timeData;
                shipStatesData[FirstElementIndex].inputData = inputData;
            }
            else if (timeData.dataNumber > FirstElementNumber + 1)
            {
                double deltaTime = (timeData.time - shipStatesData[FirstElementIndex].timeData.time) / (timeData.dataNumber - FirstElementNumber);
                int prevIndex = FirstElementIndex;
                FirstElementNumber++;
                int firstNumber = FirstElementNumber;
                while (timeData.dataNumber > FirstElementNumber)
                {
                    int index = FirstElementIndex;
                    shipStatesData[index].timeData = new TimeData()
                    {
                        dataNumber = shipStatesData[prevIndex].timeData.dataNumber + 1,
                        time = shipStatesData[prevIndex].timeData.time + deltaTime
                    };
                    shipStatesData[index].inputData = shipStatesData[(FirstElementNumber - 1) % count].inputData;// new InputData();

                    prevIndex = index;
                    FirstElementNumber++;
                }
                shipStatesData[FirstElementIndex].inputData = inputData;
                shipStatesData[FirstElementIndex].timeData = timeData;
                CalculateShipDataStates(firstNumber, FirstElementNumber);
            }
            else if (timeData.time > lastTimeDataSent)
            {
                shipStatesData[timeData.dataNumber % count].inputData = inputData;
                shipStatesData[timeData.dataNumber % count].timeData = timeData;
                CalculateShipDataStates(timeData.dataNumber + 1, FirstElementNumber);
            }
        }
        private void SetInactive(int index)
        {
            shipStatesData[index].inputData = new InputData();
            SetShipState(index);
            while (testShip.Speed > 0)
                testShip.Update((float)iterationTime);
        }
        private void CalculateShipDataState(int number)
        {
            SetShipState((number - 1) % count);
            float deltaTime = (float)(shipStatesData[number % count].timeData.time - shipStatesData[(number - 1) % count].timeData.time);
            (testShip).Update(deltaTime);
            shipStatesData[number % count].shipData = testShip.GetShipData();
            shipCurrentStateNumber = number;
            System.Diagnostics.Debug.WriteLine($"{testShip.Position.X}, {testShip.Position.Y}, {testShip.Speed}");
        }
        private void CalculateShipDataStates(int firstNumber, int lastNumber)
        {
            if (firstNumber == 0)
                firstNumber++;
            int prevIndex = (firstNumber - 1) % count;
            int index = firstNumber % count;
            SetShipState(prevIndex);
            while (firstNumber <= lastNumber)
            {
                float deltaTime;
                if (shipStatesData[index].timeData.dataNumber <= LastElementNumber)
                {
                    deltaTime = (float)iterationTime;
                    shipStatesData[index].timeData.time = shipStatesData[prevIndex].timeData.time + iterationTime;
                    shipStatesData[index].timeData.dataNumber = shipStatesData[prevIndex].timeData.dataNumber + 1;
                    shipStatesData[index].inputData = new InputData();
                }
                else
                    deltaTime = (float)(shipStatesData[firstNumber % count].timeData.time - shipStatesData[prevIndex].timeData.time);
                prevIndex = index;
                testShip.Update(deltaTime);
                shipStatesData[index].shipData = testShip.GetShipData();
                testShip.SetInput(ref shipStatesData[index].inputData);
                firstNumber++;
                index = firstNumber % count;
            }
            shipCurrentStateNumber = lastNumber;
        }
        private void SetShipState(int index)
        {
            testShip.SetShipState(ref shipStatesData[index].shipData, ref shipStatesData[index].inputData);
        }
        private bool CheckState(int index)
        {
            return shipStatesData[index].timeData.dataNumber < LastElementNumber;
        }
        public void SetInitialeTime(double time)
        {
            if (FirstElementNumber >= count)
                return;
            shipStatesData[0].timeData.time = time;
            if (FirstElementNumber > 0)
                CalculateShipDataStates(0, FirstElementNumber);
        }
        public void SetShip(BaseShip baseShip, BaseShip testBaseShip)
        {
            Ship = baseShip;
            testShip = testBaseShip;
            shipStatesData = new ShipStateData[count];
            shipStatesData[0].shipData = baseShip.GetShipData();
        }
    }
}