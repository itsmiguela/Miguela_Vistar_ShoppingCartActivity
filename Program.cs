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
        Console.WriteLine(Id + ". " + Name + " - ₱" + Price + " (Stock: " + RemainingStock + ")");
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
        Product[] products = new Product[]
        {
            new Product(1, "Washing Machine", 14999, 6),
            new Product(2, "Refrigerator", 9999, 9),
            new Product(3, "Television", 20990, 5),
            new Product(4, "Stove", 6610, 10),
            new Product(5, "Aircon", 38000, 8)
        };

        // fixed-size cart arrays
        int[] cartIds = new int[10];
        int[] cartQty = new int[10];
        double[] cartSubtotal = new double[10];
        int cartCount = 0;

        string choice = "Y";

while (choice == "Y")
        {
            Console.WriteLine("\n===== STORE MENU =====");
            for (int i = 0; i < products.Length; i++)
            {
                products[i].DisplayProduct();
            }

            // INPUT PRODUCT
            Console.Write("Enter product number: ");
            int pNum;
            if (!int.TryParse(Console.ReadLine(), out pNum) || pNum < 1 || pNum > products.Length)
            {
                Console.WriteLine("Invalid product number.");

continue;
            }

            Product selected = products[pNum - 1];

            // OUT OF STOCK CHECK
            if (selected.RemainingStock == 0)
            {
                Console.WriteLine("Out of stock.");
                continue;
            }

            // INPUT QUANTITY
            Console.Write("Enter quantity: ");
            int qty;
            if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
            {

Console.WriteLine("Invalid quantity.");
                continue;
            }

            // STOCK VALIDATION
            if (!selected.HasEnoughStock(qty))
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }

            double total = selected.GetItemTotal(qty);

            // CHECK DUPLICATE IN CART
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
                // CHECK IF CART IS FULL
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

            // DEDUCT STOCK
            selected.DeductStock(qty);

            Console.WriteLine("Added to cart!");

            // CONTINUE?
            Console.Write("Add another item? (Y/N): ");
            string input = Console.ReadLine();
            if (input == null)
                choice = "N";
            else

choice = input.ToUpper();
        }

        // RECEIPT
        Console.WriteLine("\n===== RECEIPT =====");
        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
        {
            Product p = null;

            for (int j = 0; j < products.Length; j++)
            {
                if (products[j].Id == cartIds[i])
                {
                    p = products[j];
                }
            }

Console.WriteLine(p.Name + " - Qty: " + cartQty[i] + " - ₱" + cartSubtotal[i]);
            grandTotal += cartSubtotal[i];
        }

        Console.WriteLine("\nGrand Total: ₱" + grandTotal);

        // DISCOUNT
        double discount = 0;
        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
            Console.WriteLine("Discount (10%): -₱" + discount);
        }

double finalTotal = grandTotal - discount;
        Console.WriteLine("Final Total: ₱" + finalTotal);

        // UPDATED STOCK
        Console.WriteLine("\n===== UPDATED STOCK =====");
        for (int i = 0; i < products.Length; i++)
        {
            products[i].DisplayProduct();
        }
    }
}