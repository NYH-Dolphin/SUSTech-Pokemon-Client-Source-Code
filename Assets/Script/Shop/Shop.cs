using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace Script.Shop
{
    public class Shop
    {
        private bool _firstInitial = false;

        public bool FirstInitial
        {
            get => _firstInitial;
            set => _firstInitial = value;
        }

        private List<Item> _items = new List<Item>();

        public List<Item> Items
        {
            get => _items;
            set => _items = value;
        }

        private static Shop _instance = new Shop();

        public static Shop getInstance()
        {
            return _instance;
        }

        public static void SetItems(JsonData jsonData)
        {
            _instance._items = new List<Item>();
            for (int i = 0; i < jsonData.Count; i++)
            {
                JsonData jsonItem = jsonData[i]["item"];
                Item item = new Item();
                item.Price = int.Parse(jsonData[i]["price"].ToString());
                item.ID = int.Parse(jsonItem["id"].ToString());
                item.Name = jsonItem["name"].ToString();
                item.Name_EN = jsonItem["name_en"].ToString();
                item.Description = jsonItem["description"].ToString();
                item.Description_EN = jsonItem["description_en"].ToString();
                _instance.Items.Add(item);
            }

            _instance.FirstInitial = true;
        }
    }
}