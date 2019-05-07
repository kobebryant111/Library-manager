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
    public partial class Form3 : Form
    {
        string strcon = BOOKSYS.Properties.Settings.Default.MBOOKConnectionString; //获取连接字符串
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)     //还书
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入图书ID！");
                return;
            }
            SqlConnection conn = new SqlConnection(strcon);
            string sqlStr = "Delete From [TLend] where [BookID]=@BookID";
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            cmd.Parameters.Add("@BookID", SqlDbType.Char, 10).Value = textBox1.Text.Trim();
            try
            {
                conn.Open();
                int a = cmd.ExecuteNonQuery();                        		//执行SQL语句
                this.tLendTableAdapter.Fill(this.mBOOKDataSet6.TLend);  	// dataGridView1重新绑定
                if (a == 1)									//如果受影响的行数为1则删除成功
                { MessageBox.Show("还书成功！"); }
                else
                { MessageBox.Show("该图书没有被借阅！"); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            { conn.Close(); }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“mBOOKDataSet6.TLend”中。您可以根据需要移动或删除它。
            this.tLendTableAdapter.Fill(this.mBOOKDataSet6.TLend);

        }
    }
}
