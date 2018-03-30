using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos
{
    public class Pos
    {
        private List<Item> Items;
        private string inputErrorMessage;
        private Dictionary<int, BoughtItem> BoughtItems;
        private Dictionary<int, int> ItemStock;
        private int _totalPayment;
        private enum Views
        {
            Admin,
            Customer
        }
        private enum AdminActions
        {
            Exit,
            AddNewItem,
            AddStockToExistingItem
        }

        public void Start()
        {
            var input = (Views)TakeUserInput("For Admin press 0, For customer press 1", inputErrorMessage);
            switch (input)
            {
                case Views.Admin:
                    AdminView();
                    break;
                case Views.Customer:
                    CustomerView();
                    break;
            }
        }

        private void AdminView()
        {
            var input = (AdminActions)TakeUserInput("Select 0 for exit, 1 to add new item, 2 to add stock to existing item", inputErrorMessage);
            switch (input)
            {
                case AdminActions.Exit:
                    Start();
                    break;
                case AdminActions.AddNewItem:
                    AddNewItem();
                    break;
                case AdminActions.AddStockToExistingItem:
                    AddStockToExistingItem();
                    break;
            }
        }
        private void CustomerView()
        {
            _totalPayment = 0;
            BoughtItems = new Dictionary<int, BoughtItem>();
            ListOfItemsForCustomer();
            var itemId = TakeUserInput("What do you want to buy?", inputErrorMessage);
            if (itemId == 0)
            {
                Checkout();
                return;
            }
            var item = Items.Find(i => i.Id == itemId);
            var quantity = TakeUserInput("Enter quantity of " + item.Name + " to buy", inputErrorMessage);
            Purchase(item, quantity);
            CustomerView();
        }

        private void AddNewItem()
        {
            Console.WriteLine("Enter item name");
            var name = Console.ReadLine();
            var price = TakeUserInput("Enter item price", inputErrorMessage);
            var quantity = TakeUserInput("Enter item quantity", inputErrorMessage);
            var item = new Item()
            {
                Id = Items.Count + 1,
                Price = price,
                Name = name
            };
            Items.Add(item);
            ItemStock.Add(item.Id, quantity);
            ListOfItemsForAdmin();
            AdminView();
        }

        private void AddStockToExistingItem()
        {
            ListOfItemsForAdmin();
            Console.WriteLine("Enter 0 to go back");
            var itemId = TakeUserInput("Select item to add stock", inputErrorMessage);
            if (itemId == 0)
            {
                AdminView();
                return;
            }
            if (itemId <= Items.Count)
            {
                var quantity = TakeUserInput("Enter items to add", inputErrorMessage);
                if (quantity > 0)
                    ItemStock[itemId] += quantity;
                ListOfItemsForAdmin();
                AdminView();
                return;
            }
            Console.WriteLine("Please enter valid item");
            AddStockToExistingItem();
        }

        private void Purchase(Item item, int quantity)
        {
            if (quantity > ItemStock[item.Id])
                Console.WriteLine("Only " + ItemStock[item.Id] + " " + item.Name + " are in stock");
            else
            {
                ItemStock[item.Id] -= quantity;
                if (BoughtItems.ContainsKey(item.Id))
                {
                    BoughtItems[item.Id].Quantity += quantity;
                }
                else
                {
                    BoughtItems.Add(item.Id, new BoughtItem() { Item = item, Quantity = quantity });
                }
            }
        }

        private void Checkout()
        {
            Console.WriteLine("Item  Quantity UnitPrice Sum");
            foreach (var key in BoughtItems.Keys)
            {
                var boughtItem = BoughtItems[key];
                var boughtItemItemPrice = boughtItem.Quantity * boughtItem.Item.Price;
                _totalPayment += boughtItemItemPrice;
                Console.WriteLine("{0} \t {1} \t {2} \t {3}", boughtItem.Item.Name, boughtItem.Quantity, boughtItem.Item.Price, boughtItemItemPrice);
            }
            Console.WriteLine("Total Payment \t {0}", _totalPayment);
            var input = TakeUserInput("Press 1 to shop again, 0 to exit", inputErrorMessage);
            if (input == 0)
            {
                CustomerView();
                return;
            }
            Start();
        }
        private void ListOfItemsForAdmin()
        {
            Console.WriteLine("Serial ItemName Price Quantity");
            foreach (var item in Items)
            {
                Console.WriteLine("{0} \t {1}\t {2} \t{3}", item.Id, item.Name, item.Price, ItemStock[item.Id]);
            }
            
        }
        private void ListOfItemsForCustomer()
        {
            Console.WriteLine("Serial ItemName Price");
            foreach (var item in Items)
            {
                Console.WriteLine("{0} \t {1}\t {2}", item.Id, item.Name, item.Price);
            }
            Console.WriteLine("Enter 0 for checkout");
        }

        private int TakeUserInput(string inputPrompt, string errorMessage)
        {
            Console.WriteLine(inputPrompt);
            var input = Console.ReadLine();
            try
            {
                return Convert.ToInt32(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(errorMessage);
                return TakeUserInput(inputPrompt, errorMessage);
            }
        }
        public void InitDefaultValues()
        {
            Items = new List<Item>
            {
                new Item() {Id = 1, Name = "Pen", Price = 5},
                new Item() {Id = 2, Name = "Shirt", Price = 100},
                new Item() {Id = 3, Name = "Pant", Price = 50},
                new Item() {Id = 4, Name = "Soap", Price = 10},
                new Item() {Id = 5, Name = "Socks", Price = 20}
            };
            
            ItemStock = new Dictionary<int, int>
            {
                {1,10},
                {2,10},
                {3,10},
                {4,10},
                {5,10}
            };
            inputErrorMessage = "Please enter a number";
        }
    }
}

