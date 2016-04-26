using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Dashboard.Models
{
    public class Company
    {
      public ObjectId _id { get; set; }
      public  int COMPANYID;
      public string COMPANYCODE;
      public string COMPANYNAME;
      public string CONTACTPERSONFIRSTNAME;
      public string CONTACTPERSONLASTNAME;
      public string SSNNO;
      public string COMPANYEMAILID;
      public string COMPANYSTATUS;
      public string WEBSITE;
      public int COMPANYADDRESSID;
      public int USERID;
      public int EMPLOYEEID;
      public Nullable<DateTime> CREATEDDATE;
      public Nullable<DateTime> UPDATEDDATE;
      public Boolean ISDELETED;

        public Company() { 
        }

        public Company(Company c)
        {
            this.COMPANYID = c.COMPANYID;
            this.COMPANYNAME = c.COMPANYNAME;

        }
    }

    public class Customer
    {
        public ObjectId _id { get; set; }
        public int CUSTOMERID;
	    public string CUSTOMERCODE;
	    public string CUSTOMERNAME;
	    public string CONTACTPERSONFIRSTNAME;
	    public string CONTACTPERSONLASTNAME;
	    public int CUSTOMERGROUPID;
	    public int CUSTOMERTYPEID;
	    public string ISTAXAPPLIED;
	    public int PRIMARYADDRESSID;
	    public int SECONDARYADDRESSID;
	    public int PAYMENTTERMID;
    	public string CUSTOMERSTATUS;
    	public string REASONFORINACTIVE;
    	public string CUSTOMEREMAILID;
    	public string SOCIALSECURITYNUMBER;
    	public string SALESMANDAY;
    	public int INVOICEDUEDAYS;
	    public decimal CREDITLIMIT;
    	public int PRICINGMODELID;
    	public string CIGARLICENCENUMBER;
    	public string CIGARATTELICENCENUMBER;
    	public string FEDERALLICENCENUMBER;
    	public Nullable<DateTime> FEDERALEXPIRYDATE;
    	public string CUSTOMERBARCODE;
    	public Nullable<DateTime> CREATEDDATE;
    	public Nullable<DateTime> UPDATEDDATE;
    	public int EMPLOYEEID;
	    public int COMPANYID;
    	public bool ISDELETED;
    	public string FEDERALEMPLOYERID;
    	public string NOTES;
    	public decimal CUSTOMERBALANCE;
    	public string ACCOUNTNO;
    	public int SALESMANID;
    	public string PRODUCTPROMOTIONID;
    	public bool ISSALESTAXAPPLICABLE;
    	public Nullable<DateTime> EXPIRYDATE;
	    public string PRICINGMODELNAME;
    	public bool CALCULATEINTEREST;
    	public bool ISSHOWDISPATCHNOTES;
    	public bool  SENDINVOICEEMAIL;
    	public bool IsOnlineCustomer;
    	public Nullable<decimal> SHIPPINGCHARGE;
    	public Nullable<int> SHIPPINGOPTIONID;
    	public bool ISFIXED;
    	public bool IsSendInvoiceEmailUponPosting;
    	public string DBA;
    
        public Customer() { }

        public Customer(Customer x)
        {
            this.CUSTOMERID = x.CUSTOMERID;
            this.CUSTOMERNAME = x.CUSTOMERNAME;
        }
    }
}