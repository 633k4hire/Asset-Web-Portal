using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Helpers;

    public class _SQLerror
    {
        public _SQLerror() { }
        public Exception Ex = null;
        public string ErrorMsg = "generic";
        public object Tag;
    }
    public class SQL_Request
    {
    
        public bool Success = false;
        public object Tag;
        public _SQLerror Error = new _SQLerror();
        public DataSet Data = null;
        public string Message = "default";
        public SqlConnection Connection;
        public SqlCommand Command;
    }

public static class SQLfunc
{
    public static Exception _LastSqlException = null;

    public static string _ConnectionString = @"Data Source=lastsoul.database.windows.net;Initial Catalog=db;Integrated Security=False;User ID=yuriebasuta;Password=Gh0stbust3r;Connect Timeout=60;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"; // @"Data Source=lastsoul.database.windows.net;Initial Catalog=db;Integrated Security=False;User ID=yuriebasuta;Password=********;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    public static SQL_Request OpenConnection(this SQL_Request request, string connectionString = null)
    {

        try
        {
            if (connectionString == null)
                connectionString = _ConnectionString;
            request.Connection = new SqlConnection(connectionString);
            request.Success = true;
            request.Connection.Open();
        }
        catch (Exception ex)
        {
            request.Error.Ex = ex;
            request.Success = false;
        }
        return request;
    }
    public static SQL_Request CloseConnection(this SQL_Request request)
    {

        try
        {
            request.Connection.Close();
            request.Success = true;
        }
        catch (Exception ex)
        {
            request.Error.Ex = ex;
            request.Success = false;
        }
        return request;
    }


    public static SQL_Request AddAsset(this SQL_Request request, Asset asset, bool close = true)
    {
        if (request.Connection.State == ConnectionState.Closed)
        { request.Connection.Open(); }
        request.Command = new SqlCommand();

        try
        {
            string imgs = "";
            foreach (var link in asset.Images)
            {
                imgs += link + ",";
            }
            if (request.Connection.State == ConnectionState.Closed)
            { request.Connection.Open(); }
            request.Command = new SqlCommand();
            request.Command.Connection = request.Connection; //Pass the connection object to Command
            request.Command.CommandType = CommandType.StoredProcedure; // We will use stored procedure.
            request.Command.CommandText = "AssetInsert"; //Stored Procedure Name

            //	 @AssetName,@AssetNumber,@CalibratedAsset,@Damaged,@OnHold,@BarcodeImage,
            //@CalibrationCompany,@CalibrationHistory,@CalibrationPeriod,@DateReturned,
            //@DateShipped,@AssetDescription,@LastCalibrated,@OrderNumber,@PersonShipping,
            //@PackingSlip,@ReturnReport,@UPSlabel,@Images,@ImageLinks,@ServiceEngineer,@ShipTo,@AssetWeight
            request.Command.Parameters.Add("@AssetName", SqlDbType.NVarChar).Value = asset.ItemName;
            request.Command.Parameters.Add("@AssetNumber", SqlDbType.NVarChar).Value = asset.AssetNumber;
            request.Command.Parameters.Add("@CalibratedAsset", SqlDbType.Bit).Value = asset.IsCalibrated;
            request.Command.Parameters.Add("@Damaged", SqlDbType.Bit).Value = asset.IsDamaged;
            request.Command.Parameters.Add("@OnHold", SqlDbType.Bit).Value = asset.OnHold;
            request.Command.Parameters.Add("@BarcodeImage", SqlDbType.NVarChar).Value = asset.BarcodeImage; //FIX
            request.Command.Parameters.Add("@CalibrationCompany", SqlDbType.NVarChar).Value = asset.CalibrationCompany;
            request.Command.Parameters.Add("@CalibrationHistory", SqlDbType.NVarChar).Value = "none";
            request.Command.Parameters.Add("@CalibrationPeriod", SqlDbType.NVarChar).Value = asset.CalibrationPeriod;
            request.Command.Parameters.Add("@DateReturned", SqlDbType.NVarChar).Value = asset.DateRecieved.ToString();
            request.Command.Parameters.Add("@DateShipped", SqlDbType.NVarChar).Value = asset.DateShipped.ToString();
            request.Command.Parameters.Add("@AssetDescription", SqlDbType.NVarChar).Value = asset.Description;
            request.Command.Parameters.Add("@LastCalibrated", SqlDbType.NVarChar).Value = asset.LastCalibrated.ToString();
            request.Command.Parameters.Add("@OrderNumber", SqlDbType.NVarChar).Value = asset.OrderNumber.ToString();
            request.Command.Parameters.Add("@PersonShipping", SqlDbType.NVarChar).Value = asset.PersonShipping;
            request.Command.Parameters.Add("@Images", SqlDbType.NVarChar).Value = imgs;
            request.Command.Parameters.Add("@ImageLinks", SqlDbType.NVarChar).Value = imgs;
            request.Command.Parameters.Add("@ServiceEngineer", SqlDbType.NVarChar).Value = asset.ServiceEngineer;
            request.Command.Parameters.Add("@ShipTo", SqlDbType.NVarChar).Value = asset.ShipTo;
            request.Command.Parameters.Add("@AssetWeight", SqlDbType.NVarChar).Value = asset.weight.ToString();
            request.Command.Parameters.Add("@History", SqlDbType.NVarChar).Value = "none";
            request.Command.Parameters.Add("@PackingSlip", SqlDbType.NVarChar).Value = asset.PackingSlip;
            request.Command.Parameters.Add("@ReturnReport", SqlDbType.NVarChar).Value = asset.ReturnReport;
            request.Command.Parameters.Add("@UPSlabel", SqlDbType.NVarChar).Value = asset.UpsLabel;

            request.Command.ExecuteNonQuery();
            request.Success = true;
            request.Message = "success:addAsset";

        }
        catch (Exception ex)
        {
            request.Error.Ex = ex;
            request.Success = false;
            request.Message = "error:addAsset";
            return request;
        }
        finally
        {
            if (close)
                request.Connection.Close();

        }
        request.Message = "success:addAsset";
        request.Success = true;
        return request;
    }
    public static SQL_Request GetAllData(this SQL_Request request, bool close = true)
    {
        request.Command = new SqlCommand();

        try
        {
            if (request.Connection.State == ConnectionState.Closed)
            { request.Connection.Open(); }
            // Create a object of SqlCommand class
            request.Command.Connection = request.Connection; //Pass the connection object to Command
            request.Command.CommandType = CommandType.StoredProcedure; // We will use stored procedure.

            request.Command.CommandText = "AssetGetAll";
            //request.Command.CommandText = "spGetTimeData"; //Stored Procedure Name
            //request.Command.Parameters.Add("@AssetNumber", SqlDbType.NVarChar).Value = AssetNumber;

            SqlDataAdapter da = new SqlDataAdapter(request.Command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            request.Message = "success:getalldata";
            request.Data = ds;
            request.Success = true;
            List<Asset> newassets = new List<Asset>();
            try
            {
               
                foreach (DataRow dr in request.Data.Tables[0].Rows)
                {
                    Asset a = new Asset();
                    a.ItemName = dr?.Field<string>("AssetName");
                    a.AssetNumber = dr?.Field<string>("AssetNumber");
                    try
                    {
                        a.OrderNumber = Convert.ToInt32(dr?.Field<string>("OrderNumber"));
                    }
                    catch { }
                    a.ShipTo = dr?.Field<string>("ShipTo");

                    a.DateShipped = DateTime.Parse(dr?.Field<string>("DateShipped"));
                    a.ServiceEngineer = dr?.Field<string>("ServiceEngineer");
                    a.PersonShipping = dr?.Field<string>("PersonShipping");
                    a.DateRecieved = DateTime.Parse(dr?.Field<string>("DateReturned"));
                    a.weight = Convert.ToDecimal(dr?.Field<string>("AssetWeight"));
                    a.IsDamaged = dr.Field<bool>("Damaged");
                    a.OnHold = dr.Field<bool>("OnHold");
                    a.Description = dr?.Field<string>("AssetDescription");
                    a.OnHold = dr.Field<bool>("CalibratedAsset");
                    a.CalibrationCompany = dr?.Field<string>("CalibrationCompany");
                    a.LastCalibrated = DateTime.Parse(dr?.Field<string>("LastCalibrated"));
                    a.CalibrationPeriod = dr?.Field<string>("CalibrationPeriod");
                    a.CalibrationHistory = dr?.Field<string>("CalibrationHistory");
                    a.Images = dr?.Field<string>("Images");
                    a.BarcodeImage = dr?.Field<string>("BarcodeImage");
                    //a.History = dr?.Field<string>("");
                    a.PackingSlip = dr?.Field<string>("PackingSlip");
                    a.UpsLabel = dr?.Field<string>("UpsLabel");
                    a.ReturnReport = dr?.Field<string>("ReturnReport");
                    newassets.Add(a);
                }
                request.Tag = newassets;
            }
            catch { }

        }
        catch (Exception ex)
        {
            request.Error.Ex = ex;
            request.Success = false;
            request.Message = "error:getalldata";
            return request;
        }
        finally
        {
            if (close)
                request.Connection.Close();
        }
        return request;
    }

    public static SQL_Request GetData(this SQL_Request request, string AssetNumber, bool close = true)
    {
        request.Command = new SqlCommand();

        try
        {
            if (request.Connection.State == ConnectionState.Closed)
            { request.Connection.Open(); }
            // Create a object of SqlCommand class
            request.Command.Connection = request.Connection; //Pass the connection object to Command
            request.Command.CommandType = CommandType.StoredProcedure; // We will use stored procedure.

            request.Command.CommandText = "AssetGet";
            //request.Command.CommandText = "spGetTimeData"; //Stored Procedure Name
            
            request.Command.Parameters.Add("@AssetNumber", SqlDbType.NVarChar).Value = AssetNumber;

            SqlDataAdapter da = new SqlDataAdapter(request.Command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            //create an asset and tag it onto sql_request
            request.Message = "success:getdata";
            request.Data = ds;
            request.Success = true;
            try
            {
                List<Asset> newassets = new List<Asset>();
                foreach (DataRow dr in request.Data.Tables[0].Rows)
                {
                    Asset a = new Asset();
                    a.ItemName = dr?.Field<string>("AssetName");
                    a.AssetNumber = dr?.Field<string>("AssetNumber");
                    try
                    {
                        a.OrderNumber = Convert.ToInt32(dr?.Field<string>("OrderNumber"));
                    }
                    catch { }
                    a.ShipTo = dr?.Field<string>("ShipTo");

                    a.DateShipped = DateTime.Parse(dr?.Field<string>("DateShipped"));
                    a.ServiceEngineer = dr?.Field<string>("ServiceEngineer");
                    a.PersonShipping = dr?.Field<string>("PersonShipping");
                    a.DateRecieved = DateTime.Parse(dr?.Field<string>("DateReturned"));
                    a.weight = Convert.ToDecimal(dr?.Field<string>("AssetWeight"));
                    a.IsDamaged = dr.Field<bool>("Damaged");
                    a.OnHold = dr.Field<bool>("OnHold");
                    a.Description = dr?.Field<string>("AssetDescription");
                    a.OnHold = dr.Field<bool>("CalibratedAsset");
                    a.CalibrationCompany = dr?.Field<string>("CalibrationCompany");
                    a.LastCalibrated = DateTime.Parse(dr?.Field<string>("LastCalibrated"));
                    a.CalibrationPeriod = dr?.Field<string>("CalibrationPeriod");
                    a.CalibrationHistory = dr?.Field<string>("CalibrationHistory");
                    a.Images = dr?.Field<string>("Images");
                    a.BarcodeImage = dr?.Field<string>("BarcodeImage");
                    //a.History = dr?.Field<string>("");
                    a.PackingSlip = dr?.Field<string>("PackingSlip");
                    a.UpsLabel = dr?.Field<string>("UpsLabel");
                    a.ReturnReport = dr?.Field<string>("ReturnReport");
                    request.Tag = a;
                }
                 
            }
            catch { }


        }
        catch (Exception ex)
        {
            request.Error.Ex = ex;
            request.Success = false;
            request.Message = "error:getdata";
            return request;
        }
        finally
        {
            if (close)
                request.Connection.Close();
        }
        return request;
    }
    public static SQL_Request DeleteData(this SQL_Request request, string AssetNumber, bool close = true)
    {
        request.Command = new SqlCommand();

        try
        {
            if (request.Connection.State == ConnectionState.Closed)
            { request.Connection.Open(); }
            // Create a object of SqlCommand class
            request.Command.Connection = request.Connection; //Pass the connection object to Command
            request.Command.CommandType = CommandType.StoredProcedure; // We will use stored procedure.

            request.Command.CommandText = "AssetDelete";
            request.Command.Parameters.Add("@AssetNumber", SqlDbType.NVarChar).Value = AssetNumber;
            request.Command.ExecuteNonQuery();
            request.Message = "success:deletedata";
           
            request.Success = true;

        }
        catch (Exception ex)
        {
            request.Error.Ex = ex;
            request.Success = false;
            request.Message = "error:deletedata";
            return request;
        }
        finally
        {
            if (close)
                request.Connection.Close();
        }
        return request;
    }
    public static SQL_Request UpdateData(this SQL_Request request, Asset asset, bool close = true)
    {
      
        request.Command = new SqlCommand();
        try
        {
            if (request.Connection.State == ConnectionState.Closed)
            { request.Connection.Open(); }
            // Create a object of SqlCommand class
            request.Command.Connection = request.Connection; //Pass the connection object to Command
            request.Command.CommandType = CommandType.StoredProcedure; // We will use stored procedure.
            request.Command.CommandText = "AssetUpdate";
            string imgs = "";
            foreach (var link in asset.Images)
            {
                imgs += link + ",";
            }
            // request.Command.CommandText = "spUpdateTimeData"; //Stored Procedure Name
            request.Command.Parameters.Add("@AssetName", SqlDbType.NVarChar).Value = asset.ItemName;
            request.Command.Parameters.Add("@AssetNumber", SqlDbType.NVarChar).Value = asset.AssetNumber;
            request.Command.Parameters.Add("@CalibratedAsset", SqlDbType.Bit).Value = asset.IsCalibrated;
            request.Command.Parameters.Add("@Damaged", SqlDbType.Bit).Value = asset.IsDamaged;
            request.Command.Parameters.Add("@OnHold", SqlDbType.Bit).Value = asset.OnHold;
            request.Command.Parameters.Add("@BarcodeImage", SqlDbType.NVarChar).Value = asset.BarcodeImage; //FIX
            request.Command.Parameters.Add("@CalibrationCompany", SqlDbType.NVarChar).Value = asset.CalibrationCompany;
            request.Command.Parameters.Add("@CalibrationHistory", SqlDbType.NVarChar).Value = "none";
            request.Command.Parameters.Add("@CalibrationPeriod", SqlDbType.NVarChar).Value = asset.CalibrationPeriod;
            request.Command.Parameters.Add("@DateReturned", SqlDbType.NVarChar).Value = asset.DateRecieved.ToString();
            request.Command.Parameters.Add("@DateShipped", SqlDbType.NVarChar).Value = asset.DateShipped.ToString();
            request.Command.Parameters.Add("@AssetDescription", SqlDbType.NVarChar).Value = asset.Description;
            request.Command.Parameters.Add("@LastCalibrated", SqlDbType.NVarChar).Value = asset.LastCalibrated.ToString();
            request.Command.Parameters.Add("@OrderNumber", SqlDbType.NVarChar).Value = asset.OrderNumber.ToString();
            request.Command.Parameters.Add("@PersonShipping", SqlDbType.NVarChar).Value = asset.PersonShipping;
            request.Command.Parameters.Add("@Images", SqlDbType.NVarChar).Value = imgs;
            request.Command.Parameters.Add("@ImageLinks", SqlDbType.NVarChar).Value = imgs;
            request.Command.Parameters.Add("@ServiceEngineer", SqlDbType.NVarChar).Value = asset.ServiceEngineer;
            request.Command.Parameters.Add("@ShipTo", SqlDbType.NVarChar).Value = asset.ShipTo;
            request.Command.Parameters.Add("@AssetWeight", SqlDbType.NVarChar).Value = asset.weight.ToString();
            request.Command.Parameters.Add("@History", SqlDbType.NVarChar).Value = "none";
            request.Command.Parameters.Add("@PackingSlip", SqlDbType.NVarChar).Value = asset.PackingSlip;
            request.Command.Parameters.Add("@ReturnReport", SqlDbType.NVarChar).Value = asset.ReturnReport;
            request.Command.Parameters.Add("@UPSlabel", SqlDbType.NVarChar).Value = asset.UpsLabel;


            request.Command.ExecuteNonQuery();
            request.Message = "success:assetUpdate";
        }
        catch (Exception ex)
        {
            request.Error.Ex = ex;
            request.Success = false;
            request.Message = "error:assetUpdate";
            return request;
        }
        finally
        {
            if (close)
                request.Connection.Close();
        }
        return request;
    }
}