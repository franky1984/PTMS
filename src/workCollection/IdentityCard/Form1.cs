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
        }

        /// <summary>
        /// 初始化控件数据
        /// </summary>
        void init()
        {
            DataTable company        = SqlDbHelper.ExecuteDataTable("SELECT * FROM F_Base_Employer");
            ddlCompany.DataSource    = company;
            ddlCompany.DisplayMember = "F_EmployerName";
            ddlCompany.ValueMember   = "F_EmployerId";
            ddlCompany.Items.Insert(0, "请选择");

            DataTable type           = SqlDbHelper.ExecuteDataTable("SELECT * FROM ");
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
            buttonX3.Enabled = true; //启动采集
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
        /// 更新临时工上、下班打卡时间
        /// </summary>
        /// <param name="identity">身份证号</param>
        public void Insert( string identity )
        {
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@identity", identity ) );

            int checkBlack   = SqlDbHelper.ExecuteScalar( "SELECT COUNT(*) AS num FROM LR_Base_TempUser WHERE F_Identity=@identity AND F_EnabledMark=0", list.ToArray() );
            buttonX3.Enabled = true; //启动采集

            //判断是否是黑名单用户
            if ( checkBlack > 0 )
            {
                label13.ForeColor = Color.Red;
                label13.Text      = "无效用户！";
                buttonX3.Enabled  = false; //启动采集
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
    }
}
