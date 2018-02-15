﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Capstone.Classes
{
    public class MainMenu : VendingMachine
    {
        public void Display(VendingMachine vendingMachine, List<VendingMachineItem> customer, MainMenu mainmenu, VendingMachineLogger logger)
        {
            try
            {
                // variables for keeping trak of if user is still in menu and starting balance for logging
                bool stayInMenu = true;
                ConsoleKeyInfo key;

                PrintHeader();
               
                while (stayInMenu)
                {
                    MenuMusic();
                    // Main Menu
                    Console.WriteLine();
                    Console.WriteLine("Main Menu".PadLeft(15));
                    Console.WriteLine("  ________________________");
                    Console.WriteLine("1] >> Display Items");
                    Console.WriteLine("2] >> Purchase Menu");
                    Console.WriteLine("3] >> Complete Transaction");
                    Console.WriteLine("Q] >> Quit");
                    Console.WriteLine();

                    // Asking User Which Option They Want
                    Console.Write("Select Your Option: ");

                    key = Console.ReadKey();
                    ButtonClick();

                    // If Option 1, Display All Items In The Inventory
                    if (key.KeyChar == '1')
                    {
                        Console.Clear();
                        DisplayMachineItems(vendingMachine);
                    }
                    // If Option 2, Display The Purchase Menu
                    else if (key.KeyChar == '2')
                    {
                        stayInMenu = false;
                        Console.Clear();
                        DisplaySubMenu(vendingMachine, customer, mainmenu, logger);
                        break;
                    }                                                                              
                    else if (key.KeyChar == '3')
                    {
                        Console.Clear();
                        if (customer.Count > 0 || vendingMachine.Balance > 0) // if - customer has items to consume and change to be returned, perform appropriate actions 
                        {
                            stayInMenu = false;
                            CompleteTransaction(vendingMachine.Balance, vendingMachine, customer, logger);
                            break;
                        } 
                        else // else - prevent customer from even prodding this area if they don't have items to consume and change to return
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("There Is No Transaction To Complete");
                            ErrorBuzz();
                            Console.Clear();
                            mainmenu.Display(vendingMachine, customer, mainmenu, logger);

                        }
                    }
                    else if (key.KeyChar == 'q' || key.KeyChar == 'Q' && customer.Count == 0 && vendingMachine.Balance == 0)
                    {
                        stayInMenu = false;
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Thank You Come Again");
                        Console.WriteLine();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Please Select A Valid Menu Option Select Complete Transaction To Return Your Money");
                        Console.WriteLine();
                        Console.WriteLine("Select Complete Transaction To Return Your Money");
                        Delay();
                        ErrorBuzz();
                        Console.Clear();
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Please Make Another Selection");
                ErrorBuzz();
                Console.Clear();
            }
        }
        private void PrintHeader()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to Vend-O-Matic!");
            Console.WriteLine();
        }
        public void DisplaySubMenu(VendingMachine vendingMachine, List<VendingMachineItem> customer, MainMenu mainmenu, VendingMachineLogger logger)
        {
            PurchaseMenu purchaseMenu = new PurchaseMenu();
            purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
        }
        public void DisplayMachineItems(VendingMachine vendingMachine)
        {
            int enums = 0;// enumerator int used to keep track of current slot in the foreach loop
            Console.WriteLine(" ____________________________________");
            // Check to see if the items are in stock or not. If In Stock Print Price, Name and Amount Available. If Out Of Stock Print Out Of Stock
            foreach (var kvp in vendingMachine.Slots)
            {
                VendingMachineItem vmi = vendingMachine.GetItemAtSlot(kvp); // assigning the value at the current slot in the loop to a variable

                if (vmi == null)
                {
                    Console.WriteLine($"| {vendingMachine.Slots.GetValue(enums)} | SOLD OUT                      |");
                }
                else
                {
                    Console.WriteLine($"| {vendingMachine.Slots.GetValue(enums)} | {vendingMachine.GetItemAtSlot(kvp).ItemName.PadRight(18)} | {vendingMachine.GetItemAtSlot(kvp).Price} | {vendingMachine.GetQuantityRemaining(kvp)} |");
                }
                enums++;// enumerator
            }
            Console.WriteLine(" ____________________________________");
            Console.WriteLine();
            Console.WriteLine($"Current Balance: ${vendingMachine.Balance}");// money currently in the machine
            Console.WriteLine();
        }
        public void MakePurchase(string slot, string item, decimal startBal, decimal price, decimal finalBal, VendingMachine vendingMachine, List<VendingMachineItem> customer, VendingMachineLogger logger)
        {
            vendingMachine.Purchase(slot, vendingMachine, customer);
            logger.RecordTransaction($"{item} {slot.ToUpper()}", startBal, price, finalBal);
        }
        public void CompleteTransaction(decimal startBal, VendingMachine vendingMachine, List<VendingMachineItem> customer, VendingMachineLogger logger)
        {
            Console.WriteLine();
            foreach (var item in customer)// loop through purchased items and 'cosume' them
            {
                Console.WriteLine(item.Consume());
            }
            customer.Clear(); // clear out consumed items
            startBal = vendingMachine.Balance;
            Console.WriteLine();
            Console.WriteLine($"Total Change Due: {vendingMachine.Balance}");
            Console.WriteLine();
            Console.WriteLine(vendingMachine.ReturnChange().DueChange); // prints change in least amount of quarters, dimes and nickels
            logger.RecordTransaction("GIVE CHANGE ", startBal, startBal, vendingMachine.Balance);
            Console.WriteLine();
            Console.WriteLine("Thank You Come Again");
            Console.WriteLine();
            Delay();
            Console.Clear();
        }
        public void ButtonClick()
        {
            System.Media.SoundPlayer click = new System.Media.SoundPlayer(@"Sound\click_x.wav");
            click.PlaySync();
        }
        public void ErrorBuzz()
        {
            System.Media.SoundPlayer error = new System.Media.SoundPlayer(@"Sound\fail-buzzer.wav");
            error.PlaySync();
        }
        public void MenuMusic()
        {
            System.Media.SoundPlayer loop = new System.Media.SoundPlayer(@"Sound\Elevator-music.wav");
            loop.PlayLooping();
        }
        public void Delay()
        {
            System.Threading.Thread.Sleep(2000);
        }
    }
}


