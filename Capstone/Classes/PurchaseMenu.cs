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
        public void Display(VendingMachine vendingMachine, List<VendingMachineItem> customer, MainMenu mainmenu, PurchaseMenu purchaseMenu, VendingMachineLogger logger)
        {

            // variables for keeping track of various user inputs and interaction
            string input;
            int addedMoney;
            decimal startingBalance = 0.00m;
            decimal moneyFed = 0.00m;
            decimal price = 0.00m;
            bool stillShopping = true;
            bool stayInMenu = true;
            string item;

            try
            {
                while (stayInMenu)
                {
                    MenuSound();
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
                    ConsoleKeyInfo key = Console.ReadKey();

                    if (key.KeyChar == '1')
                    {
                        ButtonSound();
                        Console.Clear();
                        Console.WriteLine();
                        Console.Write("What Bills Would You Like To Insert?(ie $1, $5, $10, $20): $");
                        addedMoney = int.Parse(Console.ReadLine()); // prompt user to insert bills of various denominations
                        Console.WriteLine();
                        ButtonSound();
                        startingBalance = vendingMachine.Balance;
                        moneyFed = addedMoney + 0.00m;

                        vendingMachine.FeedMoney(addedMoney);// add the users money in to the machine's balance 

                        logger.RecordTransaction("FEED MONEY: ", startingBalance, moneyFed, vendingMachine.Balance);

                        Console.Clear();
                        purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
                    }
                    else if (key.KeyChar == '2')
                    {
                        ButtonSound();
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
                                    Console.WriteLine($"| {vendingMachine.Slots.GetValue(enums)} | SOLD OUT                      |");
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
                            Console.Write("Which item would you like to purchase(press Q to leave)? ");
                            input = Console.ReadLine();
                            

                            if (input.ToLower() == "q")
                            {
                                ButtonSound();
                                Console.Clear();
                                Console.WriteLine("Returning To Purchase Menu");
                                stillShopping = false;
                                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
                            }
                            else
                            {
                                ButtonSound();
                                Console.Clear();
                                price = vendingMachine.GetItemAtSlot(input).Price;
                                startingBalance = vendingMachine.Balance;
                                item = vendingMachine.GetItemAtSlot(input).ItemName;
                                /*        vendingMachine.Purchase(input, vendingMachine, customer, purchaseMenu, mainmenu, logger);*/ // perform purchase
                                MakePurchase(input, vendingMachine, customer, purchaseMenu, mainmenu, logger);
                                /*logger.RecordTransaction($"{item} {input.ToUpper()}", startingBalance, price, vendingMachine.Balance); */// log the transactions
                                SalesAudit($"{item} {input.ToLower()}", startingBalance, price, Balance, logger);
                                stillShopping = false;
                                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
                            }
                        }
                    }
                    else if (key.KeyChar == '3')
                    {
                        ButtonSound();
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Returning To Main Menu");
                        stayInMenu = false;
                        break;
                    }
                    else
                    {
                        FatalErrorSound();
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Please Select A Valid Menu Option");
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                FatalErrorSound();
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Invalid Product Code");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
            }
            catch (IndexOutOfRangeException)
            {
                FatalErrorSound();
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Please Make Your Selection Again");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
            }
            catch (NullReferenceException)
            {
                FatalErrorSound();
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Please Select Another Product");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
            }
            catch (FormatException)
            {
                FatalErrorSound();
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Please Enter Whole Dollar Amounts(ie $1, $5, $10, $20)");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
            }
            catch (OverflowException)
            {
                FatalErrorSound();
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Machine Can't Handle That Much Money");
                purchaseMenu.Display(vendingMachine, customer, mainmenu, purchaseMenu, logger);
            }
        }
    }
}



