using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD_Assignment___Banking_Application
{
    internal class ApplicationConstants
    {
        public const string unavailableField = "N/A"; // Used In Logger when a field is unavailable
        public const string table_name = "Bank_Accounts"; // Database Table Name
        public const string key_name = "encryption_key"; // Name Of The Key In Windows DPAPI Key Store
        public const string default_account_field = "TO-BE-SET"; // Default IV Value For Account Fields
        public const string database_file = "Banking Database.db"; // Database File Name
        public const string application_dll = "SSD Assignment - Banking Application.dll"; // Application DLL Name
        public const string domainName = "ITSLIGO.LAN";  // Domain Name For Authentication
        public const string tellerRoleName = "Bank Teller"; // Role Name For Bank Tellers
        public const string adminTellerRoleName = "Bank Teller Administrator"; // Role Name For Bank Teller Administrators

        // Messages
        public const string invalidCredentialsMessage = "User Is Not Authorized To Perform This Action - Invalid User Credentials Provided."; // Invalid Credentials Message
        public const string invalidUserGroup = "User Is Not Authorized To Perform This Action - User Is Not A Member Of The Authorized User Group."; // Invalid User Group Message
        public const string authorizedUser = "User Is Authorized To Perform Access Control Protected Action"; // Authorized User Message
        public const string successfulAuthentication = "Successful Authentication"; // Successful Authentication Message
        public const string failedAuthentication = "Failed Authentication"; // Failed Authentication Message

        // Menu Prompts
        public const string MainMenu = "\n***Banking Application Menu***\n1. Add Bank Account\n2. Close Bank Account (ADMIN ONLY)\n3. View Account Information\n4. Make Lodgement\n5.Make Withdrawal\n6. Exit\nCHOOSE OPTION:";
        public const string AccountTypeMenu = "\n***Account Types***:\n1. Current Account.\n2. Savings Account.\nCHOOSE OPTION:";

        // Authentication
        public const string TooManyInvalidAttempts = "TOO MANY INVALID ATTEMPTS";
        public const string InvalidCredentials = "INVALID CREDENTIALS - PLEASE TRY AGAIN";
        public const string EnterUsername = "Enter Username:";
        public const string EnterPassword = "Enter Password:";

        // Add Account
        public const string TooManyInvalidAttemptsReturnMenu = "TOO MANY INVALID ATTEMPTS - RETURNING TO MAIN MENU";
        public const string InvalidOptionChosen = "INVALID OPTION CHOSEN - PLEASE TRY AGAIN";
        public const string InvalidNameEntered = "INVALID NAME ENTERED - PLEASE TRY AGAIN";
        public const string EnterName = "Enter Name: ";
        public const string InvalidAddressLine1Entered = "INVALID ÀDDRESS LINE 1 ENTERED - PLEASE TRY AGAIN";
        public const string EnterAddressLine1 = "Enter Address Line 1: ";
        public const string EnterAddressLine2 = "Enter Address Line 2: ";
        public const string EnterAddressLine3 = "Enter Address Line 3: ";
        public const string InvalidTownEntered = "INVALID TOWN ENTERED - PLEASE TRY AGAIN";
        public const string EnterTown = "Enter Town: ";
        public const string InvalidOpeningBalanceEntered = "INVALID OPENING BALANCE ENTERED - PLEASE TRY AGAIN";
        public const string EnterOpeningBalance = "Enter Opening Balance: ";
        public const string InvalidOverdraftAmountEntered = "INVALID OVERDRAFT AMOUNT ENTERED - PLEASE TRY AGAIN";
        public const string EnterOverdraftAmount = "Enter Overdraft Amount: ";
        public const string InvalidInterestRateEntered = "INVALID INTEREST RATE ENTERED - PLEASE TRY AGAIN";
        public const string EnterInterestRate = "Enter Interest Rate: ";
        public const string NewAccountNumberIs = "New Account Number Is: ";

        // Close Account
        public const string AccessDeniedAdminRequired = "ACCESS DENIED - ADMINISTRATOR PRIVILEGES REQUIRED";
        public const string InvalidAccountFormatEntered = "INVALID ACCOUNT FORMAT ENTERED - PLEASE TRY AGAIN";
        public const string AccountDoesNotExist = "Account Does Not Exist";
        public const string ProceedWithDeletion = "Proceed With Delection (Y/N)?";

        // Lodgement
        public const string InvalidAmountEntered = "INVALID AMOUNT ENTERED - PLEASE TRY AGAIN";
        public const string EnterAmountToLodge = "Enter Amount To Lodge: ";
        public const string LargeLodgementReason = "Large lodgements require reason, Please enter why this lodgement is being made: ";

        // Withdrawal
        public const string EnterAmountToWithdraw = "Enter Amount To Withdraw (€{0} Available): ";
        public const string LargeWithdrawalReason = "Large withdrawls require reason, Please enter why this withdrawl is being made: ";
        public const string InsufficientFunds = "Insufficient Funds Available.";

        // Default
        public const string InvalidOptionChosenDefault = "INVALID OPTION CHOSEN - PLEASE TRY AGAIN";
        public const string EnterAccountNumber = "Enter Account Number: "; 


    }
}
