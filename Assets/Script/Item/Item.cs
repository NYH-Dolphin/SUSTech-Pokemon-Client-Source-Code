public class Item
{
    protected string name;
    protected int id;
    protected string description;
    protected int cost;
    protected string quality;
    public string Name
    {
        get => name;
        set => name = value;
    }

    public int ID
    {
        get => id;
        set => id = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public int Cost
    {
        get => cost;
        set => cost = value;
    }

    public string Quality
    {
        get => quality;
        set => quality = value;
    }

    public int GetId()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }
    
    public Item(string name, int id, string description)
    {
        this.name = name;
        this.id = id;
        this.description = description;
    }

    
    public Item(string name, int id)
    {
        this.name = name;
        this.id = id;
    }

    public Item()
    {
        
    }
}