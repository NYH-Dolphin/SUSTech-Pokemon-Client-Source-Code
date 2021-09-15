public class BookItem : Item
{ 
    protected int experience;
    
    public BookItem(string name, int id, string description)
    {
        this.name = name;
        this.id = id;
        this.description = description;
    }
}
