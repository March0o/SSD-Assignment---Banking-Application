using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SSD_Assignment___Banking_Application;
using static System.Net.Mime.MediaTypeNames;

namespace Banking_Application
{
    public class Data_Access_Layer
    {
        private readonly AES_DLE encryption = new AES_DLE();
        private List<Bank_Account> accounts;
        public static String databaseName = "Banking Database.db";
        private static Data_Access_Layer instance = new Data_Access_Layer();
        private readonly Logger logger;


        private Data_Access_Layer() //Singleton Design Pattern (For Concurrency Control) - Use getInstance() Method Instead.
        {
            accounts = new List<Bank_Account>();
            logger = new Logger();
        }
        public static Data_Access_Layer getInstance()
        {
            // Reduce Reflection Check Calling Method
            if (Verify_Calling_Method("SSD Assignment - Banking Application.dll", "Program", "Main") != true)
            {
                throw new MethodAccessException();
            }
            return instance;
        }

        private SqliteConnection getDatabaseConnection()
        {

            String databaseConnectionString = new SqliteConnectionStringBuilder()
            {
                DataSource = Data_Access_Layer.databaseName,
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();

            return new SqliteConnection(databaseConnectionString);

        }

        private void initialiseDatabase()
        {
            using (var connection = getDatabaseConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
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

                command.ExecuteNonQuery();
                
            }
        }

        public static bool TableExists(string tableName)
        {
            using var conn = new SqliteConnection($"Data Source={Data_Access_Layer.databaseName}");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 1
                FROM sqlite_master
                WHERE type = 'table'
                  AND name = @tableName
                LIMIT 1;
            ";

            cmd.Parameters.AddWithValue("@tableName", tableName);

            return cmd.ExecuteScalar() != null;
        }

        public void loadBankAccounts()
        {
            if (!File.Exists(Data_Access_Layer.databaseName) || !TableExists("Bank_Accounts"))
                initialiseDatabase();
            else
            {
                accounts = [];
                using (var connection = getDatabaseConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Bank_Accounts";
                    SqliteDataReader dr = command.ExecuteReader();
                    
                    while(dr.Read())
                    {

                        int accountType = dr.GetInt16(13);

                        if(accountType == Account_Type.Current_Account)
                        {
                            Current_Account ca = new Current_Account();
                            ca.accountNo = dr.GetString(0);
                            ca.accountNo_iv = dr.GetString(1);
                            ca.name = dr.GetString(2);
                            ca.name_iv = dr.GetString(3);
                            ca.address_line_1 = dr.GetString(4);
                            ca.address_line_1_iv = dr.GetString(5);
                            ca.address_line_2 = dr.GetString(6);
                            ca.address_line_2_iv = dr.GetString(7);
                            ca.address_line_3 = dr.GetString(8);
                            ca.address_line_3_iv = dr.GetString(9);
                            ca.town = dr.GetString(10);
                            ca.town_iv = dr.GetString(11);
                            ca.balance = dr.GetDouble(12);
                            ca.overdraftAmount = dr.GetDouble(14);
                            accounts.Add(ca);
                        }
                        else
                        {
                            Savings_Account sa = new Savings_Account();
                            sa.accountNo = dr.GetString(0);
                            sa.accountNo_iv = dr.GetString(1);
                            sa.name = dr.GetString(2);
                            sa.name_iv = dr.GetString(3);
                            sa.address_line_1 = dr.GetString(4);
                            sa.address_line_1_iv = dr.GetString(5);
                            sa.address_line_2 = dr.GetString(6);
                            sa.address_line_2_iv = dr.GetString(7);
                            sa.address_line_3 = dr.GetString(8);
                            sa.address_line_3_iv = dr.GetString(9);
                            sa.town = dr.GetString(10);
                            sa.town_iv = dr.GetString(11);
                            sa.balance = dr.GetDouble(12);
                            sa.interestRate = dr.GetDouble(15);
                            accounts.Add(sa);
                        }


                    }

                }

            }
        }

        /*
         Add Bank Account
            -Encryption on adding account
         */
        public String addBankAccount(Bank_Account ba) 
        {
            // Reduce Reflection Check Calling Method
            if (Verify_Calling_Method("SSD Assignment - Banking Application.dll", "Program", "Main") != true)
            {
                throw new MethodAccessException();
            }

            if (ba.GetType() == typeof(Current_Account))
                ba = (Current_Account)ba;
            else
                ba = (Savings_Account)ba;

            accounts.Add(ba);

            using (var connection = getDatabaseConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                var acc_no_encrypt = EncryptField(ba.accountNo);
                var name_encrypt = EncryptField(ba.name);
                var addr1_encrypt = EncryptField(ba.address_line_1);
                var addr2_encrypt = EncryptField(ba.address_line_2);
                var addr3_encrypt = EncryptField(ba.address_line_3);
                var town_encrypt = EncryptField(ba.town);
                command.CommandText =
                @"
                    INSERT INTO Bank_Accounts VALUES(" +
                    "'" + acc_no_encrypt.Item1 + "', " +
                    "'" + acc_no_encrypt.Item2 + "', " +
                    "'" + name_encrypt.Item1 + "', " +
                    "'" + name_encrypt.Item2 + "', " +
                    "'" + addr1_encrypt.Item1 + "', " +
                    "'" + addr1_encrypt.Item2 + "', " +
                    "'" + addr2_encrypt.Item1 + "', " +
                    "'" + addr2_encrypt.Item2 + "', " +
                    "'" + addr3_encrypt.Item1 + "', " +
                    "'" + addr3_encrypt.Item2 + "', " +
                    "'" + town_encrypt.Item1 + "', " +
                    "'" + town_encrypt.Item2 + "', " +
                    ba.balance + ", " +
                    (ba.GetType() == typeof(Current_Account) ? 1 : 2) + ", ";

                if (ba.GetType() == typeof(Current_Account))
                {
                    Current_Account ca = (Current_Account)ba;
                    command.CommandText += ca.overdraftAmount + ", NULL)";
                }

                else
                {
                    Savings_Account sa = (Savings_Account)ba;
                    command.CommandText += "NULL," + sa.interestRate + ")";
                }

                command.ExecuteNonQuery();
                logger.Log("N/A", ba.accountNo, ba.name, "1", "");
            }
            GC.Collect(); // Clear Memory
            return ba.accountNo;
        }


        /*
         Find Bank Account by Account Number
            -Decryption on retrieving account
         */
        public Bank_Account findBankAccountByAccNo(String accNo) 
        {
            // Reduce Reflection Check Calling Method
            if (Verify_Calling_Method("SSD Assignment - Banking Application.dll", "Program", "Main") != true)
            {
                throw new MethodAccessException();
            }

            foreach (Bank_Account ba in accounts)
            {
                string iv = ba.accountNo_iv;
                string decryptedAccNo = DecryptField(ba.accountNo, iv);
                if (decryptedAccNo.Equals(accNo))
                {
                    if (ba.GetType() == typeof(Current_Account))
                    {
                        Current_Account decryptedAcc = new Current_Account
                        {
                            accountNo = accNo,
                            name = DecryptField(ba.name, ba.name_iv),
                            address_line_1 = DecryptField(ba.address_line_1, ba.address_line_1_iv),
                            address_line_2 = DecryptField(ba.address_line_2, ba.address_line_2_iv),
                            address_line_3 = DecryptField(ba.address_line_3, ba.address_line_3_iv),
                            town = DecryptField(ba.town, ba.town_iv),
                            balance = ba.balance,
                            overdraftAmount = ((Current_Account)ba).overdraftAmount
                        };
                        logger.Log("N/A", decryptedAcc.accountNo, decryptedAcc.name, "3", "");
                        return decryptedAcc;
                    }
                    else
                    {
                        Savings_Account decryptedAcc = new Savings_Account
                        {
                            accountNo = accNo,
                            name = DecryptField(ba.name, ba.name_iv),
                            address_line_1 = DecryptField(ba.address_line_1, ba.address_line_1_iv),
                            address_line_2 = DecryptField(ba.address_line_2, ba.address_line_2_iv),
                            address_line_3 = DecryptField(ba.address_line_3, ba.address_line_3_iv),
                            town = DecryptField(ba.town, ba.town_iv),
                            balance = ba.balance,
                            interestRate = ((Savings_Account)ba).interestRate
                        };
                        logger.Log("N/A", decryptedAcc.accountNo, decryptedAcc.name, "3", "");
                        return decryptedAcc;
                    }
                }

            }
            GC.Collect(); // Clear Memory
            return null; 
        }

        /*
         Close Bank Account
            - Added Decryption on retrieval
         */
        public bool closeBankAccount(String accNo) 
        {
            // Reduce Reflection Check Calling Method
            if (Verify_Calling_Method("SSD Assignment - Banking Application.dll", "Program", "Main") != true)
            {
                throw new MethodAccessException();
            }

            string decryptedAccNo = "";
            string decryptedName = "";
            Bank_Account toRemove = null;
            foreach (Bank_Account ba in accounts)
            {
                string iv = ba.accountNo_iv;
                decryptedAccNo = DecryptField(ba.accountNo, iv);
                decryptedName = DecryptField(ba.name, ba.name_iv);
                if (decryptedAccNo.Equals(accNo))
                {
                    toRemove = ba;
                    break;
                }
            }

            if (toRemove == null)
            {
                GC.Collect(); // Clear Memory
                return false;
            }
            else
            {
                accounts.Remove(toRemove);
                using (var connection = getDatabaseConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Bank_Accounts WHERE accountNo = '" + toRemove.accountNo + "'";
                    command.ExecuteNonQuery();

                }
                logger.Log("N/A", decryptedAccNo, decryptedName, "2", "");
                GC.Collect(); // Clear Memory
                return true;
            }

        }


        /*
         Lodge
            - Added Decryption on retrieval
         */
        public bool lodge(String accNo, double amountToLodge, string reason)
        {
            // Reduce Reflection Check Calling Method
            if (Verify_Calling_Method("SSD Assignment - Banking Application.dll", "Program", "Main") != true)
            {
                throw new MethodAccessException();
            }

            string decryptedAccNo = "";
            string decryptedName = "";
            Bank_Account toLodgeTo = null;
            foreach (Bank_Account ba in accounts)
            {
                string iv = ba.accountNo_iv;
                decryptedAccNo = DecryptField(ba.accountNo, iv);
                decryptedName = DecryptField(ba.name, ba.name_iv);
                if (decryptedAccNo.Equals(accNo))
                {
                    ba.lodge(amountToLodge);
                    toLodgeTo = ba;
                    break;
                }

            }

            if (toLodgeTo == null)
            {
                GC.Collect(); // Clear Memory
                return false;
            }
            else
            {

                using (var connection = getDatabaseConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE Bank_Accounts SET balance = " + toLodgeTo.balance + " WHERE accountNo = '" + toLodgeTo.accountNo + "'";
                    command.ExecuteNonQuery();

                }
                logger.Log("N/A", decryptedAccNo, decryptedName, "4", reason);
                GC.Collect(); // Clear Memory
                return true;
            }

        }


        /*
         Withdraw
            - Added Decryption on retrieval
         */
        public bool withdraw(String accNo, double amountToWithdraw, string reason)
        {
            // Reduce Reflection Check Calling Method
            if (Verify_Calling_Method("SSD Assignment - Banking Application.dll", "Program", "Main") != true)
            {
                throw new MethodAccessException();
            }

            string decryptedAccNo = "";
            string decryptedName = "";
            Bank_Account toWithdrawFrom = null;
            bool result = false;

            foreach (Bank_Account ba in accounts)
            {
                string iv = ba.accountNo_iv;
                decryptedAccNo = DecryptField(ba.accountNo, iv);
                decryptedName = DecryptField(ba.name, ba.name_iv);
                if (decryptedAccNo.Equals(accNo))
                {
                    result = ba.withdraw(amountToWithdraw);
                    toWithdrawFrom = ba;
                    break;
                }

            }

            if (toWithdrawFrom == null || result == false)
                return false;
            else
            {

                using (var connection = getDatabaseConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE Bank_Accounts SET balance = " + toWithdrawFrom.balance + " WHERE accountNo = '" + toWithdrawFrom.accountNo + "'";
                    command.ExecuteNonQuery();

                }
                logger.Log("N/A", decryptedAccNo, decryptedName, "5", reason);
                GC.Collect(); // Clear Memory
                return true;
            }

        }

        /*
         Encrypt String Property
         */
        private (string, string) EncryptField(string inputField)
        {
            byte[] field_data = Encoding.ASCII.GetBytes(inputField);
            (byte[] encrypted_data, byte[] iv) = encryption.Encrypt(field_data);
            return (Convert.ToBase64String(encrypted_data), Convert.ToBase64String(iv));
        }

        /*
         Decrypt String Property
         */
        private string DecryptField(string inputField, string iv)
        {
            byte[] bytes = Convert.FromBase64String(inputField);
            byte[] iv_bytes = Convert.FromBase64String(iv);
            byte[] decrypted_data = encryption.Decrypt(bytes, iv_bytes);
            return Encoding.UTF8.GetString(decrypted_data);
        }

        static bool Verify_Calling_Method(string desiredAssesmblyName, string desiredClassName, string desiredMethodName, string desiredFileName = null, int desiredLineNo = 0)
        {

            StackFrame stackFrame = new StackFrame(2, true);
            MethodBase actualCallingMethod = stackFrame.GetMethod();
            string actualCallingMethodName = actualCallingMethod.Name;
            string actualCallingClass = actualCallingMethod.ReflectedType.Name;
            string actualCallingAssembly = actualCallingMethod.Module.Name;

            //Check Calling Assembly

            bool assemblyMatch = false;

            if (desiredAssesmblyName != null)
                assemblyMatch = desiredAssesmblyName.Equals(actualCallingAssembly);
            else//NULL => No Check Required.
                assemblyMatch = true;

            //Check Calling Class

            bool classMatch = false;

            if (desiredClassName != null)
                classMatch = desiredClassName.Equals(actualCallingClass);
            else//NULL => No Check Required.
                classMatch = true;

            //Check Calling Method

            bool methodMatch = false;

            if (desiredMethodName != null)
                methodMatch = desiredMethodName.Equals(actualCallingMethodName);
            else//NULL => No Check Required.
                methodMatch = true;

            //Other Checks (Debug Builds Only)

            bool fileNameMatch = true;//Assume True Incase A Release Build Is Used
            bool lineNoMatch = true;

            if (stackFrame.HasSource() && desiredFileName != null)//If HasSource() == true => Debug Build => File Names And Line Numbers Can Also Be Used
            {
                string actualFileName = stackFrame.GetFileName().ToString();//If stackFrame.HasSource() == false => Result of GetFileName() Is null.
                string actualLineNo = stackFrame.GetFileLineNumber().ToString();//If stackFrame.HasSource() == false => Result of GetLineNumber() Is null.
                fileNameMatch = desiredFileName.Equals(actualFileName);
                lineNoMatch = desiredLineNo.Equals(actualLineNo);

            }

            return (assemblyMatch && classMatch && methodMatch && fileNameMatch && lineNoMatch);//If All TRUE, Result Is TRUE.


        }

    }
}
