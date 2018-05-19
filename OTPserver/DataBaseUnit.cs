using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace OTPserver
{
    public class DataBaseUnit
    {
        public class record
        {
            public sbyte no;
            List<object> args;
            public record(sbyte funcNo,List<object> args)
            {
                no = funcNo;
                this.args = args;
            }
        }
        string dbHost = "localhost";//資料庫位址
        string dbUser = "root";//資料庫使用者帳號
        string dbPass = "";//資料庫使用者密碼
        string dbName = "test";//資料庫名稱
        string connStr;
        public DataBaseUnit()
        {
             connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + ";SslMode = none";

            //connection = new MySqlConnection(connStr);
            //connection.Open();
            //Console.Write("has open");

        }
        public DataBaseUnit(string dbHost,string dbUser,string dbPass,string dbName)
        {
            if (dbHost != null)
            {
                this.dbHost = dbHost;
            }
            if (dbUser != null)
            {
                this.dbUser = dbUser;
            }
            if (dbPass != null)
            {
                this.dbPass = dbPass;
            }
            if (dbName != null)
            {
                this.dbName = dbName;
            }
             connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + ";SslMode = none";
            //MySqlConnection connection;
            //connection = new MySqlConnection(connStr);
            //connection.Open();
            //cmd = connection.CreateCommand();
        }
        public int getKey(string name)
        {
            //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `default` WHERE `account` LIKE '"+name+"'");
            MySqlConnection connection=new MySqlConnection(connStr);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM `keys` WHERE `account` LIKE '" + name + "'";
            MySqlDataReader reader= cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                if (reader.FieldCount == 0)
                {
                    reader.Close();
                    return -1;
                }
                else
                {
                    
                    int ans = reader.GetInt32(1);
                    reader.Close();
                    Console.WriteLine("getkey中 read結束");
                    return ans;
                }
            }
            return -1;
        }
        public int getSecondFromOri(string account)
        {
            //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `default` WHERE `account` LIKE '"+name+"'");
            MySqlConnection connection = new MySqlConnection(connStr);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM `timeBefore` WHERE `account` LIKE '" + account + "'";
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.FieldCount == 0)
                {
                    return -1;
                }
                else
                {
                    int ans = reader.GetInt32(1);
                    reader.Close();
                    return ans;
                }
            }
            return -1;
        }
        public int getSecPassed(string account)
        {
            Console.WriteLine("查詢中:開始");
            MySqlConnection connection = new MySqlConnection(connStr);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText= "SELECT `timeBefore`.`secondFromOri` FROM `timeBefore`,`keys` WHERE `default`.`account`=`timeBefore`.`account` AND `default`.`account`= '"+ account+"'";
            Console.WriteLine("查詢中:已產生命令列");
            MySqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("查詢中:獲得reader");
            while (reader.Read())
            {
                if (reader.FieldCount == 0)
                {
                    return -1;
                }
                else
                {
                    Console.WriteLine("進入有結果");
                    int ans = reader.GetInt32(0);
                    reader.Close();
                    return ans;
                }
            }
            return -1;
        }
        public void addData(string account,int key,int second)
        {
            MySqlConnection connection = new MySqlConnection(connStr);
            connection.Open();
            addData(account, key);
            MySqlCommand cmd2 = connection.CreateCommand();
            cmd2.CommandText = "INSERT INTO `timeBefore` (`account`, `secondFromOri`) VALUES ('" + account + "', '" + second + "');";
            cmd2.ExecuteNonQuery();

        }
        public void addData(string account, int key)
        {
            MySqlConnection connection = new MySqlConnection(connStr);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO `keys` (`account`, `key`) VALUES ('" + account + "', '" + key + "');";
            Console.WriteLine("開始 addData中的insert指令");
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void changeCounter(string account,int num)
        {
            MySqlConnection connection = new MySqlConnection(connStr);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE `timeBefore` SET `secondFromOri` = '" + num+ "' WHERE `timeBefore`.`account` = '" + account+"'";
            Console.WriteLine("開始 addData中的update指令");
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}