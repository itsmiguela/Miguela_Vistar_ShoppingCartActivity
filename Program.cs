using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int Stock;
    public string Category;

    public Product(int id, string name, double price, int stock, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
        Category = category;
    }

    public void Display()
    {
        Console.WriteLine($"{Id}. [{Category}] {Name} - ₱{Price} (Stock: {Stock})");
    }
}

class Program
{
    static void Main()
    {
        Product[] products =
        {
            new Product(1, "Washing Machine", 14999, 6, "Appliances"),
            new Product(2, "Refrigerator", 9999, 9, "Appliances"),
            new Product(3, "Television", 20990, 5, "Electronics"),
            new Product(4, "Stove", 6610, 10, "Appliances"),
            new Product(5, "Aircon", 38000, 8, "Electronics")
        };

        int[] cartIds = new int[10];
        int[] cartQty = new int[10];
        double[] cartTotal = new double[10];
        int cartCount = 0;

        double[] history = new double[20];
        int historyCount = 0;
        int receiptNo = 1;

        while (true)
        {
            Console.WriteLine("\n==== MAIN MENU ====");
            Console.WriteLine("1. PART 1 - Basic Shopping");
            Console.WriteLine("2. PART 2 - Enhanced System");
            Console.WriteLine("3. Exit");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int mainChoice)) continue;
            if (mainChoice == 3) break;

            if (mainChoice == 1 || mainChoice == 2)
            {
                RunShoppingSystem(mainChoice, products, cartIds, cartQty, cartTotal,
                    ref cartCount, history, ref historyCount, ref receiptNo);
            }
        }
    }

    static void RunShoppingSystem(int part, Product[] products,
        int[] cartIds, int[] cartQty, double[] cartTotal,
        ref int cartCount, double[] history,
        ref int historyCount, ref int receiptNo)
    {
        while (true)
        {
            Console.WriteLine(part == 1 ? "\n--- PART 1 MENU ---" : "\n==== PART 2 MENU ====");

            if (part == 2)
            {
                Console.WriteLine("1. Search & Add Item\n2. View Cart\n3. Remove Item\n4. Update Quantity\n5. Clear Cart\n6. Checkout\n7. Order History\n8. Back");
            }
            else
            {
                Console.WriteLine("1. Add Item\n2. View Receipt & Exit Part 1");
            }

            Console.Write("Choose: ");
            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            if (part == 2 && choice == 8) break;
            if (part == 1 && choice == 2)
            {
                Checkout(products, cartIds, cartQty, cartTotal,
                    ref cartCount, history, ref historyCount, ref receiptNo, false);
                break;
            }

            // ADD ITEM
            if (choice == 1)
            {
                if (cartCount >= 10)
                {
                    Console.WriteLine("Cart full!");
                    continue;
                }

                Console.WriteLine("\n--- PRODUCTS ---");
                foreach (var p in products) p.Display();

                Console.Write("\nSearch (optional): ");
                string search = Console.ReadLine().ToLower();

                if (!string.IsNullOrEmpty(search))
                {
                    Console.WriteLine("Results:");
                    foreach (var p in products)
                        if (p.Name.ToLower().Contains(search)) p.Display();
                }

                Console.Write("Enter Product ID: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    Product p = Find(products, id);
                    if (p != null)
                    {
                        Console.Write("Quantity: ");
                        if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0 && qty <= p.Stock)
                        {
                            cartIds[cartCount] = p.Id;
                            cartQty[cartCount] = qty;
                            cartTotal[cartCount] = qty * p.Price;
                            cartCount++;
                            p.Stock -= qty;
                            Console.WriteLine("Added.");
                        }
                        else Console.WriteLine("Invalid quantity.");
                    }
                }
            }

            // VIEW CART
            else if (choice == 2 && part == 2)
            {
                Console.WriteLine("\n--- CART ---");
                for (int i = 0; i < cartCount; i++)
                {
                    Product p = Find(products, cartIds[i]);
                    Console.WriteLine($"{i + 1}. {p.Name} x{cartQty[i]} - ₱{cartTotal[i]}");
                }
            }

            // REMOVE
            else if (choice == 3 && part == 2)
            {
                Console.Write("Item #: ");
                if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= cartCount)
                {
                    Product p = Find(products, cartIds[idx - 1]);
                    p.Stock += cartQty[idx - 1];

                    for (int i = idx - 1; i < cartCount - 1; i++)
                    {
                        cartIds[i] = cartIds[i + 1];
                        cartQty[i] = cartQty[i + 1];
                        cartTotal[i] = cartTotal[i + 1];
                    }
                    cartCount--;
                }
            }

            // UPDATE QUANTITY (FIXED)
            else if (choice == 4 && part == 2)
            {
                Console.Write("Item #: ");
                if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= cartCount)
                {
                    Product p = Find(products, cartIds[idx - 1]);

                    Console.Write("New quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int newQty) && newQty > 0)
                    {
                        int currentQty = cartQty[idx - 1];
                        int availableStock = p.Stock + currentQty;

                        if (newQty > availableStock)
                        {
                            Console.WriteLine("Not enough stock.");
                            continue;
                        }

                        p.Stock += currentQty;
                        p.Stock -= newQty;

                        cartQty[idx - 1] = newQty;
                        cartTotal[idx - 1] = newQty * p.Price;

                        Console.WriteLine("Updated.");
                    }
                }
            }

            // CLEAR
            else if (choice == 5 && part == 2)
            {
                for (int i = 0; i < cartCount; i++)
                    Find(products, cartIds[i]).Stock += cartQty[i];

                cartCount = 0;
                Console.WriteLine("Cart cleared.");
            }

            // CHECKOUT
            else if (choice == 6)
            {
                Checkout(products, cartIds, cartQty, cartTotal,
                    ref cartCount, history, ref historyCount, ref receiptNo, true);
            }

            // HISTORY
            else if (choice == 7 && part == 2)
            {
                Console.WriteLine("\n--- HISTORY ---");
                for (int i = 0; i < historyCount; i++)
                    Console.WriteLine($"Receipt #{i + 1:D4} - ₱{history[i]}");
            }
        }
    }

    static void Checkout(Product[] products, int[] cartIds, int[] cartQty,
        double[] cartTotal, ref int cartCount, double[] history,
        ref int historyCount, ref int receiptNo, bool full)
    {
        if (cartCount == 0)
        {
            Console.WriteLine("Cart empty.");
            return;
        }

        double total = 0;
        for (int i = 0; i < cartCount; i++) total += cartTotal[i];

        double discount = total >= 5000 ? total * 0.10 : 0;
        double finalTotal = total - discount;

        Console.WriteLine($"\nReceipt #{receiptNo:D4}");
        Console.WriteLine($"Date: {DateTime.Now}");

        for (int i = 0; i < cartCount; i++)
        {
            Product p = Find(products, cartIds[i]);
            Console.WriteLine($"{p.Name} x{cartQty[i]} - ₱{cartTotal[i]}");
        }

        Console.WriteLine($"Total: ₱{total}");
        Console.WriteLine($"Discount: ₱{discount}");
        Console.WriteLine($"Final: ₱{finalTotal}");

        if (full)
        {
            double payment;
            while (true)
            {
                Console.Write("Payment: ");
                if (double.TryParse(Console.ReadLine(), out payment) && payment >= finalTotal)
                    break;
                Console.WriteLine("Invalid.");
            }

            Console.WriteLine($"Payment: ₱{payment}");
            Console.WriteLine($"Change: ₱{payment - finalTotal}");
        }

        history[historyCount++] = finalTotal;
        receiptNo++;

        Console.WriteLine("\nLOW STOCK:");
        foreach (var p in products)
            if (p.Stock <= 5)
                Console.WriteLine($"{p.Name} - {p.Stock} left");

        cartCount = 0;
    }

    static Product Find(Product[] products, int id)
    {
        foreach (var p in products)
            if (p.Id == id) return p;
        return null;
    }
}
