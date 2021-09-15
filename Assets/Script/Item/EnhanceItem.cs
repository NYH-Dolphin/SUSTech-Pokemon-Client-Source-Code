using System.Collections.Generic;

public class EnhanceItem : Item
{
    protected double rate = 1;
    protected int adder = 0;
    
    public EnhanceItem(string name, int id, string description)
    {
        this.name = name;
        this.id = id;
        this.description = description;
    }
    
    
}
