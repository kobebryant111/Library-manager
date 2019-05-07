using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace BOOKSYS
{
    public partial class Form2 : Form
    {
        string strcon = @"Data Source=69F638102711447\SQL2008;Initial Catalog=MBOOK;User ID=sa;Password=123456";
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)//查询
        {

            SqlConnection conn = new SqlConnection(strcon);
            string sqlStrSelect = "select [BookID],[ISBN],[BookName],[Publisher],[Price],[LTime] from [RBL] where [ReaderID]='" + textBox1.Text.Trim() + "'";
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sqlStrSelect, conn);	//实例化数据库适配器
                DataSet dstable = new DataSet();							//定义数据集dstable
                adapter.Fill(dstable, "testTable");							//填充数据集
                dataGridView1.DataSource = dstable.Tables["testTable"];	//表testTable为dataGridView1数据源
                dataGridView1.Show();								//显示dataGridView1数据
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            { conn.Close(); }

        }

        private void button2_Click(object sender, EventArgs e)//借书
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "")
            {
                MessageBox.Show("借书证号，ISBN，图书ID输入完整！");
                return;
            }
            SqlConnection conn = new SqlConnection(strcon); 				//新建数据库连接对象
            SqlCommand cmd = new SqlCommand("Book_Borrow", conn); 	//新建数据库命令对象
            cmd.CommandType = CommandType.StoredProcedure;			//设置命令类型为存储过程
            SqlParameter inReaderID = new SqlParameter("@in_ReaderID", SqlDbType.Char, 8);
            inReaderID.Direction = ParameterDirection.Input;				//参数类型为输入参数
            inReaderID.Value = textBox1.Text.Trim();					//给参数赋值
            cmd.Parameters.Add(inReaderID);
            SqlParameter inISBN = new SqlParameter("@in_ISBN", SqlDbType.Char, 18);
            inISBN.Direction = ParameterDirection.Input;
            inISBN.Value = textBox2.Text.Trim();
            cmd.Parameters.Add(inISBN);
            SqlParameter inBookID = new SqlParameter("@in_BookID", SqlDbType.Char, 8);
            inBookID.Direction = ParameterDirection.Input;
            inBookID.Value = textBox3.Text.Trim();
            cmd.Parameters.Add(inBookID);
            SqlParameter outReturn = new SqlParameter("@out_str", SqlDbType.Char, 30);
            outReturn.Direction = ParameterDirection.Output;				//参数类型为输出参数
            cmd.Parameters.Add(outReturn);							//添加参数
            try
            {
                conn.Open();									//打开数据库连接
                cmd.ExecuteNonQuery();							//执行存储过程
                MessageBox.Show(outReturn.Value.ToString());			//输出数据库返回的信息
            }
            catch
            { MessageBox.Show("借书出错！"); }
            finally
            {
                conn.Close();									//关闭数据库连接
                button1_Click(null, null);					   //调用button1_Click方法
            }

        }
    }
}
