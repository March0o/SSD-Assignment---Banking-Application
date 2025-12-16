using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Banking_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            Data_Access_Layer dal = Data_Access_Layer.getInstance();
            bool running = true;

            do
            {
                dal.loadBankAccounts(); // Reset Account on Updates

                Console.WriteLine("");
                Console.WriteLine("***Banking Application Menu***");
                Console.WriteLine("1. Add Bank Account");
                Console.WriteLine("2. Close Bank Account");
                Console.WriteLine("3. View Account Information");
                Console.WriteLine("4. Make Lodgement");
                Console.WriteLine("5. Make Withdrawal");
                Console.WriteLine("6. Exit");
                Console.WriteLine("CHOOSE OPTION:");
                String option = Console.ReadLine();
                
                switch(option)
                {
                    // Add Bank Account
                    case "1":
                        // Account Type
                        String accountType = "";
                        int loopCount = 0;
                        do
                        {
                            if(loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0)  Console.WriteLine("INVALID OPTION CHOSEN - PLEASE TRY AGAIN");

                            Console.WriteLine("");
                            Console.WriteLine("***Account Types***:");
                            Console.WriteLine("1. Current Account.");
                            Console.WriteLine("2. Savings Account.");
                            Console.WriteLine("CHOOSE OPTION:");
                            accountType = Console.ReadLine();
                            loopCount++;

                        } while (!(accountType.Equals("1") || accountType.Equals("2")));
                        
                        // Account Name
                        String name = "";
                        loopCount = 0;
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0)  Console.WriteLine("INVALID NAME ENTERED - PLEASE TRY AGAIN");

                            Console.WriteLine("Enter Name: ");
                            name = Console.ReadLine();
                            loopCount++;

                        } while (name.Equals(""));
                        
                        // Address Line 1
                        String addressLine1 = "";
                        loopCount = 0;
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine("INVALID ÀDDRESS LINE 1 ENTERED - PLEASE TRY AGAIN");

                            Console.WriteLine("Enter Address Line 1: ");
                            addressLine1 = Console.ReadLine();
                            loopCount++;

                        } while (addressLine1.Equals(""));
                        
                        // Address Line 2
                        Console.WriteLine("Enter Address Line 2: ");
                        String addressLine2 = Console.ReadLine();
                        
                        // Address Line 3
                        Console.WriteLine("Enter Address Line 3: ");
                        String addressLine3 = Console.ReadLine();
                        
                        // Town
                        String town = "";
                        loopCount = 0;
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0)  Console.WriteLine("INVALID TOWN ENTERED - PLEASE TRY AGAIN");

                            Console.WriteLine("Enter Town: ");
                            town = Console.ReadLine();
                            loopCount++;

                        } while (town.Equals(""));
                        
                        // Balance
                        double balance = -1;
                        loopCount = 0;
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0)  Console.WriteLine("INVALID OPENING BALANCE ENTERED - PLEASE TRY AGAIN");

                            Console.WriteLine("Enter Opening Balance: ");
                            String balanceString = Console.ReadLine();
                            try
                            {
                                balance = Convert.ToDouble(balanceString);
                            }
                            catch 
                            {
                                loopCount++;
                            }

                        } while (balance < 0);

                        // Create Account
                        Bank_Account ba; // Placeholder for Bank Account

                        // Current Account Case
                        if (Convert.ToInt32(accountType) == Account_Type.Current_Account)
                        {
                            double overdraftAmount = -1;
                            loopCount = 0;
                            do
                            {
                                if (loopCount > 3)
                                {
                                    Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                    break;
                                }
                                if (loopCount > 0)
                                    Console.WriteLine("INVALID OVERDRAFT AMOUNT ENTERED - PLEASE TRY AGAIN");

                                Console.WriteLine("Enter Overdraft Amount: ");
                                String overdraftAmountString = Console.ReadLine();
                                try
                                {
                                    overdraftAmount = Convert.ToDouble(overdraftAmountString);
                                }

                                catch
                                {
                                    loopCount++;
                                }

                            } while (overdraftAmount < 0);

                            ba = new Current_Account(name, addressLine1, addressLine2, addressLine3, town, balance, overdraftAmount); // Create Account Object
                        }

                        // Savings Account Case
                        else
                        {

                            double interestRate = -1;
                            loopCount = 0;

                            do
                            {
                                if (loopCount > 3)
                                {
                                    Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                    break;
                                }
                                if (loopCount > 0)  Console.WriteLine("INVALID INTEREST RATE ENTERED - PLEASE TRY AGAIN");

                                Console.WriteLine("Enter Interest Rate: ");
                                String interestRateString = Console.ReadLine();

                                try
                                {
                                    interestRate = Convert.ToDouble(interestRateString);
                                }

                                catch
                                {
                                    loopCount++;
                                }

                            } while (interestRate < 0);

                            ba = new Savings_Account(name, addressLine1, addressLine2, addressLine3, town, balance, interestRate);
                        }
                        String accNo = dal.addBankAccount(ba);
                        Console.WriteLine("New Account Number Is: " + accNo);

                        break;
                    // Close Bank Account
                    case "2":
                        accNo = "";
                        loopCount = 0;
                        bool validAccNo = false;

                        // Validate Format
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine("INVALID ACCOUNT FORMAT ENTERED- PLEASE TRY AGAIN");

                            Console.WriteLine("Enter Account Number: ");
                            accNo = Console.ReadLine();
                            validAccNo = Guid.TryParse(accNo, out Guid guid);
                            loopCount++;
                        } while (loopCount > 0 && !validAccNo);

                        ba = dal.findBankAccountByAccNo(accNo);

                        if (ba is null)
                        {
                            Console.WriteLine("Account Does Not Exist");
                        }
                        else
                        {
                            Console.WriteLine(ba.ToString());
                            String ans = "";

                            do
                            {
                                Console.WriteLine("Proceed With Delection (Y/N)?"); 
                                ans = Console.ReadLine().ToUpper();
                                switch (ans)
                                {
                                    case "Y": dal.closeBankAccount(accNo);
                                        break;
                                    case "N":
                                        break;
                                    default:
                                        Console.WriteLine("INVALID OPTION CHOSEN - PLEASE TRY AGAIN");
                                        break;
                                }
                            } while (!(ans.Equals("Y") || ans.Equals("y") || ans.Equals("N") || ans.Equals("n")));
                        }

                        break;
                    // View Account Information
                    case "3":
                        accNo = "";
                        loopCount = 0;
                        validAccNo = false;

                        // Validate Format
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine("INVALID ACCOUNT FORMAT ENTERED - PLEASE TRY AGAIN");

                            Console.WriteLine("Enter Account Number: ");
                            accNo = Console.ReadLine();

                            validAccNo = Guid.TryParse(accNo, out Guid guid);
                            loopCount++;
                        } while (loopCount > 0 && !validAccNo);

                        ba = dal.findBankAccountByAccNo(accNo);

                        if (ba is null) Console.WriteLine("Account Does Not Exist");
                        else Console.WriteLine(ba.ToString());
 
                        break;
                    // Make Lodgement
                    case "4": //Lodge
                        accNo = "";
                        loopCount = 0;
                        validAccNo = false;

                        // Validate Format
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine("INVALID ACCOUNT FORMAT ENTERED - PLEASE TRY AGAIN");

                            Console.WriteLine("Enter Account Number: ");
                            accNo = Console.ReadLine();

                            validAccNo = Guid.TryParse(accNo, out Guid guid);
                            loopCount++;
                        } while (loopCount > 0 && !validAccNo);

                        string lodgementReason = ""; // If Lodge Exceeds 10,000
                        ba = dal.findBankAccountByAccNo(accNo);

                        if (ba is null)
                        {
                            Console.WriteLine("Account Does Not Exist");
                        }
                        else
                        {
                            double amountToLodge = -1;
                            loopCount = 0;

                            do
                            {

                                if (loopCount > 0)  Console.WriteLine("INVALID AMOUNT ENTERED - PLEASE TRY AGAIN");

                                Console.WriteLine("Enter Amount To Lodge: ");
                                String amountToLodgeString = Console.ReadLine();

                                try
                                {
                                    amountToLodge = Convert.ToDouble(amountToLodgeString);
                                }

                                catch
                                {
                                    loopCount++;
                                }

                            } while (amountToLodge < 0);

                            if (amountToLodge > 10000)
                            {
                                Console.WriteLine("Large lodgements require reason, Please enter why this lodgement is being made: ");
                                lodgementReason = Console.ReadLine();
                            }

                            dal.lodge(accNo, amountToLodge, lodgementReason);
                        }
                        break;
                    // Make Withdrawal
                    case "5": //Withdraw
                        accNo = "";
                        loopCount = 0;
                        validAccNo = false;

                        // Validate Format
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine("TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU");
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine("INVALID ACCOUNT FORMAT ENTERED - PLEASE TRY AGAIN");

                            Console.WriteLine("Enter Account Number: ");
                            accNo = Console.ReadLine();

                            validAccNo = Guid.TryParse(accNo, out Guid guid);
                            loopCount++;
                        } while (loopCount > 0 && !validAccNo);

                        string withdrawlReason = ""; // If Withdraw Exceeds 10,000
                        ba = dal.findBankAccountByAccNo(accNo);

                        if (ba is null)
                        {
                            Console.WriteLine("Account Does Not Exist");
                        }
                        else
                        {
                            double amountToWithdraw = -1;
                            loopCount = 0;

                            do
                            {

                                if (loopCount > 0)
                                    Console.WriteLine("INVALID AMOUNT ENTERED - PLEASE TRY AGAIN");

                                Console.WriteLine("Enter Amount To Withdraw (€" + ba.getAvailableFunds() + " Available): ");
                                String amountToWithdrawString = Console.ReadLine();

                                try
                                {
                                    amountToWithdraw = Convert.ToDouble(amountToWithdrawString);
                                }

                                catch
                                {
                                    loopCount++;
                                }

                            } while (amountToWithdraw < 0);

                            if (amountToWithdraw > 10000)
                            {
                                Console.WriteLine("Large withdrawls require reason, Please enter why this withdrawl is being made: ");
                                withdrawlReason = Console.ReadLine();
                            }

                            bool withdrawalOK = dal.withdraw(accNo, amountToWithdraw, withdrawlReason);

                            if(withdrawalOK == false)
                            {

                                Console.WriteLine("Insufficient Funds Available.");
                            }
                        }
                        break;
                    // Exit
                    case "6":
                        running = false;
                        break;
                    default:    
                        Console.WriteLine("INVALID OPTION CHOSEN - PLEASE TRY AGAIN");
                        break;
                }
                
                
            } while (running != false);

        }

    }
}