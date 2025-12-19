using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.CompilerServices;
using SSD_Assignment___Banking_Application; // Add this for ApplicationConstants

namespace Banking_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int loopCount = 0;
            bool authenticated = false;
            string username = "";
            string password = "";

            Data_Access_Layer dal = Data_Access_Layer.getInstance();
            bool running = true;

            // Authorize
            do
            {
                if (loopCount > 3)
                {
                    Console.WriteLine(ApplicationConstants.TooManyInvalidAttempts);
                    running = false;
                    throw new UnauthorizedAccessException();
                }
                if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidCredentials);

                Console.WriteLine(ApplicationConstants.EnterUsername);
                username = Console.ReadLine();
                Console.WriteLine(ApplicationConstants.EnterPassword);
                password = Console.ReadLine();

                authenticated = dal.AuthenticateLogin(username, password);

                loopCount++;

            } while (authenticated == false);
            // Menu
            do
            {
                dal.loadBankAccounts(); // Reset Account on Updates
                Console.Clear();
                Console.WriteLine(ApplicationConstants.MainMenu);
                String option = Console.ReadLine();

                switch (option)
                {
                    // Add Bank Account
                    case "1":
                        // Account Type
                        String accountType = "";
                        loopCount = 0;
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidOptionChosen);

                            Console.WriteLine(ApplicationConstants.AccountTypeMenu);
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
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidNameEntered);

                            Console.WriteLine(ApplicationConstants.EnterName);
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
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidAddressLine1Entered);

                            Console.WriteLine(ApplicationConstants.EnterAddressLine1);
                            addressLine1 = Console.ReadLine();
                            loopCount++;

                        } while (addressLine1.Equals(""));

                        // Address Line 2
                        Console.WriteLine(ApplicationConstants.EnterAddressLine2);
                        String addressLine2 = Console.ReadLine();

                        // Address Line 3
                        Console.WriteLine(ApplicationConstants.EnterAddressLine3);
                        String addressLine3 = Console.ReadLine();

                        // Town
                        String town = "";
                        loopCount = 0;
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidTownEntered);

                            Console.WriteLine(ApplicationConstants.EnterTown);
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
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidOpeningBalanceEntered);

                            Console.WriteLine(ApplicationConstants.EnterOpeningBalance);
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
                                    Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                    running = false;
                                    break;
                                }
                                if (loopCount > 0)
                                    Console.WriteLine(ApplicationConstants.InvalidOverdraftAmountEntered);

                                Console.WriteLine(ApplicationConstants.EnterOverdraftAmount);
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
                                    Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                    running = false;
                                    break;
                                }
                                if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidInterestRateEntered);

                                Console.WriteLine(ApplicationConstants.EnterInterestRate);
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
                        Console.WriteLine(ApplicationConstants.NewAccountNumberIs + accNo);
                        endOfOperation();
                        break;
                    // Close Bank Account
                    case "2":
                        if (!dal.isAdmin())
                        {
                            Console.WriteLine(ApplicationConstants.AccessDeniedAdminRequired);
                            break;
                        }

                        accNo = "";
                        loopCount = 0;
                        bool validAccNo = false;

                        // Validate Format
                        do
                        {
                            if (loopCount > 3)
                            {
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidAccountFormatEntered);

                            Console.WriteLine(ApplicationConstants.EnterAccountNumber);
                            accNo = Console.ReadLine();
                            validAccNo = Guid.TryParse(accNo, out Guid guid);
                            loopCount++;
                        } while (loopCount > 0 && !validAccNo);

                        ba = dal.findBankAccountByAccNo(accNo);

                        if (ba is null)
                        {
                            Console.WriteLine(ApplicationConstants.AccountDoesNotExist);
                        }
                        else
                        {
                            Console.WriteLine(ba.ToString());
                            String ans = "";

                            do
                            {
                                Console.WriteLine(ApplicationConstants.ProceedWithDeletion);
                                ans = Console.ReadLine().ToUpper();
                                switch (ans)
                                {
                                    case "Y":
                                        dal.closeBankAccount(accNo);
                                        break;
                                    case "N":
                                        break;
                                    default:
                                        Console.WriteLine(ApplicationConstants.InvalidOptionChosen);
                                        break;
                                }
                            } while (!(ans.Equals("Y") || ans.Equals("N")));
                        }
                        endOfOperation();
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
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidAccountFormatEntered);

                            Console.WriteLine(ApplicationConstants.EnterAccountNumber);
                            accNo = Console.ReadLine();

                            validAccNo = Guid.TryParse(accNo, out Guid guid);
                            loopCount++;
                        } while (loopCount > 0 && !validAccNo);

                        ba = dal.findBankAccountByAccNo(accNo);

                        if (ba is null) Console.WriteLine(ApplicationConstants.AccountDoesNotExist);
                        else Console.WriteLine(ba.ToString());
                        endOfOperation();
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
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidAccountFormatEntered);

                            Console.WriteLine(ApplicationConstants.EnterAccountNumber);
                            accNo = Console.ReadLine();

                            validAccNo = Guid.TryParse(accNo, out Guid guid);
                            loopCount++;
                        } while (loopCount > 0 && !validAccNo);

                        string lodgementReason = ""; // If Lodge Exceeds 10,000
                        ba = dal.findBankAccountByAccNo(accNo);

                        if (ba is null)
                        {
                            Console.WriteLine(ApplicationConstants.AccountDoesNotExist);
                        }
                        else
                        {
                            double amountToLodge = -1;
                            loopCount = 0;

                            do
                            {

                                if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidAmountEntered);

                                Console.WriteLine(ApplicationConstants.EnterAmountToLodge);
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
                                Console.WriteLine(ApplicationConstants.LargeLodgementReason);
                                lodgementReason = Console.ReadLine();
                            }

                            dal.lodge(accNo, amountToLodge, lodgementReason);
                        }
                        endOfOperation();
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
                                Console.WriteLine(ApplicationConstants.TooManyInvalidAttemptsReturnMenu);
                                running = false;
                                break;
                            }
                            if (loopCount > 0) Console.WriteLine(ApplicationConstants.InvalidAccountFormatEntered);

                            Console.WriteLine(ApplicationConstants.EnterAccountNumber);
                            accNo = Console.ReadLine();

                            validAccNo = Guid.TryParse(accNo, out Guid guid);
                            loopCount++;
                        } while (loopCount > 0 && !validAccNo);

                        string withdrawlReason = ""; // If Withdraw Exceeds 10,000
                        ba = dal.findBankAccountByAccNo(accNo);

                        if (ba is null)
                        {
                            Console.WriteLine(ApplicationConstants.AccountDoesNotExist);
                        }
                        else
                        {
                            double amountToWithdraw = -1;
                            loopCount = 0;

                            do
                            {

                                if (loopCount > 0)
                                    Console.WriteLine(ApplicationConstants.InvalidAmountEntered);

                                Console.WriteLine(string.Format(ApplicationConstants.EnterAmountToWithdraw, ba.getAvailableFunds()));
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
                                Console.WriteLine(ApplicationConstants.LargeWithdrawalReason);
                                withdrawlReason = Console.ReadLine();
                            }

                            bool withdrawalOK = dal.withdraw(accNo, amountToWithdraw, withdrawlReason);

                            if (withdrawalOK == false)
                            {

                                Console.WriteLine(ApplicationConstants.InsufficientFunds);
                            }
                            endOfOperation();
                        }
                        break;
                    // Exit
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.WriteLine(ApplicationConstants.InvalidOptionChosenDefault);
                        break;
                }


            } while (running != false);

        }

        private static void endOfOperation()
        {
            Console.WriteLine(ApplicationConstants.continueMessage);
            Console.ReadKey();
        }
    }
}
