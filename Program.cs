using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int Stock;

    public Product(int id, string name, double price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
    }

    public void Display()
    {
        Console.WriteLine(Id + ". " + Name + " - ₱" + Price + " (Stock: " + Stock + ")");
    }
}

class Program
{
    static void Main()
    {
        Product[] products =
        {
            new Product(1, "Washing Machine", 14999, 6),
            new Product(2, "Refrigerator", 9999, 9),
            new Product(3, "Television", 20990, 5),
            new Product(4, "Stove", 6610, 10),
            new Product(5, "Aircon", 38000, 8)
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
            int mainChoice;

            if (!int.TryParse(Console.ReadLine(), out mainChoice))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            if (mainChoice == 3) break;

            // =====================
            // PART 1
            // =====================
            if (mainChoice == 1)
            {
                string choice = "Y";

                while (choice == "Y")
                {
                    Console.WriteLine("\n--- PRODUCTS ---");
                    for (int i = 0; i < products.Length; i++)
                        products[i].Display();

                    Console.Write("Enter product number: ");
                    int num;

                    if (!int.TryParse(Console.ReadLine(), out num) || num < 1 || num > products.Length)
                    {
                        Console.WriteLine("Invalid product.");
                        continue;
                    }

                    Product p = products[num - 1];

                    Console.Write("Enter quantity: ");
                    int qty;

                    if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                    {
                        Console.WriteLine("Invalid quantity.");
                        continue;
                    }

                    if (qty > p.Stock)
                    {
                        Console.WriteLine("Not enough stock.");
                        continue;
                    }

                    cartIds[cartCount] = p.Id;
                    cartQty[cartCount] = qty;
                    cartTotal[cartCount] = qty * p.Price;
                    cartCount++;

                    p.Stock -= qty;

                    Console.Write("Add another item? (Y/N): ");
                    choice = ReadYN();
                }

                // SIMPLE RECEIPT
                Console.WriteLine("\n--- RECEIPT ---");
                double total = 0;

                for (int i = 0; i < cartCount; i++)
                {
                    Product p = Find(products, cartIds[i]);
                    Console.WriteLine(p.Name + " x" + cartQty[i] + " - ₱" + cartTotal[i]);
                    total += cartTotal[i];
                }

                Console.WriteLine("Total: ₱" + total);
                cartCount = 0;
            }

            // =====================
            // PART 2
            // =====================
            else if (mainChoice == 2)
            {
                while (true)
                {
                    Console.WriteLine("\n==== PART 2 MENU ====");
                    Console.WriteLine("1. Add Item");
                    Console.WriteLine("2. View Cart");
                    Console.WriteLine("3. Remove Item");
                    Console.WriteLine("4. Update Quantity");
                    Console.WriteLine("5. Clear Cart");
                    Console.WriteLine("6. Checkout");
                    Console.WriteLine("7. Order History");
                    Console.WriteLine("8. Back");

                    Console.Write("Choose: ");
                    int choice;

                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid input.");
                        continue;
                    }

                    if (choice == 8) break;

                    // ADD ITEM
                    if (choice == 1)
                    {
                        Console.WriteLine("\n--- PRODUCTS ---");
                        for (int i = 0; i < products.Length; i++)
                            products[i].Display();

                        Console.Write("Enter product number: ");
                        int num;

                        if (!int.TryParse(Console.ReadLine(), out num) || num < 1 || num > products.Length)
                        {
                            Console.WriteLine("Invalid product.");
                            continue;
                        }

                        Product p = products[num - 1];

                        Console.Write("Enter quantity: ");
                        int qty;

                        if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                        {
                            Console.WriteLine("Invalid quantity.");
                            continue;
                        }

                        if (qty > p.Stock)
                        {
                            Console.WriteLine("Not enough stock.");
                            continue;
                        }

                        cartIds[cartCount] = p.Id;
                        cartQty[cartCount] = qty;
                        cartTotal[cartCount] = qty * p.Price;
                        cartCount++;

                        p.Stock -= qty;
                    }

                    // VIEW CART
                    else if (choice == 2)
                    {
                        Console.WriteLine("\n--- CART ---");
                        for (int i = 0; i < cartCount; i++)
                        {
                            Product p = Find(products, cartIds[i]);
                            Console.WriteLine((i + 1) + ". " + p.Name + " x" + cartQty[i]);
                        }
                    }

                    // REMOVE
                    else if (choice == 3)
                    {
                        Console.Write("Item #: ");
                        int index;

                        if (int.TryParse(Console.ReadLine(), out index) && index > 0 && index <= cartCount)
                        {
                            for (int i = index - 1; i < cartCount - 1; i++)
                            {
                                cartIds[i] = cartIds[i + 1];
                                cartQty[i] = cartQty[i + 1];
                                cartTotal[i] = cartTotal[i + 1];
                            }
                            cartCount--;
                        }
                    }

                    // UPDATE
                    else if (choice == 4)
                    {
                        Console.Write("Item #: ");
                        int index;

                        if (int.TryParse(Console.ReadLine(), out index) && index > 0 && index <= cartCount)
                        {
                            Console.Write("New quantity: ");
                            int qty;

                            if (int.TryParse(Console.ReadLine(), out qty))
                            {
                                Product p = Find(products, cartIds[index - 1]);

                                if (qty > p.Stock)
                                {
                                    Console.WriteLine("Not enough stock.");
                                    continue;
                                }

                                cartQty[index - 1] = qty;
                                cartTotal[index - 1] = qty * p.Price;
                            }
                        }
                    }

                    // CLEAR
                    else if (choice == 5)
                    {
                        cartCount = 0;
                        Console.WriteLine("Cart cleared.");
                    }

                    // CHECKOUT
                    else if (choice == 6)
                    {
                        double total = 0;

                        for (int i = 0; i < cartCount; i++)
                            total += cartTotal[i];

                        double discount = total >= 5000 ? total * 0.10 : 0;
                        double finalTotal = total - discount;

                        Console.WriteLine("\n--- RECEIPT ---");
                        Console.WriteLine("Receipt No: " + receiptNo);
                        Console.WriteLine("Date: " + DateTime.Now);

                        Console.WriteLine("Final Total: ₱" + finalTotal);

                        double payment;
                        while (true)
                        {
                            Console.Write("Enter payment: ");
                            if (double.TryParse(Console.ReadLine(), out payment) && payment >= finalTotal)
                                break;

                            Console.WriteLine("Invalid or insufficient.");
                        }

                        Console.WriteLine("Change: ₱" + (payment - finalTotal));

                        history[historyCount++] = finalTotal;
                        receiptNo++;

                        Console.WriteLine("\nLOW STOCK:");
                        for (int i = 0; i < products.Length; i++)
                        {
                            if (products[i].Stock <= 5)
                                Console.WriteLine(products[i].Name + " has " + products[i].Stock + " left.");
                        }

                        cartCount = 0;
                    }

                    // HISTORY
                    else if (choice == 7)
                    {
                        Console.WriteLine("\n--- ORDER HISTORY ---");
                        for (int i = 0; i < historyCount; i++)
                        {
                            Console.WriteLine("Order " + (i + 1) + " - ₱" + history[i]);
                        }
                    }
                }
            }
        }
    }

    static string ReadYN()
    {
        while (true)
        {
            string input = Console.ReadLine().ToUpper();
            if (input == "Y" || input == "N") return input;
            Console.Write("Enter Y or N: ");
        }
    }

    static Product Find(Product[] products, int id)
    {
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].Id == id)
                return products[i];
        }
        return null;
    }
}
