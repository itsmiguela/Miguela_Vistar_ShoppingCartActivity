Enhance Shopping Cart Activity

This project is a simple console-based shopping system made in C#.
It allows users to browse products, add items to a cart, and complete purchases with a receipt.

Part 2: Features
This version improves the system by making it more organized, accurate, and easier to use. It focuses on better cart control, correct stock handling, and a complete checkout process.

Product Display

Show all products:
Product Display

Show all products:
foreach (var p in products)
{
p.Display();
}

Search product by name:
string search = Console.ReadLine().ToLower();

foreach (var p in products)
{
if (p.Name.ToLower().Contains(search))
p.Display();
}


Product categories:
Appliances
Electronics


Cart Features
Add item to cart:
cart[count] = selected;
qty[count++] = q;

Avoid duplicate items:
int index = FindItem(cart, count, selected.Id);
if (index != -1)
qty[index] += q;


Update quantity:
int difference = newQty - oldQty;
cart[u].RemainingStock -= difference;
qty[u] = newQty;


Remove item:
cart[r].RemainingStock += qty[r];

for (int i = r; i < count - 1; i++)
{
cart[i] = cart[i + 1];
qty[i] = qty[i + 1];
}
count--;


Clear cart:
for (int i = 0; i < count; i++)
cart[i].RemainingStock += qty[i];

count = 0;
Stock check:
if (q > selected.RemainingStock)
{
Console.WriteLine("Not enough stock.");
}



Checkout Process
Receipt output:
Console.WriteLine($"{cart[i].Name} x{qty[i]} = ₱{sub:F2}");

Discount (10% if ₱5000+):
double discount = total >= 5000 ? total * 0.10 : 0;

Payment validation:
if (double.TryParse(Console.ReadLine(), out payment) && payment >= finalTotal)

Change computation:
Console.WriteLine($"Change: ₱{payment - finalTotal:F2}");


Order History
history[historyCount++] = new Order
{
ReceiptNo = receiptCounter - 1,
Date = DateTime.Now,
FinalTotal = finalTotal
};
Console.WriteLine($"Receipt #{history[i].ReceiptNo:0000} | {history[i].Date}");

Stock System
Reduce stock when buying:
selected.RemainingStock -= q;

Restore stock when removing:
cart[r].RemainingStock += qty[r];

Low stock alert:
if (p.RemainingStock <= 5)
{
Console.WriteLine($"{p.Name} is low on stock.");
}



AI Usage
AI was used as a guide to help improve the program, fix errors, and understand some coding logic.

Questions asked:
How to use TryParse for input validation
How to manage cart items properly
How to update stock correctly
How to apply discounts
How to validate payment



Conclusion
This project helped apply basic programming concepts such as arrays, loops, input validation, cart management, and stock control. It improved both coding logic and problem-solving skills in C#.
