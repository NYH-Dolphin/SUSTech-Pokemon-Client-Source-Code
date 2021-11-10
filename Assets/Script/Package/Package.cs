using System.Collections.Generic;

public class Package
{
    private HashMap<Item,int> _medicineItems = new HashMap<Item, int>();
    private HashMap<Item, int> _experienceItems = new HashMap<Item, int>();
    private HashMap<Item, int> _materialItems = new HashMap<Item, int>();

    public HashMap<Item, int> MedicineItems
    {
        get => _medicineItems;
        set => _medicineItems = value;
    }

    public HashMap<Item, int> ExperienceItems
    {
        get => _experienceItems;
        set => _experienceItems = value;
    }

    public HashMap<Item, int> MaterialItems
    {
        get => _materialItems;
        set => _materialItems = value;
    }
}
