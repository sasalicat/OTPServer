using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace OTPserver
{
    public class DataBaseUnit
    {
        string dbHost = "localhost";//資料庫位址
        string dbUser = "root";//資料庫使用者帳號
        string dbPass = "";//資料庫使用者密碼
        string dbName = "test";//資料庫名稱
        private MySqlConnection connection;
        public DataBaseUnit()
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + ";SslMode = none";
            connection = new MySqlConnection(connStr);
            connection.Open();
            Console.Write("has open");
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
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + ";SslMode = none";
            connection = new MySqlConnection(connStr);
            connection.Open();
        }
        public int getKey(string name)
        {
            //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `default` WHERE `account` LIKE '"+name+"'");
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM `default` WHERE `account` LIKE '" + name + "'";
            MySqlDataReader reader= cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.FieldCount == 0)
                {
                    return -1;
                }
                else
                {
                    return reader.GetInt32(1);
                }
            }
            return -1;
        }
        
    }
}