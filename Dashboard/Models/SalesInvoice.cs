using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;


namespace Dashboard.Models
{
    public class SalesInvoice
    {
        public ObjectId _id { get; set; }
        public Int32 INVOICEID;
        public string INVOICENUMBER;
        public Int32 ORDERID;
        public Int32 EMPLOYEEID;
        public Int32 USERID;
        public Nullable<DateTime> CREATEDDATE;
        public Nullable<DateTime> UPDATEDDATE;
        public string DELIVERYTYPE;
        public double OUTSTANDINGAMOUNT;
        public double TOTALPAYABLEAMOUNT;
        public Nullable<DateTime> INVOICEDUEDATE;
        public Boolean ISORDERINVOICE;
        public Boolean ISPAID;
        public string PAYMENTSTATUS;
        public Boolean ISPOSTED;
        public Boolean ISDISPATCH;
        public string CUSTOMERNOTES;
        public Int32 TOTALBOXES;
        public Nullable<DateTime> INVOICEDATETIME;
        public Boolean IsCashAndCarry;
        public Boolean ISNONSALESINVOICE;
        public Nullable<Int32> SHIPPINGOPTIONID;
        public Nullable<double> SHIPPINGCHARGEDAMT;
        [DataType(DataType.Date)]
        public DateTime INVOICEDATE;
        
        public decimal INVOICEAMOUNT;
        public Int32 COMPANYID;
        public Int32 CUSTOMERID;
        
        public SalesInvoice()
        {
        }

        public SalesInvoice(SalesInvoice s){
            this.INVOICEAMOUNT = s.INVOICEAMOUNT;
           // this.INVOICEDATE = s.INVOICEDATE;
                //s.INVOICEDATE.Value.Date;
            this.COMPANYID = s.COMPANYID;
            this.CUSTOMERID = s.CUSTOMERID;
            this.INVOICEDATE = s.INVOICEDATE.Date;
            
            this._id = s._id;
        }
    }

    public class PayTransaction
    {
        public ObjectId _id { get; set; }
        public int  PAYMENTTRANSACTIONID;
	    public Nullable<int> REFERENCEID;
        public string REFERENCETYPE;
	    public Nullable<int> CUSTOMERID;
	    public Nullable<int> VENDORID;
	    public decimal AMOUNT;
	    public Nullable<DateTime> PAYMENTDATE;
	    public Nullable<int> COMPANYID;
	    public Nullable<int> EMPLOYEEID;
	    public Nullable<DateTime> CREATEDDATE;
	    public int INVOICEID;
	    public string  INVOICETYPE;
	    public Nullable<int> CUSTOMERGROUPID;
	    public decimal DISCOUNT;

        public PayTransaction() { }

        public PayTransaction(PayTransaction p)
        {
            this.INVOICEID = p.INVOICEID;
            this.AMOUNT = p.AMOUNT;
            this.REFERENCEID = p.REFERENCEID;
            this.CUSTOMERID = p.CUSTOMERID;
        }
    }
}