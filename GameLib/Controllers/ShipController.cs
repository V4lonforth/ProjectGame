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

        public ShipController(int bufferSize = 100)
        {
            count = bufferSize;
            
            FirstElementNumber = 0;
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
            SetShipState(number);
            (ship).Update((float)(time - shipStatesData[number].timeData.time));
            shipCurrentStateNumber = -1;
            return new ShipStateData()
            {
                shipId = ship.Id,
                inputData = shipStatesData[number].inputData,
                shipData = ship.GetShipData(),
                timeData = new TimeData()
                {
                    time = time,
                    dataNumber = shipStatesData[number].timeData.dataNumber
                }
            };
        }
        public void CheckShipData(ref TimeData timeData, ref ShipData shipData)
        {
            if (timeData.dataNumber < LastElementNumber || timeData.dataNumber > FirstElementNumber)
                return;
            shipStatesData[timeData.dataNumber % count].shipData = shipData;
            CalculateShipDataStates(timeData.dataNumber, FirstElementNumber);
        }
        public void AddInputData(ref TimeData timeData, ref InputData inputData)
        {
            if (timeData.dataNumber == FirstElementNumber + 1)
            {
                FirstElementNumber++;
                shipStatesData[FirstElementIndex].inputData = inputData;
                shipStatesData[FirstElementIndex].timeData = timeData;
                CalculateShipDataState(FirstElementNumber);
            }
            else if (timeData.dataNumber < LastElementNumber)
                return;
            else if (timeData.dataNumber >= LastElementNumber + count)
            {
                shipStatesData[0].shipData = shipStatesData[FirstElementIndex].shipData;
                FirstElementNumber = 0;
                shipStatesData[0].timeData = timeData;
                shipStatesData[0].inputData = inputData;
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
            else
            {
                shipStatesData[timeData.dataNumber % count].inputData = inputData;
                shipStatesData[timeData.dataNumber % count].timeData = timeData;
                CalculateShipDataStates(timeData.dataNumber + 1, FirstElementNumber);
            }
        }
        private void CalculateShipDataState(int number)
        {
            SetShipState((number - 1) % count);
            float deltaTime = (float)(shipStatesData[number % count].timeData.time - shipStatesData[(number - 1) % count].timeData.time);
            (ship).Update(deltaTime);
            shipStatesData[number % count].shipData = ship.GetShipData();
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
                (ship).Update(deltaTime);
                shipStatesData[index].shipData = ship.GetShipData();
                ship.SetInput(ref shipStatesData[index].inputData);
                firstNumber++;
            }
            shipCurrentStateNumber = lastNumber;
        }
        private void SetShipState(int index)
        {
            ship.SetShipState(ref shipStatesData[index].shipData, ref shipStatesData[index].inputData);
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
        public void SetShip(BaseShip baseShip)
        {
            ship = baseShip;
            shipStatesData = new ShipStateData[count];
            shipStatesData[0].shipData = baseShip.GetShipData();
        }
    }
}