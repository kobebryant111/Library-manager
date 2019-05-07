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
    public partial class Form4 : Form
    {
        string strcon = BOOKSYS.Properties.Settings.Default.MBOOKConnectionString; //获取连接字符串
        string FileNamePath = "";
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“mBOOKDataSet.TReader”中。您可以根据需要移动或移除它。
            this.tReaderTableAdapter.Fill(this.mBOOKDataSet.TReader);
            
        }

        private void button1_Click(object sender, EventArgs e)//读者添加
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("请输入完整！");
                return;     //如果没输入完整则返回
            }
            string sqlStr;
            SqlConnection conn = new SqlConnection(strcon);//新建数据库连接对象
            if (FileNamePath != "")    //如果选择了照片
            {
                sqlStr = "insert [TReader]([ReaderID],[Name],[Sex],[Spec],[Born],[Photo],[Detail])values(@ReaderID,@Name,@Sex,@Spec,@Born,@Photo,@Detail)";   //设置Sql语句
            }
            else                                                //如果现在没照片
            {
                sqlStr = "insert [TReader]([ReaderID],[Name],[Sex],[Spec],[Born],[Detail])values(@ReaderID,@Name,@Sex,@Spec,@Born,@Detail)";   //设置Sql语句
            }
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            //添加参数
            cmd.Parameters.Add("@ReaderID", SqlDbType.Char, 8).Value = textBox1.Text.Trim();
            cmd.Parameters.Add("@Name", SqlDbType.Char, 8).Value = textBox2.Text.Trim();
            if (radioButton1.Checked == true)               //如果性别是男
            {
                cmd.Parameters.Add("@Sex", SqlDbType.Bit).Value = true;
            }
            else if (radioButton2.Checked == true)          //如果性别是女
            {
                cmd.Parameters.Add("@Sex", SqlDbType.Bit).Value = false;
            }
            else
            {
                MessageBox.Show("请选择性别");
            }
            cmd.Parameters.Add("@Spec", SqlDbType.Char, 12).Value = comboBox1.Text;//专业
            cmd.Parameters.Add("@Born", SqlDbType.Date).Value = textBox3.Text.Trim();//出生时间
            cmd.Parameters.Add("@Detail", SqlDbType.NText).Value = textBox4.Text.Trim();//详细信息
            if (FileNamePath != "")//如果选择了照片
            {
                FileStream fs = null;//以文件流方式读取照片
                fs = new FileStream(FileNamePath, FileMode.Open, FileAccess.Read);
                MemoryStream mem = new MemoryStream();//实例化内存流对象mem
                byte[] data1 = new byte[fs.Length];//定义照片长度的数组
                fs.Read(data1, 0, (int)fs.Length);//把照片存到数组中
                cmd.Parameters.Add("@Photo", SqlDbType.Image);		//这里选择Image类型  
                cmd.Parameters["@Photo"].Value = data1;             //给@Photo参数赋值
            }
            try
            {
                conn.Open();                                    //打开数据库连接
                cmd.ExecuteNonQuery();  //执行SQL语句
                MessageBox.Show("保存成功！");
                //重新绑定dataGridView1，此段代码是配置数据源时机器自动生成，在Form1_Load方法中
                this.tReaderTableAdapter.Fill(this.mBOOKDataSet.TReader);
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错！" + ex.Message);
            }
            finally
            {
                conn.Close();       //关闭数据库连接
                FileNamePath = "";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();       //实例化打开文件对话框
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "jpg图片|*.jpg|gif图片|*.gif|所有文件(*.*)|*.*";//设置打开文件类型
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileNamePath = openFileDialog.FileName;         //获取文件路径
                pictureBox1.Image = Image.FromFile(FileNamePath);//将图片显示在pictureBox中
            }
        }

        private void button2_Click(object sender, EventArgs e)  //读者删除
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入借书证号");
                return;
            }
            SqlConnection conn = new SqlConnection(strcon);
            string sqlStr = "Delete From [TReader] where [ReaderID]=@ReaderID";
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            cmd.Parameters.Add("@ReaderID", SqlDbType.Char, 8).Value = textBox1.Text.Trim();
            try
            {
                conn.Open();
                int a = cmd.ExecuteNonQuery();                        		//执行SQL语句
                this.tReaderTableAdapter.Fill(this.mBOOKDataSet.TReader);  	// dataGridView1重新绑定
                if (a == 1)									//如果受影响的行数为1则删除成功
                { MessageBox.Show("删除成功！"); }
                else
                { MessageBox.Show("数据库中没有此读者！"); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            { conn.Close(); }

        }

        private void button3_Click(object sender, EventArgs e)//读者修改
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入借书证号!");
                return;
            }
            //获取连接字符串
            SqlConnection conn = new SqlConnection(strcon);
            string sqlStr = "update [TReader] set";
            if (textBox2.Text.Trim().ToString() != "")//如果姓名有输入
            {
                sqlStr += "[Name]='" + textBox2.Text.Trim() + "',";
            }
            if (textBox3.Text.Trim() != "")//如果出生时间有输入
            {
                sqlStr += "[Born]='" + textBox3.Text.Trim() + "',";
            }
            if (FileNamePath != "")//如果选择了照片
            {
                sqlStr += "[Photo]=@Photo,";
            }
            if (textBox4.Text.Trim() != "")
            {
                sqlStr += "[Detail]='" + textBox4.Text.Trim() + "',";
            }
            sqlStr += "[Spec]='" + comboBox1.Text + "'," + "[Sex]=@Sex";
            sqlStr += " where ReaderID='" + textBox1.Text.Trim() + "'";
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            if (radioButton1.Checked == true)//如果性别是男
            {
                cmd.Parameters.Add("@Sex", SqlDbType.Bit).Value = true;
            }
            else if (radioButton2.Checked == true)//如果性别是女
            {
                cmd.Parameters.Add("@Sex", SqlDbType.Bit).Value = false;
            }
            else
            {
                MessageBox.Show("请选择性别");
                return;
            }
            if (FileNamePath != "")         //如果选择了照片
            {
                FileStream fs = null;
                fs = new FileStream(FileNamePath, FileMode.Open, FileAccess.Read);
                MemoryStream mem = new MemoryStream();
                byte[] data1 = new byte[fs.Length];
                fs.Read(data1, 0, (int)fs.Length);
                cmd.Parameters.Add("@Photo", SqlDbType.VarBinary);		//这里选择VarBinary类型  
                cmd.Parameters["@Photo"].Value = data1;             //把照片变化成字节数组
            }
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
                    MessageBox.Show("数据库中没有此读者！");
                }
                this.tReaderTableAdapter.Fill(this.mBOOKDataSet.TReader);

            }
            catch (Exception ex)
            {
                MessageBox.Show("出错，没有完成读者的修改！" + ex.Message);
            }
            finally
            {
                conn.Close();
                FileNamePath = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)//读者查询
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入借书证号!");
                return;
            }
            SqlConnection conn = new SqlConnection(strcon);
            string sqlStrSelect = "select [ReaderID],[Name],[Sex],[Spec],[Born],[Photo],[Num],[Detail] from [TReader] where [ReaderID]='" + textBox1.Text.Trim() + "'";
            SqlCommand cmd = new SqlCommand(sqlStrSelect, conn);
            try
            {
                conn.Open();//打开数据库连接
                SqlDataReader sdr = cmd.ExecuteReader();
                MemoryStream memStream = null;//定义一个内存流
                if (sdr.HasRows)    //如果有记录
                {
                    sdr.Read();//读取第一行记录
                    textBox2.Text = sdr["Name"].ToString();//读取姓名
                    textBox3.Text = sdr["Born"].ToString();//读取出生时间
                    comboBox1.Text = sdr["Spec"].ToString();//读取专业
                    label7.Text = sdr["Num"].ToString() + "本";//读取所借的书本数
                    bool sex = Convert.ToBoolean(sdr["Sex"]);//读取性别
                    if (sex == true)
                    {
                        radioButton1.Checked = true;
                    }
                    else
                    {
                        radioButton2.Checked = true;
                    }
                    if (sdr["Photo"] != System.DBNull.Value)//如果有照片
                    {
                        if (this.pictureBox1.Image != null)//如果pictureBox1中有图片销毁
                        {
                            pictureBox1.Image.Dispose();
                        }

                        byte[] data = (byte[])sdr["Photo"];
                        memStream = new MemoryStream(data); //字节流转换为内存流
                        this.pictureBox1.Image = Image.FromStream(memStream);//内存流转换为照片
                    }
                    else
                    {
                        this.pictureBox1.Image = null;
                    }
                    textBox4.Text = sdr["Detail"].ToString();
                }
                else
                {
                    MessageBox.Show("没有此读者");
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                button4_Click(null, null);
            }
        }
    }
}
