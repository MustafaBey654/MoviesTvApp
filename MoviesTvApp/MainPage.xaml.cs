
using Microsoft.Maui.Controls.PlatformConfiguration;
using MoviesTvApp.Models;
using MoviesTvApp.Pages;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MoviesTvApp;

public partial class MainPage : ContentPage
{
    List<Tv> tvListesi = new List<Tv>();
    XmlDocument doc = new XmlDocument();

    public MainPage()
    {
        InitializeComponent();

        if(DeviceInfo.Platform == DevicePlatform.Android)
        {
            AndoridPlatform();
        }
        else
        {
            WinPlatfrom();
        }




        listContacts.ItemsSource = tvListesi;

    }

    async void AndoridPlatform()
    {

        try
        {
            //string path = Path.Combine(FileSystem.AppDataDirectory, "MyTvies.xml");
            Stream path = await FileSystem.Current.OpenAppPackageFileAsync("MyTvies.xml");
            //if(!File.Exists(path))
            //{
            //    using StreamWriter file = new StreamWriter(path);
            //}

            using (StreamReader sr = new StreamReader(path))
            {
                string okunan = sr.ReadToEnd();
                doc.LoadXml(okunan);
            }

            using (XmlNodeList nodes = doc.GetElementsByTagName("tv"))
            {

                foreach (XmlNode item in nodes)
                {
                    Tv myTv = new Tv();

                    myTv.tvName = item["tvName"].InnerText;
                    myTv.tvLogo = item["tvLogo"].InnerText;
                    myTv.tvUrl = item["videoUrl"].InnerText;
                    tvListesi.Add(myTv);
                }
            }

        }
        catch (FileNotFoundException ex)
        {

            Console.WriteLine(ex.Message);  
        }


    }


    void WinPlatfrom()
    {
        StreamReader sr = new StreamReader("MyTvies.xml");
        string okunan = sr.ReadToEnd();
        doc.LoadXml(okunan);

        XmlNodeList nodes = doc.GetElementsByTagName("tv");

        foreach (XmlNode item in nodes)
        {
            Tv myTv = new Tv();

            myTv.tvName = item["tvName"].InnerText;
            myTv.tvLogo = item["tvLogo"].InnerText;
            myTv.tvUrl = item["videoUrl"].InnerText;
            tvListesi.Add(myTv);
        }

    }








    void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        Tv sendTv = args.SelectedItem as Tv;
        Navigation.PushAsync(new VideoPage(sendTv));
    }



}

