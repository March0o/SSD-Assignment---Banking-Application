using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD_Assignment___Banking_Application
{
    internal static class Prepared_Statements
    {
        // SQL Prepared Statements
        public const string add_account_command = @"
INSERT INTO Bank_Accounts (
    accountNo,
    accountNo_iv,
    name,
    name_iv,
    address_line_1,
    address_line_1_iv,
    address_line_2,
    address_line_2_iv,
    address_line_3,
    address_line_3_iv,
    town,
    town_iv,
    balance,
    accountType,
    overdraftAmount,
    interestRate
) VALUES (
    @accountNo,
    @accountNo_iv,
    @name,
    @name_iv,
    @address_line_1,
    @address_line_1_iv,
    @address_line_2,
    @address_line_2_iv,
    @address_line_3,
    @address_line_3_iv,
    @town,
    @town_iv,
    @balance,
    @accountType,
    @overdraftAmount,
    @interestRate
);";
        public const string check_table_command = @"
    SELECT 1
    FROM sqlite_master
    WHERE type = 'table'
        AND name = @tableName
    LIMIT 1;
";
        public const string get_accounts_command = "SELECT * FROM Bank_Accounts";
        public const string get_account_command = @"SELECT * FROM Bank_Accounts WHERE accountNo = @accountNo";
        public const string update_account_balance_command = @"UPDATE Bank_Accounts SET balance = @balance WHERE accountNo = @accountNo";
        public const string create_table_command = @"
    CREATE TABLE IF NOT EXISTS Bank_Accounts(    
        accountNo TEXT PRIMARY KEY,
        accountNo_iv TEXT NOT NULL,
        name TEXT NOT NULL,
        name_iv TEXT NOT NULL,
        address_line_1 TEXT,
        address_line_1_iv TEXT NOT NULL,
        address_line_2 TEXT,
        address_line_2_iv TEXT NOT NULL,
        address_line_3 TEXT,
        address_line_3_iv TEXT NOT NULL,
        town TEXT NOT NULL,
        town_iv TEXT NOT NULL,
        balance REAL NOT NULL,
        accountType INTEGER NOT NULL,
        overdraftAmount REAL,
        interestRate REAL
    ) WITHOUT ROWID
";
        public const string delete_account_command = @"DELETE FROM Bank_Accounts WHERE accountNo = @accountNo";
    }

}
