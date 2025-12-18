using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking_Application
{
    public abstract class Bank_Account
    {

        public String accountNo;
        public String accountNo_iv; // IV value for account number encryption
        public String name;
        public String name_iv; // IV value for name encryption
        public String address_line_1;
        public String address_line_1_iv; // IV value for address line 1 encryption
        public String address_line_2;
        public String address_line_2_iv; // IV value for address line 2 encryption
        public String address_line_3;
        public String address_line_3_iv; // IV value for address line 3 encryption
        public String town;
        public String town_iv; // IV value for town encryption
        public double balance;

        public Bank_Account()
        {

        }
        
        public Bank_Account(
            String name, 
            String address_line_1, 
            String address_line_2, 
            String address_line_3,
            String town, double balance)
        {
            this.accountNo = System.Guid.NewGuid().ToString();
            this.accountNo_iv = "TO-BE-SET";
            this.name = name;
            this.name_iv = "TO-BE-SET";
            this.address_line_1 = address_line_1;
            this.address_line_1_iv = "TO-BE-SET";
            this.address_line_2 = address_line_2;
            this.address_line_2_iv = "TO-BE-SET";
            this.address_line_3 = address_line_3;
            this.address_line_3_iv = "TO-BE-SET";
            this.town = town;
            this.balance = balance;
        }

        public void lodge(double amountIn)
        {

            balance += amountIn;

        }

        public abstract bool withdraw(double amountToWithdraw);

        public abstract double getAvailableFunds();

        public override String ToString()
        {

            return "\nAccount No: " + accountNo + "\n" +
            "Name: " + name + "\n" +
            "Address Line 1: " + address_line_1 + "\n" +
            "Address Line 2: " + address_line_2 + "\n" +
            "Address Line 3: " + address_line_3 + "\n" +
            "Town: " + town + "\n" +
            "Balance: " + balance + "\n";
        }

    }
}
