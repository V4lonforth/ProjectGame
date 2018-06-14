using GameLib.GameObjects;
using GameLib.GameObjects.Base;
using Microsoft.Xna.Framework;
using NetworkLib.Data;

namespace GameLib.Controllers
{
    public class ShipController
    {
        private ShipStateData[] shipStatesData;

        private BaseShip ship;
        private BaseShip testShip;
        private int shipCurrentStateNumber;
        
        private int count;

        public int Id
        {
            get { return ship.Id; }
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
        public void SetShipToLastState()
        {
            ship.SetShipState(ref shipStatesData[FirstElementIndex].shipData, ref shipStatesData[FirstElementIndex].inputData);
        }
        public ShipStateData GetShipState(int number)
        {
            return shipStatesData[number % count];
        }
        public ShipStateData GetShipState(double time)
        {
            int number = FirstElementNumber;
            while (number > LastElementNumber && shipStatesData[number % count].timeData.time > time)
                number--;
            number %= count;
            ship.SetShipState(ref shipStatesData[number].shipData, ref shipStatesData[number].inputData);
            ship.Update((float)(time - shipStatesData[number].timeData.time));
            shipCurrentStateNumber = -1;
            lastTimeDataSent = time;
            return new ShipStateData()
            {
                shipId = ship.Id,
                inputData = shipStatesData[number].inputData,
                shipData = testShip.GetShipData(),
                timeData = new TimeData()
                {
                    time = time,
                    dataNumber = shipStatesData[number].timeData.dataNumber
                }
            };
        }
        public void CheckShipData(ref ShipStateData shipStateData)
        {
            if (shipStateData.timeData.dataNumber <= LastElementNumber)
                return;
            if (shipStateData.timeData.dataNumber > FirstElementNumber)
            {
                AddInputData(ref shipStateData.timeData, ref shipStateData.inputData);
                shipStatesData[shipStateData.timeData.dataNumber % count].shipData = shipStateData.shipData;
            }
            else
            {
                shipStatesData[shipStateData.timeData.dataNumber % count] = shipStateData;
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
                    shipStatesData[index].inputData = new InputData();

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
        }
        private void CalculateShipDataStates(int firstNumber, int lastNumber)
        {
            int index = (firstNumber - 1) % count;
            SetShipState(index);
            while (firstNumber <= lastNumber)
            {
                float deltaTime = (float)(shipStatesData[firstNumber % count].timeData.time - shipStatesData[index].timeData.time);
                index = firstNumber % count;
                (testShip).Update(deltaTime);
                shipStatesData[index].shipData = testShip.GetShipData();
                testShip.SetInput(ref shipStatesData[index].inputData);
                firstNumber++;
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
            ship = baseShip;
            testShip = testBaseShip;
            shipStatesData = new ShipStateData[count];
            shipStatesData[0].shipData = baseShip.GetShipData();
        }
    }
}