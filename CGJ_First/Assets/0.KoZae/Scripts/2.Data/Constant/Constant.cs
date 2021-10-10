
using System;

public static class Constant
{
    public class Event
    {

    }

    public class Data
    {
        public const int MAX_STAGE_COUNT = 10;
    }
}

[Flags]
public enum LayerKind
{
    Magenta_Layer,
    Cyan_Layer,
    Yellow_Layer,
}