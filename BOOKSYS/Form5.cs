using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.IO;
namespace BOOKSYS
{
    public partial class Form5 : Form
    {
        string strcon = BOOKSYS.Properties.Settings.Default.MBOOKConnectionString; //获取连接字符串

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“mBOOKDataSet4.TBook”中。您可以根据需要移动或删除它。
            this.tBookTableAdapter1.Fill(this.mBOOKDataSet4.TBook);
        }

        private void button3_Click(object sender, EventArgs e)   //图书添加
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox7.Text == ""|| textBox4.Text == ""|| textBox5.Text == ""|| textBox6.Text == ""|| textBox8.Text == ""|| textBox9.Text == "")
            {
                MessageBox.Show("请输入完整！");
                return;     //如果没输入完整则返回
            }
            string sqlStr;
            SqlConnection conn = new SqlConnection(strcon);//新建数据库连接对象
            sqlStr = "insert [TBook]([ISBN],[BookName],[Author],[Publisher],[Price],[CNum],[SNum],[Summary])values(@ISBN,@BookName,@Author,@Publisher,@Price,@CNum,@SNum,@Summary)";   //设置Sql语句
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            //添加参数
            cmd.Parameters.Add("@ISBN", SqlDbType.Char, 18).Value = textBox6.Text.Trim();
            cmd.Parameters.Add("@BookName", SqlDbType.Char, 40).Value = textBox2.Text.Trim();
            cmd.Parameters.Add("@Author", SqlDbType.Char, 16).Value = textBox3.Text.Trim();
            cmd.Parameters.Add("@Publisher", SqlDbType.Char, 30).Value = textBox4.Text.Trim();
            cmd.Parameters.Add("@Price", SqlDbType.Float).Value = textBox5.Text.Trim();
            cmd.Parameters.Add("@CNum", SqlDbType.Int).Value = textBox7.Text.Trim();
            cmd.Parameters.Add("@SNum", SqlDbType.Int).Value = textBox8.Text.Trim();
            cmd.Parameters.Add("@Summary", SqlDbType.VarChar, 200).Value = textBox9.Text.Trim();
            try
            {
                conn.Open();                                    //打开数据库连接
                cmd.ExecuteNonQuery();  //执行SQL语句
                MessageBox.Show("保存成功！");
                //重新绑定dataGridView1，此段代码是配置数据源时机器自动生成，在Form1_Load方法中
                this.tBookTableAdapter1.Fill(this.mBOOKDataSet4.TBook);
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错！" + ex.Message);
            }
            finally
            {
                conn.Close();       //关闭数据库连接
            }
        }

        private void button1_Click(object sender, EventArgs e)   //图书删除
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入图书书名");
                return;
            }
            SqlConnection conn = new SqlConnection(strcon);
            string sqlStr = "Delete From [TBook] where [BookName]=@BookName";
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            cmd.Parameters.Add("@BookName", SqlDbType.Char, 40).Value = textBox1.Text.Trim();
            try
            {
                conn.Open();
                int a = cmd.ExecuteNonQuery();                        		//执行SQL语句
                this.tBookTableAdapter1.Fill(this.mBOOKDataSet4.TBook);  	// dataGridView1重新绑定
                if (a == 1)									//如果受影响的行数为1则删除成功
                { MessageBox.Show("删除成功！"); }
                else
                { MessageBox.Show("数据库中没有此书！"); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            { conn.Close(); }
        }

        private void button2_Click(object sender, EventArgs e)  //图书修改
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入图书书名!");
                return;
            }
            //获取连接字符串
            SqlConnection conn = new SqlConnection(strcon);
            string sqlStr = "update [TBook] set";
            if (textBox3.Text.Trim().ToString() != "")//如果有输入
            {
                sqlStr += "[Author]='" + textBox3.Text.Trim() + "',";
            }
            if (textBox4.Text.Trim().ToString() != "")//如果有输入
            {
                sqlStr += "[Publisher]='" + textBox4.Text.Trim() + "',";
            }
            if (textBox5.Text.Trim().ToString() != "")//如果有输入
            {
                sqlStr += "[Price]='" + textBox5.Text.Trim() + "',";
            }
            if (textBox7.Text.Trim().ToString() != "")//如果有输入
            {
                sqlStr += "[CNum]='" + textBox7.Text.Trim() + "',";
            }
            if (textBox8.Text.Trim().ToString() != "")//如果有输入
            {
                sqlStr += "[SNum]='" + textBox8.Text.Trim() + "',";
            }
            if (textBox9.Text.Trim().ToString() != "")//如果有输入
            {
                sqlStr += "[Summary]='" + textBox9.Text.Trim() + "'";
            }
            sqlStr += " where BookName='" + textBox1.Text.Trim() + "'";
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            try
            {
                conn.Open();
                int yxh = cmd.ExecuteNonQuery();
                if (yxh == 1)   //如果受影响的行数为1则修改成功
                {
                    MessageBox.Show("修改成功");
                }
                else
                {
                    MessageBox.Show("数据库中没有此图书！");
                }
                this.tBookTableAdapter1.Fill(this.mBOOKDataSet4.TBook);

            }
            catch (Exception ex)
            {
                MessageBox.Show("出错，没有完成图书信息的修改！" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)    //图书查询
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入图书书名!");
                return;
            }
            SqlConnection conn = new SqlConnection(strcon);
            string sqlStrSelect = "select [ISBN],[BookName],[Author],[Publisher],[Price],[CNum],[SNum],[Summary] from [TBook] where [BookName]='" + textBox1.Text.Trim() + "'";
            SqlCommand cmd = new SqlCommand(sqlStrSelect, conn);
            try
            {
                conn.Open();//打开数据库连接
                SqlDataReader sdr = cmd.ExecuteReader();
                MemoryStream memStream = null;//定义一个内存流
                if (sdr.HasRows)    //如果有记录
                {
                    sdr.Read();//读取第一行记录
                    textBox2.Text = sdr["BookName"].ToString();//读取书名
                    textBox3.Text = sdr["Author"].ToString();//读取作者
                    textBox4.Text = sdr["Publisher"].ToString();//读取出版社
                    textBox5.Text = sdr["Price"].ToString();//读取价格
                    textBox6.Text = sdr["ISBN"].ToString();//读取ISBN
                    textBox7.Text = sdr["CNum"].ToString();//读取数量
                    textBox8.Text = sdr["SNum"].ToString();//读取库存量
                    textBox9.Text = sdr["Summary"].ToString();//读取内容摘要
                }
                else
                {
                    MessageBox.Show("没有此图书");
                }
                if (memStream != null)//如果内存流不为空则关闭
                {
                    memStream.Close();
                }
                if (!sdr.IsClosed)
                {
                    sdr.Close();//关闭sdr
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

                if (conn.State == ConnectionState.Open) //如果数据处于连接状态，关闭连接
                {
                    conn.Close();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)   //模糊查询
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入图书书名关键字!");
                return;
            }
            SqlConnection conn = new SqlConnection(strcon);
            string sqlStrSelect = "select [ISBN],[BookName],[Author],[Publisher],[Price],[CNum],[SNum],[Summary] from [TBook] where [BookName] like '%" + textBox1.Text.Trim() + "%'";
            SqlCommand cmd = new SqlCommand(sqlStrSelect, conn);
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sqlStrSelect, conn);    //实例化数据库适配器
                DataSet dstable = new DataSet();
                adapter.Fill(dstable, "testTable");
                dataGridView1.DataSource = dstable.Tables["testTable"];
                dataGridView1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错！" + ex.Message);
            }
            finally
            {
                conn.Close();       //关闭数据库连接
            }
        }
    }
}
