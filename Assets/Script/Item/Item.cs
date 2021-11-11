public class Item
{
    protected string _name;
    protected int _id;
    protected string _description;
    protected int _price;
    protected string _quality;
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public int ID
    {
        get => _id;
        set => _id = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public int Price
    {
        get => _price;
        set => _price = value;
    }

    public string Quality
    {
        get => _quality;
        set => _quality = value;
    }

    public int GetId()
    {
        return _id;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetDescription()
    {
        return _description;
    }
    
    public Item(string name, int id, string description)
    {
        this._name = name;
        this._id = id;
        this._description = description;
    }

    
    public Item(string name, int id)
    {
        this._name = name;
        this._id = id;
    }

    public Item()
    {
        
    }
}