using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace IdentityCard
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        private string identity;
        [DllImport("DLL_File.dll", CallingConvention = CallingConvention.Cdecl)]//注意这里的调用方式为Cdecl
        static extern int unpack(byte[] szSrcWltData, byte[] szDstPicData, int iIsSaveToBmp);

        private TcpListener tcpipServer; 
        TcpClient client = null;
        private NetworkStream ns;
        private Dictionary<string, string> lstMZ = new Dictionary<string, string>();
        bool checkStop = true;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            ReadNewModel();
            init();
        }

        /// <summary>
        /// 初始化控件数据
        /// </summary>
        void init()
        {
            DataTable activity   = SqlDbHelper.ExecuteDataTable("SELECT F_OrderId,F_MeetingName FROM F_Base_TempWorkOrder");
            DataRow dr           = activity.NewRow();
            dr["F_OrderId"]      = "0";
            dr["F_MeetingName"]  = "——请选择——";
            activity.Rows.InsertAt(dr, 0);
            ddlActivity.DataSource    = activity;
            ddlActivity.DisplayMember = "F_MeetingName";
            ddlActivity.ValueMember   = "F_OrderId";

            DataTable company        = SqlDbHelper.ExecuteDataTable("SELECT F_EmployerId,F_EmployerName FROM F_Base_Employer");
            dr                       = company.NewRow();
            dr["F_EmployerId"]       = "0";
            dr["F_EmployerName"]     = "——请选择——";
            company.Rows.InsertAt(dr, 0);
            ddlCompany.DataSource    = company;
            ddlCompany.DisplayMember = "F_EmployerName";
            ddlCompany.ValueMember   = "F_EmployerId";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            System.Net.IPHostEntry myEntry = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName());
            string ipAddress = myEntry.AddressList[0].ToString();

            tcpipServer = new TcpListener(IPAddress.Parse(ipAddress), 8234);
            tcpipServer.Start();

            //创立新线程循环搜索客户端并读取数据
            Thread th = new Thread(new ThreadStart(GetData));
            th.Start();

            btnStart.Enabled = false;
            btnStart.Text    = "已启动";
        }

        public void GetData()
        {
            while (true)
            {
                if( !checkStop )
                {
                    break;
                }

                try
                {
                    client = tcpipServer.AcceptTcpClient();
                    ns = client.GetStream();
                }
                catch( Exception e )
                {

                }

                try
                {
                    if (ns.CanRead)
                    {
                        lock (ns)
                        {
                            //报文全部转换成16进制
                            StringBuilder sb = new StringBuilder();
                            do
                            {
                                byte[] temp = new byte[200];
                                int num = ns.Read(temp, 0, temp.Length);

                                for (int i = 0; i < temp.Length; i++)
                                {
                                    sb.AppendFormat("{0:x2}" + " ", temp[i]);
                                }

                                Thread.Sleep(20);

                            } while (ns.DataAvailable);

                            int a = sb.ToString().ToUpper().Length;

                            string[] str_all = sb.ToString().ToUpper().Replace(" ", "").Split(new string[] { "1000400" }, StringSplitOptions.RemoveEmptyEntries);

                            string str_Name    = str_all[1].Trim().Substring(0, 60);
                            string str_Sex     = str_all[1].Trim().Substring(60, 4);
                            string str_Mz      = str_all[1].Trim().Substring(64, 8);
                            string str_Birth   = str_all[1].Trim().Substring(72, 32);
                            string str_Address = str_all[1].Trim().Substring(104, 140);
                            string str_idc     = str_all[1].Trim().Substring(244, 72);

                            label2.Text      = UnicodeToCharacter(str_Name);
                            label3.Text      = UnicodeToCharacter(str_Sex) == "1" ? "男" : "女";
                            label4.Text      = lstMZ[UnicodeToCharacter( str_Mz)];
                            string birth     = UnicodeToCharacter(str_Birth);
                            label5.Text      = birth.Substring(0, 4);
                            label6.Text      = birth.Substring( 4, 2 ).TrimStart('0');
                            label7.Text      = birth.Substring( 6, 2 ).TrimStart( '0' );
                            label9.Text      = UnicodeToCharacter(str_Address);
                            label8.Text      = UnicodeToCharacter(str_idc);

                            
                            byte[] byBgrBuffer = new byte[38556];    //解码后图片BGR编码值
                            byte[] byRgbBuffer = new byte[38808];    //解码后图片RGB编码值
                            byte[] byBmpBuffer = new byte[38862];    //解码后图片RGB编码值
                            unpack(HexStringToByteArray(str_all[1].Trim().Substring(512, 2048)), byBgrBuffer, 0);

                            //拼接BMP图片格式头，14字节
                            byte[] byBmpHead = new byte[14] { 0x42, 0x4D, 0xCE, 0x97, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00 };
                            Array.Copy(byBmpHead, 0, byBmpBuffer, 0, 14);

                            //拼接BMP图像信息，40字节
                            byte[] byBmpInfo = new byte[40]{   0x28,0x00,0x00,0x00,//结构所占用40字节    
                                0x66,0x00,0x00,0x00,//位图的宽度102像素
                                0x7E,0x00,0x00,0x00,//位图的高度126像素
                                0x01,0x00,          //目标设备的级别必须为1
                                0x18,0x00,          //每个像素所需的位数24
                                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00//......其他信息省略为0
                            };
                            Array.Copy(byBmpInfo, 0, byBmpBuffer, 14, 40);
                            //将解码后的BGR格式数据进行B、R互换
                            int iResult = bgr2rgb(byBgrBuffer, byBgrBuffer.Length, byRgbBuffer, byRgbBuffer.Length, 102, 126);
                            Array.Copy(byRgbBuffer, 0, byBmpBuffer, 54, iResult);
                            //写入文件
                            int iBmpSize = 54 + iResult;
                            tool_WriteOneFile(CardPath + label8.Text + ".bmp", byBmpBuffer, iBmpSize);
                            pictureBox2.BackgroundImage = Image.FromFile( CardPath + label8.Text + ".bmp" );

                            Insert(label8.Text);
                        }
                    }
                }
                catch (Exception e)
                {
//                    client = tcpipServer.AcceptTcpClient();
//                    ns = client.GetStream();
                    continue;
                }

                ns.Dispose();
            }
        }

        /// <summary>
        /// 将字节数组写入文件
        /// </summary>
        /// <param name="_i_strFileName">文件路径</param>
        /// <param name="_i_byData">写入字节数组</param>
        /// <param name="_i_iDataSize">字节数组大小</param>
        /// <returns></returns>
        static public bool tool_WriteOneFile(string _i_strFileName, byte[] _i_byData, int _i_iDataSize)
        {

            if (_i_iDataSize == 0)
            {
                return false;
            }

            FileStream fileStream = new FileStream(_i_strFileName, FileMode.Create);
            fileStream.Write(_i_byData, 0, _i_iDataSize);
            fileStream.Flush();
            fileStream.Close();

            return true;
        }

        /// <summary>
        /// 对BGR格式数据进行B、R转换，只支持24位深度图像
        /// </summary>
        /// <param name="_i_bySrc">bgr数据</param>
        /// <param name="_i_iSrcSize">bgr数据大小</param>
        /// <param name="_o_byDst">转换后的rgb格式数据，需要开((_i_iWidth * 3 + 3) / 4) * 4 * _i_iHeight字节空间</param>
        /// <param name="_i_iDstSize">开辟空间大小</param>
        /// <param name="_i_iWidth">图片宽度（像素）</param>
        /// <param name="_i_iHeight">图片高度（像素）</param>
        /// <returns>>0 执行成功，函数执行成功后返回转换后的rgb格式数据大小</returns>
        static public int bgr2rgb(byte[] _i_bySrc, int _i_iSrcSize, byte[] _o_byDst, int _i_iDstSize, int _i_iWidth, int _i_iHeight)
        {
            int iWidthSize = _i_iWidth * 3;
            int iDstWidthSize = ((_i_iWidth * 3 + 3) / 4) * 4;
            int iExternSize = ((_i_iWidth * 3 + 3) / 4) * 4 - _i_iWidth * 3;
            int iDstSize = iDstWidthSize * _i_iHeight;
            int iPosX = 0;
            int iPosY = 0;

            if (_i_iSrcSize != (iWidthSize * _i_iHeight))
            {
                return -1;
            }

            if (_i_iDstSize < iDstSize)
            {
                return -2;
            }

            for (iPosY = 0; iPosY < _i_iHeight; iPosY++)
            {
                for (iPosX = 0; iPosX < _i_iWidth * 3; iPosX += 3)
                {
                    _o_byDst[(iWidthSize + iExternSize) * iPosY + iPosX + 0] = _i_bySrc[iWidthSize * iPosY + iPosX + 2];
                    _o_byDst[(iWidthSize + iExternSize) * iPosY + iPosX + 1] = _i_bySrc[iWidthSize * iPosY + iPosX + 1];
                    _o_byDst[(iWidthSize + iExternSize) * iPosY + iPosX + 2] = _i_bySrc[iWidthSize * iPosY + iPosX + 0];
                }
            }

            return iDstSize;
        }

        private static byte[] HexStringToByteArray(string input)
        {
            var result = new byte[(input.Length + 1) / 2];
            var offset = 0;
            if (input.Length % 2 == 1)
            {
                // If length of input is odd, the first character has an implicit 0 prepended.
                result[0] = (byte)Convert.ToUInt32(input[0] + "", 16);
                offset = 1;
            }
            for (int i = 0; i < input.Length / 2; i++)
            {
                result[i + offset] = (byte)Convert.ToUInt32(input.Substring(i * 2 + offset, 2), 16);
            }
            return result;
        }
        public string UnicodeToCharacter(string text)
        {
            text.Replace(" ", "");
            byte[] arr = HexStringToByteArray(text);

            System.Text.UnicodeEncoding converter = new System.Text.UnicodeEncoding();

            string str = converter.GetString(arr);

            return str;

        }

        public static string CardPath
        {
            get
            {
                string path = Application.StartupPath + "\\photo\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;

            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {

        }

        private void buttonX3_Click( object sender, EventArgs e )
        {

        }

        private void btnExit_Click( object sender, EventArgs e )
        {
            if (tcpipServer != null)
            {
                if (ns != null)
                {
                    ns.Close();
                    client.Close();
                }

                tcpipServer.Stop();
                checkStop = false;
            }

            Application.Exit();
        }

        private void Form1_FormClosed( object sender, FormClosedEventArgs e )
        {
            if( tcpipServer != null )
            {
                if( ns != null )
                {
                    ns.Close();
                    client.Close();
                }

                tcpipServer.Stop();
                checkStop = false;
            }

            Application.Exit();
        }

        public void ReadNewModel()
        {
            lstMZ.Add( "01", "汉族" );
            lstMZ.Add( "02", "蒙古族" );
            lstMZ.Add( "03", "回族" );
            lstMZ.Add( "04", "藏族" );
            lstMZ.Add( "05", "维吾尔族" );
            lstMZ.Add( "06", "苗族" );
            lstMZ.Add( "07", "彝族" );
            lstMZ.Add( "08", "壮族" );
            lstMZ.Add( "09", "布依族" );
            lstMZ.Add( "10", "朝鲜族" );
            lstMZ.Add( "11", "满族" );
            lstMZ.Add( "12", "侗族" );
            lstMZ.Add( "13", "瑶族" );
            lstMZ.Add( "14", "白族" );
            lstMZ.Add( "15", "土家族" );
            lstMZ.Add( "16", "哈尼族" );
            lstMZ.Add( "17", "哈萨克族" );
            lstMZ.Add( "18", "傣族" );
            lstMZ.Add( "19", "黎族" );
            lstMZ.Add( "20", "傈僳族" );
            lstMZ.Add( "21", "佤族" );
            lstMZ.Add( "22", "畲族" );
            lstMZ.Add( "23", "高山族" );
            lstMZ.Add( "24", "拉祜族" );
            lstMZ.Add( "25", "水族" );
            lstMZ.Add( "26", "东乡族" );
            lstMZ.Add( "27", "纳西族" );
            lstMZ.Add( "28", "景颇族" );
            lstMZ.Add( "29", "柯尔克孜族" );
            lstMZ.Add( "30", "土族" );
            lstMZ.Add( "31", "达翰尔族" );
            lstMZ.Add( "32", "仫佬族" );
            lstMZ.Add( "33", "羌族" );
            lstMZ.Add( "34", "布朗族" );
            lstMZ.Add( "35", "撒拉族" );
            lstMZ.Add( "36", "毛南族" );
            lstMZ.Add( "37", "仡佬族" );
            lstMZ.Add( "38", "锡伯族" );
            lstMZ.Add( "39", "阿昌族" );
            lstMZ.Add( "40", "普米族" );
            lstMZ.Add( "41", "塔吉克族" );
            lstMZ.Add( "42", "怒族" );
            lstMZ.Add( "43", "乌孜别克族" );
            lstMZ.Add( "44", "俄罗斯族" );
            lstMZ.Add( "45", "鄂温克族" );
            lstMZ.Add( "46", "德昂族" );
            lstMZ.Add( "47", "保安族" );
            lstMZ.Add( "48", "裕固族" );
            lstMZ.Add( "49", "京族" );
            lstMZ.Add( "50", "塔塔尔族" );
            lstMZ.Add( "51", "独龙族" );
            lstMZ.Add( "52", "鄂伦春族" );
            lstMZ.Add( "53", "赫哲族" );
            lstMZ.Add( "54", "门巴族" );
            lstMZ.Add( "55", "珞巴族" );
            lstMZ.Add( "56", "基诺族" );
            lstMZ.Add( "57", "其它" );
            lstMZ.Add( "98", "外国人入籍" );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity">身份证号</param>
        public void Insert( string identity )
        {
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@identity", identity ) );

            int checkBlack   = SqlDbHelper.ExecuteScalar( "SELECT COUNT(*) AS num FROM LR_Base_TempUser WHERE F_Identity=@identity AND F_EnabledMark=0", list.ToArray() );

            //判断是否是黑名单用户
            if ( checkBlack > 0 )
            {
                label13.ForeColor = Color.Red;
                label13.Text      = "无效用户！";
            }
            else
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label10.Text = DateTime.Now.AddSeconds(0).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void panelEx1_Click( object sender, EventArgs e )
        {

        }

        /// <summary>
        /// 人员采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX3_Click_1(object sender, EventArgs e)
        {
            string company  = ddlCompany.SelectedValue.ToString();
            string activity = ddlActivity.SelectedValue.ToString();
            string type     = string.Empty;

            try
            {
                type = ddlType.SelectedValue.ToString();
            }
            catch
            {
                type = "0";
            }

            if (activity == "0")
            {
                MessageBox.Show("请选择一个活动！");
            }
            else if ( company == "0" )
            {
                MessageBox.Show("请选择一个劳务公司！");
            }
            else if (type == "0")
            {
                MessageBox.Show("请选择一个工种！");
            }
            else if (string.IsNullOrEmpty(identity))
            {
                MessageBox.Show("没有检测到打卡人！");
            }
            else
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@identity", identity));

                //获取小时工信息
                DataTable userInfo = SqlDbHelper.ExecuteDataTable("SELECT f_userid,F_RealName,F_Gender FROM MLR_Base_TempUser WHERE F_Identity=@identity", list.ToArray() );

                list = new List<SqlParameter>();
                list.Add(new SqlParameter("@orderID", ddlActivity));
                list.Add(new SqlParameter("@userID", userInfo.Rows[ 0 ]["f_userid"].ToString() ));

                int num = Convert.ToInt32( SqlDbHelper
                    .ExecuteScalar(
                        "SELECT COUNT(*) AS num FROM F_Base_TempWorkOrderUserDetail WHERE F_TempWorkOrderId=@orderID AND f_userID=@userID", list.ToArray() )
                    .ToString() );

                if ( num > 0 )
                {
                    MessageBox.Show("活动里已存在该临时工！");
                }
                else
                {
                    list.Add(new SqlParameter("@f_id", Guid.NewGuid().ToString()));
                    list.Add(new SqlParameter("@f_employerid", company));
                    list.Add(new SqlParameter("@f_categoryid", type));

                    //添加小时工与活动对应关系
                    SqlDbHelper.ExecuteNonQuery("INSERT F_Base_TempWorkOrderUserDetail(f_id,F_TempWorkOrderId,f_userid,f_employerid,f_categoryid,F_WorkSubstitute) VALUES(@f_id,@orderID,@userID,@f_employerid,@f_categoryid,0)", list.ToArray());

                    //--------------------------往打卡记录表中初始经小时工打卡记录------------------------------
                    list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@orderID", ddlActivity));

                    DataTable dt       = SqlDbHelper.ExecuteDataTable( "SELECT * FROM F_Base_TempWorkOrder WHERE f_orderid=@orderID", list.ToArray() );
                    DateTime startTime = DateTime.Parse(dt.Rows[0]["F_StartTime"].ToString());
                    DateTime endTime   = DateTime.Parse(dt.Rows[0]["F_EndTime"].ToString());

                    for (DateTime t = startTime; t <= endTime; t = t.AddDays(1))
                    {
                        list.Add(new SqlParameter("@F_RecordId", Guid.NewGuid().ToString()));
                        list.Add(new SqlParameter("@F_Identity", identity));
                        list.Add(new SqlParameter("@F_CreateTime", t));
                        list.Add(new SqlParameter("@F_RealName", userInfo.Rows[0]["F_RealName"].ToString() ));
                        list.Add(new SqlParameter("@F_Gender", userInfo.Rows[0]["F_Gender"].ToString() ));
                        list.Add(new SqlParameter("@F_RecordDate", t.ToString("yyyy-MM-dd")));

                        //自动给临时工往打卡记录里初始化打卡记录
                        SqlDbHelper.ExecuteNonQuery("INSERT LR_Base_CardRecord(F_RecordId,F_Identity,F_CreateTime,F_RealName,F_Gender,F_OrderId,F_RecordDate) VALUES (@F_RecordId,@F_Identity,@F_CreateTime,@F_RealName,@F_Gender,@orderID,@F_RecordDate)", list.ToArray() );
                    }
                }

                identity = string.Empty;
            }
        }

        private void ddlActivity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ddlActivity_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void ddlActivity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //根据选择的活动只获取这个活动下的工种
            DataTable type = SqlDbHelper.ExecuteDataTable("SELECT DISTINCT t.F_CategoryId,t.F_CategoryName FROM LR_Base_Category t INNER JOIN F_Base_TempWorkOrderCategoryDetail c ON t.F_CategoryId=c.F_CategoryName WHERE c.F_TempWorkOrderId='" + ddlActivity.SelectedValue.ToString() + "'");

            DataRow dr = type.NewRow();
            dr["F_CategoryId"] = "0";
            dr["F_CategoryName"] = "——请选择——";
            type.Rows.InsertAt(dr, 0);
            ddlType.DataSource = type;
            ddlType.DisplayMember = "F_CategoryName";
            ddlType.ValueMember = "F_CategoryId";
        }
    }
}
