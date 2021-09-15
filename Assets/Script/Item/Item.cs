public class Item
{
    protected string name;
    protected int id;
    protected string description;
    protected int cost;

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


    public Item()
    {
        
    }
}