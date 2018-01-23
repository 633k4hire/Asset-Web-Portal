using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Helpers
{    
    public static class Extensions
    {

        public static string Sanitize(this string str)
        {
            if (str == null)
                str = "";
            return str.Replace("\"", "").Replace("'", "");
        }
        public static string SanitizeHTML(this string str)
        {
            if (str == null)
                str = "";
            return str.Replace("<", "").Replace(">", "").Replace("*", "").Replace(":", "").Replace("|", "");
        }
        public static Asset FindAssetByNumber(this List<Asset> assets, string assetNumber)
        {
            return (from x in assets
                    where x.AssetNumber ==assetNumber
                    select x).FirstOrDefault();
        }
        public static Asset FindAssetByName(this List<Asset> assets, string assetName)
        {
            return (from x in assets
                    where x.ItemName == assetName
                    select x).FirstOrDefault();
        }
        public static List<Asset> FindAssetsByNumber(this List<Asset> assets, string assetNumber)
        {
            return (from x in assets
                    where x.AssetNumber == assetNumber
                    select x).ToList();
        }
    }

    [Serializable]
    public class NotificationListItem
    {
        public string Notification { get; set; }
        public NotificationListItem()
        {
            Notification = "";
        }
    }

    [Serializable]
    public struct Email
    {
        public EmailAddress Address;
        public string Subject;
        public string Body;
        public object Attachments;
        public DateTime Time;
    }
    [Serializable]
    public struct EmailAddress
    {
        public static EmailAddress Create(string email)
        {

            EmailAddress e = new EmailAddress();
            e.Email = email;
            if (!e.Email.Contains("@"))
            {
                e.Email = "servicetools.starrag@gmail.com";
            }
            try
            {
                if (email.Contains("@"))
                {
                    var temp = email.Split('@');
                    e.Name = temp[0];
                    if (temp[0].Contains("."))
                    {
                        e.Name = temp[0].Replace(".", " ");
                    }
                }
            }
            catch
            {
                e.Name = e.Email;
            }

            return e;
        }
        public string Name;
        public string Email;
    }
    [Serializable]
    public class Customer
    {
        public Customer()
        {
        }
        public static Customer Create(string coName, string attn, string address, string phone, string fax, EmailAddress email)
        {
            Customer c = new Customer();
            c.CompanyName = coName;
            c.Attn = attn;
            c.Address = address;
            c.Phone = phone;
            c.Fax = fax;
            c.Email = email;
            return c;
        }
        public string CompanyName = "";
        public string Attn = "";
        public string Address = "";
        public string Address2 = "";
        public string Address3 = "";
        public string City = "";
        public string State = "";
        public string NickName;
        public string ResInd;
        public string LocID;
        public string ConsInd;
        public string AccountNumber;
        public string AccPostalCd;
        public string USPSPOBoxIND;
        public string Postal = "";
        public string Country = "";
        public string PackageWeight = "";
        public string Phone = "";
        public string Fax = "";
        public EmailAddress Email = new EmailAddress();
        public List<string> OrderNumbers = new List<string>();
        public string OrderNumber = "";
        public List<string> CurrentAssignedAssets = new List<string>();
        public object Tag = null;
    }
    [Serializable]
    public class Engineer
    {
        public Engineer()
        { }
        public static Engineer Create(string name, string address, string phone, EmailAddress email)
        {
            Engineer e = new Engineer();
            e.Name = name;
            e.Address = address;
            e.Phone = phone;
            e.Email = email;
            return e;
        }
        public string Name = "";
        public string Address = "";
        public string Phone = "";
        public EmailAddress Email = new EmailAddress();
        public object Tag = null;
    }
    [Serializable]
    public class NotificationLibrary
    {
        public void Write(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, this);
                }
            }
            catch { }
        }
        public static NotificationLibrary Read(string path)
        {
            NotificationLibrary obj = new NotificationLibrary();
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                obj = (NotificationLibrary)formatter.Deserialize(stream);
                stream.Close();
            }
            catch { }
            return obj;
        }
        public NotificationLibrary()
        {

        }
        public List<AssetNotification> ThrityDayNotifications = new List<AssetNotification>();
        public List<AssetNotification> FifteenDayNotifications = new List<AssetNotification>();
    }
    [Serializable]
    public class Backup
    {
        public void Write(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, this);
                }
            }
            catch { }
        }
        public static Backup Read(string path)
        {
            Backup obj = new Backup();
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                obj = (Backup)formatter.Deserialize(stream);
                stream.Close();
            }
            catch { }
            return obj;
        }
        public Backup()
        {
        }
        public DateTime LastBackup;
        public object Tag;
        public object Tag2;
    }


    [Serializable]
    public struct AssetNotification
    {
        public static AssetNotification Create(string assetNumber, List<EmailAddress> emails, bool isNotified = false)
        {
            AssetNotification n = new AssetNotification();
            n.Emails = new List<EmailAddress>();
            n.Emails.AddRange(emails);
            n.AssetNumber = assetNumber;
            n.Time = DateTime.Now;
            n.LastNotified = DateTime.Now;
            n.IsNotified = isNotified;
            return n;
        }
        public List<EmailAddress> Emails;
        public string AssetNumber;
        public DateTime Time;
        public DateTime LastNotified;
        public bool IsNotified;
        public object Tag;

    }
    [Serializable]
    public class Settings
    {
        public static Settings GetInstance()
        {
            return new Settings();
        }
        public Settings()
        {

        }
        public void Write(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, this);
                }
            }
            catch { }
        }
        public static Settings Read(string path)
        {
            Settings obj = new Settings();
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                obj = (Settings)formatter.Deserialize(stream);
                stream.Close();
            }
            catch { }
            return obj;
        }
        public string LibraryPath;
        public string AssetsPath;
        public List<AssetNotification> Notifications_30_Day = new List<AssetNotification>();
        public List<AssetNotification> Notifications_15_Day = new List<AssetNotification>();
        public List<Email> NotSentEmails = new List<Email>();
        public List<Email> d = new List<Email>();
        public List<EmailAddress> ServiceEngineers = new List<EmailAddress>();
        public List<Customer> Customers;
        public List<EmailAddress> StaticEmails;
        public List<string> ShippingPersons;

    }
    [Serializable]
    public class Asset : Serializers.MemoryBinarySerializer
    {

        public void Write(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, this);
                }
            }
            catch { }
        }
        public static Asset Read(string path)
        {
            Asset obj = new Asset();
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                obj = (Asset)formatter.Deserialize(stream);
                stream.Close();
            }
            catch { }
            return obj;
        }
        public static Asset Create(string assetName = "", string assetNumber = "")
        {
            
            Asset ass = new Asset().Init();            
            ass.ItemName = assetName;
            ass.AssetNumber = assetNumber;
            return ass;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public Asset()
        {
            Init();
        }
        private Asset Init()
        {
            Images = "";
            IsOut = false;
            weight = 0;
            History = new List<Asset>();
            AssetNumber = "0000";
            OrderNumber = 0;
            ItemName = PackingSlip =UpsLabel = ReturnReport  = ShipTo = ServiceEngineer= PersonShipping=  Barcode = Description= BarcodeImage= CalibrationCompany= CalibrationPeriod=   "";
            IsOut = IsDamaged = OnHold = false;
            return this;
        }
        public string FirstImage{ get
            {
                var img = Images.Replace(",,,", "<@#$>").Replace(",", "").Replace("<@#$>", ",").Replace("Images", "").Replace("\\", "").Split(',')[0];
                img = "/Account/Images/" + img;
                return img;
            }
        }
        public string Images { get; set; }
        public string ItemName { get; set; }
        public int OrderNumber { get; set; }
        public string ShipTo { get; set; }
        public DateTime DateShipped { get; set; }
        public DateTime LastCalibrated { get; set; }
        public DateTime DateRecieved { get; set; }
        public string DateRecievedString { get { return DateRecieved.ToString(); } }
        public string LastCalibratedString { get { return LastCalibrated.ToString(); } }
        public string DateShippedString { get { return DateShipped.ToString(); } }

        public string ServiceEngineer { get; set; }
        public string PersonShipping { get; set; }
        public string Barcode { get; set; }
        public string AssetNumber { get; set; }// "0000";
        public string Description { get; set; }
        public string BarcodeImage { get; set; }
        public bool IsOut { get; set; }
        public bool IsDamaged { get; set; }
        public bool IsCalibrated { get; set; }
        public string CalibrationCompany { get; set; }
        public string CalibrationPeriod { get; set; }
        public object CalibrationHistory { get; set; }
        public bool OnHold { get; set; }
        public decimal weight { get; set; }
        public string PackingSlip { get; set; }
        public string UpsLabel { get; set; }
        public string ReturnReport { get; set; }

        public List<Asset> History { get; set; }
    }
    [Serializable]
    public class AssetLibrary
    {
        public void Write(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, this);
                }
            }
            catch { }
        }
        public static AssetLibrary Read(string path)
        {
            AssetLibrary obj = new AssetLibrary();
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                obj = (AssetLibrary)formatter.Deserialize(stream);
                stream.Close();
            }
            catch { }
            return obj;
        }

        public List<Asset> Assets = new List<Asset>();
        public List<ListViewItem> InStock = new List<ListViewItem>();
        public List<ListViewItem> OutStock = new List<ListViewItem>();
        public string Name;
        public string Filename;
        public object Tag;
        public AssetLibrary()
        {

        }
    }
}