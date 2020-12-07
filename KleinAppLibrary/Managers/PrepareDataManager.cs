using KleinMapLibrary.Enums;
using KleinMapLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace KleinMapLibrary.Managers
{
    public static class PrepareDataManager
    {
        public static State SetState(ParamType type, double value)
        {
            State state = State.Unknown;

            switch (type)
            {
                case ParamType.Unknown:
                    state = State.Unknown;
                    break;

                case ParamType.NO2:

                    if (value <= 40)
                    {
                        state = State.VeryGood;
                    }
                    else if (value >= 40.1 && value <= 100)
                    {
                        state = State.Good;
                    }
                    else if (value >= 100.1 && value <= 150)
                    {
                        state = State.OK;
                    }
                    else if (value >= 150.1 && value <= 200)
                    {
                        state = State.Warning;
                    }
                    else if (value >= 200.1 && value <= 400)
                    {
                        state = State.NonFatal;
                    }
                    else if (value > 400)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.PM10:

                    if (value >= 0 && value <= 20)
                    {
                        state = State.VeryGood;
                    }
                    else if (value >= 20.1 && value <= 50)
                    {
                        state = State.Good;
                    }
                    else if (value >= 50.1 && value <= 80)
                    {
                        state = State.OK;
                    }
                    else if (value >= 80.1 && value <= 110)
                    {
                        state = State.Warning;
                    }
                    else if (value >= 110.1 && value <= 150)
                    {
                        state = State.NonFatal;
                    }
                    else if (value > 150)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.PM25:

                    if (value >= 0 && value <= 13)
                    {
                        state = State.VeryGood;
                    }
                    else if (value >= 13.1 && value <= 35)
                    {
                        state = State.Good;
                    }
                    else if (value >= 35.1 && value <= 55)
                    {
                        state = State.OK;
                    }
                    else if (value >= 55.1 && value <= 75)
                    {
                        state = State.Warning;
                    }
                    else if (value >= 75.1 && value <= 110)
                    {
                        state = State.NonFatal;
                    }
                    else if (value > 110)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.O3:

                    if (value >= 0 && value <= 70)
                    {
                        state = State.VeryGood;
                    }
                    else if (value >= 70.1 && value <= 120)
                    {
                        state = State.Good;
                    }
                    else if (value >= 120.1 && value <= 150)
                    {
                        state = State.OK;
                    }
                    else if (value >= 150.1 && value <= 180)
                    {
                        state = State.Warning;
                    }
                    else if (value >= 180.1 && value <= 240)
                    {
                        state = State.NonFatal;
                    }
                    else if (value > 240)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.CO:

                    if (value >= 0 && value <= 3)
                    {
                        state = State.VeryGood;
                    }
                    else if (value >= 3.1 && value <= 7)
                    {
                        state = State.Good;
                    }
                    else if (value >= 7.1 && value <= 11)
                    {
                        state = State.OK;
                    }
                    else if (value >= 11.1 && value <= 15)
                    {
                        state = State.Warning;
                    }
                    else if (value >= 15.1 && value <= 21)
                    {
                        state = State.NonFatal;
                    }
                    else if (value > 21)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.C6H6:

                    if (value >= 0 && value <= 6)
                    {
                        state = State.VeryGood;
                    }
                    else if (value >= 6.1 && value <= 11)
                    {
                        state = State.Good;
                    }
                    else if (value >= 11.1 && value <= 16)
                    {
                        state = State.OK;
                    }
                    else if (value >= 16.1 && value <= 21)
                    {
                        state = State.Warning;
                    }
                    else if (value >= 21.1 && value <= 51)
                    {
                        state = State.NonFatal;
                    }
                    else if (value > 51)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.SO2:

                    if (value >= 0 && value <= 50)
                    {
                        state = State.VeryGood;
                    }
                    else if (value >= 50.1 && value <= 100)
                    {
                        state = State.Good;
                    }
                    else if (value >= 100.1 && value <= 200)
                    {
                        state = State.OK;
                    }
                    else if (value >= 200.1 && value <= 350)
                    {
                        state = State.Warning;
                    }
                    else if (value >= 350.1 && value <= 500)
                    {
                        state = State.NonFatal;
                    }
                    else if (value > 500)
                    {
                        state = State.Fatal;
                    }

                    break;

                default:
                    state = State.Unknown;
                    break;
            }

            return state;
        }
        public static ParamType SetType(string key)
        {
            ParamType type;

            switch (key)
            {
                case "NO2":
                    type = ParamType.NO2;
                    break;

                case "PM10":
                    type = ParamType.PM10;
                    break;

                case "PM2.5":
                    type = ParamType.PM25;
                    break;

                case "O3":
                    type = ParamType.O3;
                    break;

                case "CO":
                    type = ParamType.CO;
                    break;

                case "C6H6":
                    type = ParamType.C6H6;
                    break;

                case "SO2":
                    type = ParamType.SO2;
                    break;

                default:
                    type = ParamType.Unknown;
                    break;
            }

            return type;
        }
        public static State MedianState(IEnumerable<Station> stations)
        {
            IEnumerable<Sensor> sensors = stations.SelectMany(station => station.sensors);
            State state = State.Unknown;

            if (sensors != null && sensors.Count() > 1)
            {
                IEnumerable<State> states = sensors
                    .Where(sensor => sensor.state != State.Unknown)
                    .OrderBy(sensor => sensor.state)
                    .Select(x => x.state);

                int middle = (states.Count() / 2) - 1;

                state = states.ElementAt(middle);
            } else if (sensors.Count() == 1)
            {
                state = sensors.ElementAt(0).state;
            }

            return state;
        }
    }
}
