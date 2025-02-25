using System;
using System.Collections.Generic;

[Serializable]
public class Plant
{
    public int id;
    public string name;
    public string description;
    public string sprite_name;
    public Harvest harvest;
    public Price price;
}

[Serializable]
public class Harvest
{
    public int underripe;
    public int overripe;
    public int best;
}

[Serializable]
public class Price{
    public int underripe;
    public int overripe;
    public int best;
}

[Serializable]
public class PlantList
{
    public List<Plant> plants;
}