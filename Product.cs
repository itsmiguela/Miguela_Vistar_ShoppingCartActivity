using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public Product(int id, string name, double price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        RemainingStock = stock;
    }
 
    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price:N2} (Stock: {RemainingStock})");
    }

    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }

    public bool HasEnoughStock(int quantity)
    {
        return quantity <= RemainingStock;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "washing machine", Price = 14999, RemainingStock = 6 },
            new Product { Id = 2, Name = "Refrigerator", Price = 9999, RemainingStock = 9 },
            new Product { Id = 3, Name = "television", Price = 20990, RemainingStock = 5 },
            new Product { Id = 4, Name = "stove", Price = 6610, RemainingStock = 10 },
            new Product { Id = 5, Name = "Air Conditioner", Price = 38000, RemainingStock = 8 },
            new Product { Id = 6, Name = "Fan", Price = 7260, RemainingStock = 4 }
        };

        List<Product> cart = new List<Product>();
        List<int> quantities = new List<int>();
        List<double> subtotals = new List<double>();

        string choice = "Y";

        do
        {
            Console.WriteLine("\n             MENU        ");
            for (int i = 0; i < products.Length; i++)
            {
                products[i].DisplayProduct();
            }

            Console.Write("\nEnter product number: ");
            if (!int.TryParse(Console.ReadLine(), out int pNum) || pNum <= 1) || pNum > products.Length)
            {
                Console.WriteLine("Invalid input number.");
                continue;
            }


            Product selected = products[pNum - 1];

            if (selected.RemainingStock == 0)
            {
                Console.WriteLine("Out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (!selected.HasEnoughStock(qty))
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }

            double total = selected.GetItemTotal(qty);

            int foundIndex = -1;
            for (int i = 0; i < cartCount; i++)
            {
                if (cartIds[i] == selected.Id)
                {
                    foundIndex = i;
                    break;
                }
            }

            if (foundIndex != -1)
            {
                cartQty[foundIndex] += qty;
                cartSubtotal[foundIndex] += total;
            }
            else
            {
                if (cartCount == cartIds.Length)
                {
                    Console.WriteLine("Cart is full.");
                    continue;
                }

                cartIds[cartCount] = selected.Id;
                cartQty[cartCount] = qty;
                cartSubtotal[cartCount] = total;
                cartCount++;
            }

            selected.DeductStock(qty);

            Console.WriteLine("Added to cart!");

            Console.Write("Add another item? (Y/N): ");
            string input = Console.ReadLine();

            if (input == null)
                choice = "N";
            else
                choice = input.Trim().ToUpper();

        } while (choice == "Y");

        Console.WriteLine("\n            RECEIPT         ");
        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
        {
            Product p = null;

            for (int j = 0; j < products.Length; j++)
            {
                if (products[j].Id == cartIds[i])
                {
                    p = products[j];
                    break;
                }
            }

            Console.WriteLine($"{p.Name} - Qty: {cartQty[i]} - ₱{cartSubtotal[i]}");
            grandTotal += cartSubtotal[i];
        }

        Console.WriteLine($"\nGrand Total: ₱{grandTotal}");

        double discount = 0;
        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
            Console.WriteLine($"Discount (10%): -₱{discount}");
        }

        double finalTotal = grandTotal - discount;
        Console.WriteLine($"Final Total: ₱{finalTotal}");

        Console.WriteLine("\n           UPDATED STOCK        ");
        for (int i = 0; i < products.Length; i++)
        {
            products[i].DisplayProduct();
        }
    }
}
