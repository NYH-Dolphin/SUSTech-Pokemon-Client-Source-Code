using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;
using MySql.Data.MySqlClient;

public class Mysql : MonoBehaviour
{
    private static string host = "127.0.0.1";

    private static string port = "3306";

    private static string id = "root";

    private static string password = "20010922nyh";

    private static string database = "sustechpokemon";

    private static string table = "user";

    private static string charset = "utf8mb4";


    private static string connection = string.Format(
        "server={0};port={1};Pooling=False;Max Pool Size = 1024;Initial Catalog=abc;Persist Security Info=True;User ID={2};Password={3};charset={4};database={5}",
        host, port, id, password, charset, database
    );

    // Start is called before the first frame update
    void Start()
    {
        
        getRowByRowNum(1,connection, table);
        // MySqlConnection conn = getConnection(connection);
        // Debug.Log(conn.Database);
        // Debug.Log(conn.State);


        // conn.Close();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private MySqlConnection getConnection(string connection)
    {
        Debug.Log(connection);
        MySqlConnection conn = new MySqlConnection(connection);
        conn.Open();

        // conn.Close();
        return conn;
    }

    public void getRowByRowNum(int rowNum, string connection, string tableName)
    {
        List<string> temp = new List<string>();
        string sql = "SELECT * FROM " + tableName + " limit " + rowNum.ToString() + ",1";
        DataTableCollection dataTableCollection = GetTable(connection, sql);

        for (int i = 0; i < dataTableCollection[0].Columns.Count; i++)
        {
            string str = dataTableCollection[0].Rows[0][i].ToString();
            Debug.Log(str);
        }
    }

    private DataTableCollection GetTable(string s, string sql)
    {
        MySqlDataAdapter da = new MySqlDataAdapter(sql, getConnection(connection));
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds.Tables;
    }
}