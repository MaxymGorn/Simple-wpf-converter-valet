using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<valet> List = new List<valet>();
        XmlDocument doc = new XmlDocument();
        public MainWindow()
        {
            InitializeComponent();
            LoadVallet();

        }


        class valet
        {
            public string r030 { get; set; }
            public string txt { get; set; }
            public string rateStr { get; set; }
            public string cc { get; set; }
            public decimal rate { get; set; }
        }
        void LoadVallet()
        {
            try
            {
                if (!CheckInternetConnection())
                {

                    throw new Exception();
                }
                doc.Load("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange");
                doc.Save("Valet.xml");
            }
            catch(Exception er)
            {
                doc.Load("Valet.xml");
            }
            finally
            {
                XmlElement xRoot = doc.DocumentElement;

                foreach (XmlNode node in doc.GetElementsByTagName("currency"))
                {
                    valet valet = new valet();
                    foreach (XmlNode childnode in node.ChildNodes)
                    {
                        if (childnode.Name == "rate")
                            valet.rateStr = childnode.InnerText;
                        else if (childnode.Name == "cc")
                            valet.cc = childnode.InnerText;
                        else if (childnode.Name == "txt")
                            valet.txt = childnode.InnerText;
                        else if (childnode.Name == "r030")
                            valet.r030 = childnode.InnerText;
                    }
                    List.Add(valet);
                }
                DisplayRateVallet();
            }


        }
        void DisplayRateVallet()
        {
            foreach(var el in List)
            {
                if(el.cc=="USD")
                {
                    USDText.Text =Convert.ToString(el.rateStr.Replace('.',','));
                }
                else if (el.cc == "EUR")
                {
                    EURText.Text = Convert.ToString(el.rateStr.Replace('.', ','));
                }
                else  if (el.cc == "CAD")
                {
                    CADText.Text = Convert.ToString(el.rateStr.Replace('.', ','));
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
          
        }
        static valet GetValetByCurrency(string currency, List<valet> valets)


        {
            foreach (valet v in valets)
            {
                if (v.cc == currency)
                    return v;
            }
            return null;
        }
        private bool CheckInternetConnection()
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.google.ru/");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.Timeout = 10000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream ReceiveStream1 = response.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream1, true);
                string responseFromServer = sr.ReadToEnd();

                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Нет подключения к интернету!\nПроверьте ваш фаервол или настройки сетевого подключения...", "Notifications", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;

            }
        }
        string Select1(int op, decimal tmp, string b) => op switch
        {
            1 => Convert.ToString(tmp * decimal.Parse(EURText.Text) / decimal.Parse(b)),
            2 => Convert.ToString(tmp * decimal.Parse(USDText.Text) / decimal.Parse(b)),
            3 => Convert.ToString(tmp * decimal.Parse(CADText.Text) / decimal.Parse(b)),
            _ => throw new ArgumentException("Недопустимый код операции")
        };
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            resUsd.Text = "";
            resEur.Text = "";
            resCad.Text = "";
  
                decimal tmp = 0;

                if (decimal.TryParse(Text.Text, out tmp))
                {
                    if (radiobtnbuy.IsChecked == true)
                    {

                    }
                    else if (radiobtnbuy.IsChecked == false)
                    {
                        tmp *= 1.0012m;
                    }
                    if (RadEur.IsChecked == true)
                    {
                        if (checkUsd.IsChecked == true)
                        {
                            resUsd.Text = Select1(1, tmp, USDText.Text);
                        }
                        if (checkCad.IsChecked == true)
                        {
                            resCad.Text = Select1(1, tmp, CADText.Text);
                        }
                    }
                    if (RadUsd.IsChecked == true)
                    {
                        if (checkEur.IsChecked == true)
                        {
                            resEur.Text = Select1(2, tmp, EURText.Text);
                        }
                        if (checkCad.IsChecked == true)
                        {
                            resCad.Text = Select1(2, tmp, CADText.Text);
                        }
                    }
                    if (RadCad.IsChecked == true)
                    {
                        if (checkUsd.IsChecked == true)
                        {
                            resUsd.Text = Select1(3, tmp, USDText.Text);
                        }
                        if (checkEur.IsChecked == true)
                        {
                            resEur.Text = Select1(3, tmp, EURText.Text);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Eror Parse", "Notifications", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                }
            
           
          
              
    
        }

        private void RadEur_Checked(object sender, RoutedEventArgs e)
        {
            def.Source = eurimg.Source;
        }

        private void RadUsd_Checked(object sender, RoutedEventArgs e)
        {
            def.Source = usaimg.Source;
        }

        private void RadCad_Checked(object sender, RoutedEventArgs e)
        {
            def.Source = cadimg.Source;
        }
    }
}
