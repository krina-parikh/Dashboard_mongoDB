using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MongoDB.Driver;
using Dashboard.Models;

namespace Dashboard.Helpers
{
    public class ConnectDB
    {
        public MongoCollection<Company> collectionCmpy;
        public MongoCollection<SalesInvoice> collectionSI;
        public MongoCollection<PayTransaction> payment;
        public MongoCollection<InvoiceDetails> collectionDI;
        public MongoCollection<Item> collectionItem;
        public MongoCollection<Category> collectionCtgy;
        public MongoCollection<Customer> customer;

        public ConnectDB()
        {
            /*System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = new System.Diagnostics.ProcessStartInfo("C:/triveni_krina/init.bat");
            p.Start();*/
            var connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("wms");

            collectionCmpy = database.GetCollection<Company>("TBLMCOMPANY");
            collectionSI = database.GetCollection<SalesInvoice>("TBLTSALESINVOICE");
            payment = database.GetCollection<PayTransaction>("TBLDPAYMENTTRANSACTION");
            collectionDI = database.GetCollection<InvoiceDetails>("TBLTSALESINVOICEDETAIL");
            collectionItem = database.GetCollection<Item>("TBLTITEM");
            collectionCtgy = database.GetCollection<Category>("TBLMITEMCATEGORY");
            customer = database.GetCollection<Customer>("TBLTCUSTOMER");
         
        }
    }
}