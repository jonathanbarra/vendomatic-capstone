﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone.Classes
{
    public class PurchaseMenu : MainMenu
    {
        public void Display(VendingMachine vendingMachine, List<VendingMachineItem> customer, MainMenu mainmenu, PurchaseMenu purchaseMenu)
        {
            try
            {
                // variables for keeping trak of various user inputs and interactions
                bool doneWithMenu = false;
                string input;
                int addedMoney;
                bool stillShopping = true;
                while (!doneWithMenu)
                {
                    Console.WriteLine();
                    Console.WriteLine("Purchase Menu");
                    Console.WriteLine();
                    Console.WriteLine($"Current Balance: ${vendingMachine.Balance}");
                    Console.WriteLine();
                    Console.WriteLine("1] >> Insert Money");
                    Console.WriteLine("2] >> Purchase Items");
                    Console.WriteLine("3] >> Return To Main Menu");
                    Console.WriteLine();
                    Console.Write("What option do you want to select? ");
                    input = Console.ReadLine();

                    if (input == "1")
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.Write("What Bills Would You Like To Insert?(ie $1, $5, $10, $20): $"); 
                        addedMoney = int.Parse(Console.ReadLine()); // prompt user to insert bills of various denominations
                        Console.WriteLine();

                        vendingMachine.FeedMoney(addedMoney);// add the users money in to the machine's balance 
                        Console.Clear();
                    }
                    else if (input == "2")
                    {
                        Console.Clear();
                        while (stillShopping)
                        {
                            int enums = 0;// int for keeping track of current slot
                            Console.WriteLine();
                            Console.WriteLine(" ____________________________________");
                            foreach (var kvp in vendingMachine.Slots)
                            {
                                VendingMachineItem vmi = vendingMachine.GetItemAtSlot(kvp);// assigning the value at the current slot in the loop to a variable

                                if (vmi == null)// if - slot is empty list item as sold out
                                {
                                    Console.WriteLine($"| {vendingMachine.Slots.GetValue(enums)} - SOLD OUT                      |");
                                }
                                else// else - list key, item, price and quantity
                                {
                                    Console.WriteLine($"| {vendingMachine.Slots.GetValue(enums)} | {vendingMachine.GetItemAtSlot(kvp).ItemName.PadRight(18)} | {vendingMachine.GetItemAtSlot(kvp).Price} | {vendingMachine.GetQuantityRemaining(kvp)} |");
                                }
                                enums++;// enumerator
                            }
                            Console.WriteLine(" ____________________________________");
                            Console.WriteLine();
                            Console.WriteLine($"Current Balance: ${vendingMachine.Balance}");
                            Console.WriteLine();
                            Console.Write("Which item would you like to purchase? ");
                            input = Console.ReadLine();
                            Console.WriteLine();

                            vendingMachine.Purchase(input, vendingMachine, customer); // perform purchase

                            Console.WriteLine();
                            Console.Write("Are you done shopping?(y/n): "); // ask user if they would like to keep shopping or not
                            input = Console.ReadLine();

                            if (input.ToLower() == "y")
                            {
                                stillShopping = false;
                            }
                            else
                            {
                                stillShopping = true;
                            }6
                        }
                    }
                    else if (input == "3")
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Returning To Main Menu");
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please Select A Valid Menu Option");
                    }
                }
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine();
                Console.WriteLine("Please Make Another Selection");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine();
                Console.WriteLine("Please Make Another Selection");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine();
                Console.WriteLine("Please Make Another Selection");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu);

            }
            catch (FormatException ex)
            {
                Console.WriteLine();
                Console.WriteLine("Please Enter Whole Dollar Amounts(ie $1, $5, $10, $20)");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu);
            }
            catch(OverflowException)
            {
                Console.WriteLine();
                Console.WriteLine("Machine Can't Handle That Much Money!");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu);
            }
        }
    }
}



