
using System;

public static class Constant
{
    public class Event
    {

    }

    public class Data
    {
        public const int MAX_STAGE_COUNT = 30;
    }
}

[Flags]
public enum LayerKind
{
    Red_Layer,
    Green_Layer,
    Blue_Layer,
    Yellow_Layer,   // ??
}