AI USAGE

Why I used AI for these parts

As a student learning C#, I used AI as a guide to help me turn my ideas into actual code. I specifically needed help with "input errors" because I didn't know how to stop the program from crashing when a user accidentally types a letter instead of a number. I also used it to figure out the logic for the shopping cart, like how to check for duplicate items and how to update the stock correctly. AI helped me find small mistakes in Visual Studio that were hard for me to spot.

What questions did I ask?
I asked the AI these specific questions to guide my development:

"How can I use int.TryParse so my program doesn't crash if a user enters a letter instead of a number?".

"Can you help me find the logic error in my C# shopping cart loop where my stock isn't updating correctly?".

"How do I check if a product is already in my cart array so I can just add to the quantity instead of making a new row?".

"What is the code to give a 10% discount if the total reaches ₱5,000 or more?".

"How do I subtract the bought items from the RemainingStock after the user finishes their order?".

What I changed or improved after using AI

After the AI gave me suggestions, I made several changes to make sure the code followed the project rules. I manually set the discount to start exactly at the ₱5,000 mark. I also moved the DisplayProduct() code into the Product class to keep it more organized. Finally, I wrote my own error messages like "Not enough stock available" and "Cart is full" to make the program easier for people to use.
