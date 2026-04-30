using System;
using System.Text;

class Product
{
    public int Id;
    public string Name;
    public string Category;
    public double Price;
    public int RemainingStock;

    public Product(int id, string name, string category, double price, int stock)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
        RemainingStock = stock;
    }

    public void Display()
    {
        Console.WriteLine($"{Id}. {Name} ({Category}) - ₱{Price:F2} | Stock: {RemainingStock}");
    }
}

class Order
{
    public int ReceiptNo;
    public DateTime Date;
    public double FinalTotal;

    public void Show()
    {
        Console.WriteLine($"Receipt #{ReceiptNo:0000} | {Date:MMM dd, yyyy hh:mm tt} | ₱{FinalTotal:F2}");
    }
}

class Program
{
    static int receiptCounter = 1;
    static Order[] history = new Order[100];
    static int historyCount = 0;

    static void Main()
    {
        // ✅ FIX: Properly display ₱ symbol
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Product[] products =
        {
            new Product(1,"Washing Machine","Appliance",14999,6),
            new Product(2,"Refrigerator","Appliance",9999,9),
            new Product(3,"Television","Electronics",20990,5),
            new Product(4,"Stove","Appliance",6610,10),
            new Product(5,"Aircon","Appliance",38000,8),
            
        };

        int[] cartId = new int[10];
        int[] cartQty = new int[10];
        int cartCount = 0;

        while (true)
        {
            Console.WriteLine("\n===== MAIN MENU =====");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Add to Cart");
            Console.WriteLine("3. Manage Cart");
            Console.WriteLine("4. Checkout");
            Console.WriteLine("5. Order History");
            Console.WriteLine("0. Exit");

            int choice = ReadInt("Choose: ");

            if (choice == 1)
            {
                foreach (var p in products) p.Display();
            }

            else if (choice == 2)
            {
                foreach (var p in products) p.Display();

                int id = ReadInt("Product #: ");
                Product selected = Find(products, id);

                if (selected == null || selected.RemainingStock == 0)
                {
                    Console.WriteLine("Invalid or out of stock.");
                    continue;
                }

                int qty = ReadInt("Quantity: ");

                if (qty > selected.RemainingStock)
                {
                    Console.WriteLine("Not enough stock.");
                    continue;
                }

                int index = FindIndex(cartId, cartCount, id);

                if (index != -1)
                    cartQty[index] += qty;
                else
                {
                    cartId[cartCount] = id;
                    cartQty[cartCount] = qty;
                    cartCount++;
                }

                selected.RemainingStock -= qty;
                Console.WriteLine("Added to cart!");
            }

            else if (choice == 3)
            {
                ManageCart(products, cartId, cartQty, ref cartCount);
            }

            else if (choice == 4)
            {
                cartCount = Checkout(products, cartId, cartQty, cartCount);
            }

            else if (choice == 5)
            {
                Console.WriteLine("\n===== ORDER HISTORY =====");
                for (int i = 0; i < historyCount; i++)
                    history[i].Show();
            }

            else if (choice == 0)
                break;
        }
    }

    static void ManageCart(Product[] products, int[] id, int[] qty, ref int count)
    {
        while (true)
        {
            Console.WriteLine("\n===== CART MENU =====");
            Console.WriteLine("1. View Cart");
            Console.WriteLine("2. Update Quantity");
            Console.WriteLine("3. Remove Item");
            Console.WriteLine("4. Clear Cart");
            Console.WriteLine("5. Back");

            int choice = ReadInt("Choose: ");

            if (choice == 1)
                ShowCart(products, id, qty, count);

            else if (choice == 2)
            {
                int i = ReadInt("Item #: ") - 1;

                if (i < 0 || i >= count)
                {
                    Console.WriteLine("Invalid item.");
                    continue;
                }

                int newQty = ReadInt("New quantity: ");
                Product p = Find(products, id[i]);

                int diff = newQty - qty[i];

                if (diff > p.RemainingStock)
                {
                    Console.WriteLine("Not enough stock.");
                    continue;
                }

                p.RemainingStock -= diff;
                qty[i] = newQty;
            }

            else if (choice == 3)
            {
                int r = ReadInt("Remove item #: ") - 1;

                if (r < 0 || r >= count)
                {
                    Console.WriteLine("Invalid item.");
                    continue;
                }

                Product p = Find(products, id[r]);
                p.RemainingStock += qty[r];

                for (int i = r; i < count - 1; i++)
                {
                    id[i] = id[i + 1];
                    qty[i] = qty[i + 1];
                }

                count--;
            }

            else if (choice == 4)
            {
                for (int i = 0; i < count; i++)
                {
                    Product p = Find(products, id[i]);
                    p.RemainingStock += qty[i];
                }

                count = 0;
                Console.WriteLine("Cart cleared.");
            }

            else if (choice == 5)
                return;
        }
    }

    static int Checkout(Product[] products, int[] id, int[] qty, int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return count;
        }

        double total = 0;

        Console.WriteLine("\n===== RECEIPT =====");
        Console.WriteLine($"Receipt No: {receiptCounter:0000}");
        Console.WriteLine($"Date: {DateTime.Now:MMMM dd, yyyy hh:mm tt}");

        for (int i = 0; i < count; i++)
        {
            Product p = Find(products, id[i]);
            double sub = p.Price * qty[i];
            Console.WriteLine($"{p.Name} x{qty[i]} = ₱{sub:F2}");
            total += sub;
        }

        double discount = total >= 5000 ? total * 0.10 : 0;
        double final = total - discount;

        Console.WriteLine($"Grand Total: ₱{total:F2}");
        Console.WriteLine($"Discount: ₱{discount:F2}");
        Console.WriteLine($"Final Total: ₱{final:F2}");

        double payment;

        while (true)
        {
            Console.Write("Enter payment: ");
            if (double.TryParse(Console.ReadLine(), out payment) && payment >= final)
                break;

            Console.WriteLine("Invalid or insufficient payment.");
        }

        Console.WriteLine($"Change: ₱{payment - final:F2}");

        history[historyCount++] = new Order
        {
            ReceiptNo = receiptCounter,
            Date = DateTime.Now,
            FinalTotal = final
        };

        receiptCounter++;

        Console.WriteLine("\nLOW STOCK ALERT:");
        foreach (var p in products)
            if (p.RemainingStock <= 5)
                Console.WriteLine($"{p.Name} only {p.RemainingStock} left.");

        return 0;
    }

    static void ShowCart(Product[] products, int[] id, int[] qty, int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        double total = 0;

        for (int i = 0; i < count; i++)
        {
            Product p = Find(products, id[i]);
            double sub = p.Price * qty[i];
            Console.WriteLine($"{i + 1}. {p.Name} x{qty[i]} = ₱{sub:F2}");
            total += sub;
        }

        Console.WriteLine($"Total: ₱{total:F2}");
    }

    static Product Find(Product[] p, int id)
    {
        foreach (var x in p)
            if (x.Id == id) return x;
        return null;
    }

    static int FindIndex(int[] id, int count, int pid)
    {
        for (int i = 0; i < count; i++)
            if (id[i] == pid) return i;
        return -1;
    }

    static int ReadInt(string msg)
    {
        int v;
        while (true)
        {
            Console.Write(msg);
            if (int.TryParse(Console.ReadLine(), out v))
                return v;

            Console.WriteLine("Invalid input. Enter a number.");
        }
    }
}
