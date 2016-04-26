using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Dashboard.Models
{

    public class TopItem
    {
        public int ITEMID;
        [DataType(DataType.Currency)]
        public decimal ACTUALPRICE;
        public string ITEMNAME;
        public int ITEMCATEGORYID;
        public string CATEGORYNAME;
        public int ORDERQUANTITY;
        public int COMPANYID;
        public string COMPANYNAME;
        

        public TopItem() { }

        public TopItem(TopItem t)
        {
            this.ITEMNAME = t.ITEMNAME;
            this.ITEMID = t.ITEMID;
            this.COMPANYID = t.COMPANYID;
            
            this.ORDERQUANTITY = t.ORDERQUANTITY;
            this.CATEGORYNAME = t.CATEGORYNAME;
        }
    }

    public class Item
    {
        public string ITEMBARCODE;
	    public Nullable<int> VENDORID;
        public string BOOKLETCATEGORY;
	    public Nullable<int> DEALID;
	    public Nullable<int> ITEMQUANTITY;
	    public Nullable<decimal> WEIGHT;
        public string UNITSOFMEASUREMENT;
        public string UNITSDESCRIPTION;
        public string TAXABLEBY;
        public string ISMANAGEABLE;
	    public Nullable<int> PROMOTIONID;
	    public Nullable<int> QUANTITYONHAND;
	    public Nullable<int> QUANTITYONORDER;
        public string UPCNO;
        public string UPCDESCRIPTION;
        public string INNERPACKUPC;
        public string BOXUPC;
        public string IMAGEPATH;
	    public Nullable<int> JURISDICTIONID;
	    public Nullable<decimal> SELLINGPRICE;
	    public bool ISSAMEPRICEAPPLICABLE;
	    public Nullable<DateTime> CREATEDDATE;
	    public Nullable<DateTime> UPDATEDDATE;
	    public Nullable<int> EMPLOYEEID;
	    public bool ISDELETED;
	    public string BINNUMBER;
	    public string ITEMCODE;
	    public string SECONDARYITEMNUMBER;
	    public bool ISTAXALWAYSAPPLICABLE;
	    public string UNITOFWEIGHT;
	    public Nullable<int> PCSPERUNIT;
	    public Nullable<decimal> COSTPRICE;
	    public bool ISCARTONMANAGABLE;
	    public string IDENTIFICATIONSYMBOL;
	    public string DISTRIBUTORSKU;
	    public string PRODUCTDESCRIPTION;
	    public string PROMOTIONDESCRIPTION;
	    public Nullable<int> ITEMSPERSELLINGUNIT;
	    public bool PROMOTIONINDICATOR;
	    public string DISTRIBUTORCATEGORYCODE;
	    public string MSACATEGORYCODE;
	    public string PROJECTIDFORFUTUREUSE;
	    public string DISTRIBUTORPRODUCTUNITSIZE;
	    public string PRODUCTUNITSIZEDESCRIPTION;
	    public string COMPONENTSHIPPERFLAG;
	    public string MANUFACTURERPROMOTIONCODE;
	    public string MANUFACTURERPRODUCTIDCODE;
	    public string UPCEXTENSION;
	    public string UPCYEAR;
	    public string PROVINCIALMARKINGCODE;
	    public bool ISMSAMANAGEABLE;
	    public Nullable<int> DEPARTMENTID;
	    public bool ISALLSECTIONWISEPRICE;
	    public Nullable<DateTime> MANUFACTUREDDATE;
	    public Nullable<DateTime> EXPIRATIONDATE;
	    public Nullable<int> DAYSGOODBEFOREUSE;
	    public bool ISACTIVE;
	    public Nullable<int> SECONDARYITEMCATEGORYID;
	    public Nullable<int> DEFAULTORDERQUANTITY;
	    public Nullable<int> REORDERLEVELQUANTITY;
	    public Nullable<decimal> MINIMUMORDERQUANTITY;
	    public Nullable<int> LOTSIZE;
	    public string VENDORITEMNO;
	    public string MANUFACTURERITEMNO;
	    public bool IsDeal;
        public ObjectId _id { get; set; }
        public int ITEMID;
        public string ITEMNAME;
        public Nullable<int> ITEMCATEGORYID; 
        public int COMPANYID;
        
        public Item(){  }

        public Item(Item i)
        {
            this.ITEMID = i.ITEMID;
            this.ITEMNAME = i.ITEMNAME;
            this.ITEMCATEGORYID = i.ITEMCATEGORYID;
            
            this.COMPANYID = i.COMPANYID;
        }
    }

    public class Category
    {
        public string DESCRIPTION;
	    public Nullable<int> EMPLOYEEID;
	    public Nullable<int> COMPANYID;
	    public Nullable<DateTime> CREATEDDATE;
	    public Nullable<DateTime> UPDATEDDATE;
	    public Nullable<bool> ISDELETED;
	    public Nullable<bool> ISCIGARETTE;
	    public Nullable<bool> ISSECONDARY;
	    public Nullable<int> CATEGORYTYPE;
	    public Nullable<bool> ISNONSALE;
        public ObjectId _id { get; set; }
        public int ITEMCATEGORYID;
        public string CATEGORYNAME;

        public Category()
        {
        }

        public Category(Category c)
        {
            this.CATEGORYNAME = c.CATEGORYNAME;
            this.ITEMCATEGORYID = c.ITEMCATEGORYID;
        }
    }

    public class InvoiceDetails
    {
        public ObjectId _id { get; set; }
        public int INVOICEITEMID;
        public int INVOICEQUANTITY;
        public decimal DISCOUNT;
      public decimal TAXAMOUNT;
      public decimal CALCULATEDAMOUNT;
      public decimal INPUTAMOUNT;
      public bool ISRETURNED;
      public bool ISDAMAGED;
      public decimal salestaxamount;
      public decimal excisetaxamount;
      public Nullable<int> PROMOTIONID;
      public Nullable<bool> ISPROMOTIONITEM;
      public Nullable<decimal> EXCISERATE;
      public Nullable<decimal> SECTIONBASEPRICE;
      public Nullable<decimal> SALESRATE;
      public Nullable<decimal> COSTPRICE;
      public Nullable<decimal> SHIPPINGCHARGEDAMT;
      public Nullable<bool> ISSHIPPINGCHARGED;
      public int INVOICEID;
        public int ITEMID;
        public int ORDERQUANTITY;
        [DataType(DataType.Currency)]
        public decimal ACTUALPRICE;

        public InvoiceDetails() { }

        public InvoiceDetails(InvoiceDetails id)
        {
            this.ACTUALPRICE = id.ACTUALPRICE;
            this.INVOICEID = id.INVOICEID;
            this.ITEMID = id.ITEMID;
            this.ORDERQUANTITY = id.ORDERQUANTITY;
            
        }
    }

}