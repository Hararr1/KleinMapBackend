using KleinMapLibrary.Enums;

namespace KleinMapLibrary.Managers
{
    public static class PrepareDataManager
    {
        public static State SetState(ParamType type, double worstValue)
        {
            State state = State.Unknown;

            switch (type)
            {
                case ParamType.Unknown:
                    state = State.Unknown;
                    break;

                case ParamType.NO2:

                    if (worstValue <= 40)
                    {
                        state = State.VeryGood;
                    }
                    else if (worstValue >= 40.1 && worstValue <= 100)
                    {
                        state = State.Good;
                    }
                    else if (worstValue >= 100.1 && worstValue <= 150)
                    {
                        state = State.OK;
                    }
                    else if (worstValue >= 150.1 && worstValue <= 200)
                    {
                        state = State.Warning;
                    }
                    else if (worstValue >= 200.1 && worstValue <= 400)
                    {
                        state = State.NonFatal;
                    }
                    else if (worstValue > 400)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.PM10:

                    if (worstValue >= 0 && worstValue <= 20)
                    {
                        state = State.VeryGood;
                    }
                    else if (worstValue <= 20.1 && worstValue <= 50)
                    {
                        state = State.Good;
                    }
                    else if (worstValue <= 50.1 && worstValue <= 80)
                    {
                        state = State.OK;
                    }
                    else if (worstValue <= 80.1 && worstValue <= 110)
                    {
                        state = State.Warning;
                    }
                    else if (worstValue <= 110.1 && worstValue <= 150)
                    {
                        state = State.NonFatal;
                    }
                    else if (worstValue > 150)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.PM25:

                    if (worstValue >= 0 && worstValue <= 13)
                    {
                        state = State.VeryGood;
                    }
                    else if (worstValue <= 13.1 && worstValue <= 35)
                    {
                        state = State.Good;
                    }
                    else if (worstValue <= 35.1 && worstValue <= 55)
                    {
                        state = State.OK;
                    }
                    else if (worstValue <= 55.1 && worstValue <= 75)
                    {
                        state = State.Warning;
                    }
                    else if (worstValue <= 75.1 && worstValue <= 110)
                    {
                        state = State.NonFatal;
                    }
                    else if (worstValue > 110)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.O3:

                    if (worstValue >= 0 && worstValue <= 70)
                    {
                        state = State.VeryGood;
                    }
                    else if (worstValue <= 70.1 && worstValue <= 120)
                    {
                        state = State.Good;
                    }
                    else if (worstValue <= 120.1 && worstValue <= 150)
                    {
                        state = State.OK;
                    }
                    else if (worstValue <= 150.1 && worstValue <= 180)
                    {
                        state = State.Warning;
                    }
                    else if (worstValue <= 180.1 && worstValue <= 240)
                    {
                        state = State.NonFatal;
                    }
                    else if (worstValue > 240)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.CO:

                    if (worstValue >= 0 && worstValue <= 3)
                    {
                        state = State.VeryGood;
                    }
                    else if (worstValue <= 3.1 && worstValue <= 7)
                    {
                        state = State.Good;
                    }
                    else if (worstValue <= 7.1 && worstValue <= 11)
                    {
                        state = State.OK;
                    }
                    else if (worstValue <= 11.1 && worstValue <= 15)
                    {
                        state = State.Warning;
                    }
                    else if (worstValue <= 15.1 && worstValue <= 21)
                    {
                        state = State.NonFatal;
                    }
                    else if (worstValue > 21)
                    {
                        state = State.Fatal;
                    }

                    break;

                case ParamType.C6H6:

                    if (worstValue >= 0 && worstValue <= 6)
                    {
                        state = State.VeryGood;
                    }
                    else if (worstValue <= 6.1 && worstValue <= 11)
                    {
                        state = State.Good;
                    }
                    else if (worstValue <= 11.1 && worstValue <= 16)
                    {
                        state = State.OK;
                    }
                    else if (worstValue <= 16.1 && worstValue <= 21)
                    {
                        state = State.Warning;
                    }
                    else if (worstValue <= 21.1 && worstValue <= 51)
                    {
                        state = State.NonFatal;
                    }
                    else if (worstValue > 51)
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

                default:
                    type = ParamType.Unknown;
                    break;
            }

            return type;
        }
    }
}
