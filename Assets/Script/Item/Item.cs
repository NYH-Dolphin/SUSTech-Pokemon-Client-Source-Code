public class Item
{
    protected string _name;
    protected string _name_EN;
    protected int _id;
    protected string _description;
    protected string _description_EN;
    protected int _price;
    protected string _quality;
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public string Name_EN
    {
        get => _name_EN;
        set => _name_EN = value;
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
    
    public string Description_EN
    {
        get => _description_EN;
        set => _description_EN = value;
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
        _name = name;
        _id = id;
        _description = description;
    }

    
    public Item(string name, int id)
    {
        _name = name;
        _id = id;
    }

    public Item()
    {
        
    }
}