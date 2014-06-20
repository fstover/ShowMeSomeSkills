using System;
using System.Linq;
using System.Collections.Generic;
using InterviewTest1.Models;
namespace InterviewTest1
{
    internal class Program
    {
        private static void Main(string[] args)
            // Author:  Fred Stover
            //Date:      6/20/2014
            //Pulls the data and passes it to the 3 subroutines that builds and displays the data
        {
            decimal invsub = 0;
            List<Invoice> data = new Repo().All().ToList();
            {
                /*Example 1 calculates the subtotal and total for the invoice items
                   And displays the line item information on the Console */

                Console.WriteLine(" EXAMPLE 1");
                invsub = GetSubTotal_InvoiceItems(data);

                /*Example 2 calculates the sub total and total for the invoice
                  and displays the line items and the invoice information on the Console  */

                Console.WriteLine(" EXAMPLE 2");
                invsub = GetTotal_InvoiceItems(data);

                /*Example 3 provides the same functionality as example 2 but adds a 
                 * commision amount calcualtion and then displays the commssion amount 
                 * along with the line item and invoice information*/

                Console.WriteLine(" EXAMPLE 3");
                invsub = GetComm_InvoiceItems(data);
            }
         // Keeps the display on the screen until a key is hit
          Console.ReadLine();
        }

        private static decimal GetSubTotal_InvoiceItems(List<Invoice> data)
        {
            decimal subamt = 0;
            decimal subamttot = 0;
            decimal holdtot = 0;
            string holdinvnb;
            decimal holdtax = 0;
            decimal holddiscpcnt = 0;
            foreach (Invoice inv in data)
            {
                //Retrieve Invoice information from data

                holdinvnb = inv.InvoiceNo;
                Console.WriteLine("INVOICE NUMBER: " + holdinvnb);

                    //Retrieves line item information from for the Invoice

                    foreach (InvoiceItem itm in inv.LineItems)

                    {
                        //Start of caclulation code

                        subamt = itm.UnitPrice * itm.Quantity;
                        subamttot += subamt;
                        holddiscpcnt = itm.Discount;
                        holdtot = subamt - (subamt * (holddiscpcnt / 100));
                        if (itm.Taxable)
                        {
                            holdtax += holdtot * (inv.TaxRate / 100);
                            holdtot += holdtax;
                        }
 
                        //Start of code to display item information on the Console

                        Console.Write("Desc: " + itm.LineText);
                        Console.Write(", Taxable: " + itm.Taxable);
                        Console.Write(", Quantity: " + itm.Quantity);
                        Console.Write(", Unit Price: " + itm.UnitPrice);
                        Console.Write(", Discount: " + itm.Discount);
                        Console.Write(", SubTotal: " + subamt.ToString("C"));
                        Console.Write(", TOTAL: " + holdtot.ToString("C"));
                        Console.WriteLine();
                        Console.WriteLine();

                        //Clear reusable fields

                        holdtax = 0;
                        subamttot = 0;

                    }

            }
            
            return holdtot;
        }

        private static decimal GetTotal_InvoiceItems(List<Invoice> data)
        {
            decimal subamt = 0;
            decimal subamttot = 0;
            decimal tottot = 0;
            decimal holdtot = 0;
            string holdinvnb;
            decimal holddisc = 0;
            decimal holdtax = 0;
            decimal holddiscpcnt;


            //Retrieve Invoice information from data

            foreach (Invoice inv in data)
            {
                holdinvnb = inv.InvoiceNo;
                Console.WriteLine("INVOICE NUMBER: " + holdinvnb);

                    //Retrieves line item information from for the Invoice

                    foreach (InvoiceItem itm in inv.LineItems)
                    {

                        //Start of caclulation code

                        subamt = itm.UnitPrice * itm.Quantity;
                        subamttot += subamt;
                        holddiscpcnt = itm.Discount;
                        holddisc += subamt * (holddiscpcnt / 100);
                        holdtot = subamt - holddisc;
                        if (itm.Taxable)
                        {
                            holdtax += subamt * (inv.TaxRate / 100);
                            holdtot += holdtax;
                        }
                        tottot += holdtot;

                        //Start of code to display item information on the Console

                        Console.Write("Desc: " + itm.LineText);
                        Console.Write(", Taxable: " + itm.Taxable);
                        Console.Write(", Quantity: " + itm.Quantity);
                        Console.Write(", Unit Price: " + itm.UnitPrice);
                        Console.Write(", Discount: " + itm.Discount);
                        Console.Write(", SubTotal: " + subamt.ToString("C"));
                        Console.Write(", TOTAL: " +  holdtot.ToString("C"));
                        Console.WriteLine();

                        //Clear reusable fields

                        holdtax = 0;
                        holddisc = 0;
                    }

                    //Start of code to display the invoice information on the console

                Console.Write("INVOICE NUMBER: " + holdinvnb);
                Console.Write(" Company: " + inv.CompanyName);
                Console.Write(", Sub Total: " + tottot.ToString("C"));
                Console.Write(", TOTAL: " + (inv.Shipping + tottot).ToString("C"));
                Console.WriteLine();
                Console.WriteLine();

                //Clear reusable fields

                subamttot = 0;
                tottot = 0;
            }
            return holdtot;
        }


        private static decimal GetComm_InvoiceItems(List<Invoice> data)
        {

            decimal subamt = 0;
            decimal subamttot = 0;
            decimal tottot = 0;
            decimal holdtot = 0;
            string holdinvnb;
            decimal holddisc = 0;
            decimal holdtax = 0;
            decimal holddiscpcnt;
            decimal holdcomamt;
            decimal calccomm;
            int calcamt;
            decimal resultamt;;

            //Retrieve Invoice information from data

            foreach (Invoice inv in data)
            {
                holdinvnb = inv.InvoiceNo;
                Console.WriteLine("INVOICE NUMBER: " + holdinvnb);

                     //Retrieves line item information from for the Invoice

                    foreach (InvoiceItem itm in inv.LineItems)
                    {
                        //Start of caclulation code

                        subamt = itm.UnitPrice * itm.Quantity;
                        subamttot += subamt;
                        holddiscpcnt = itm.Discount;
                        holddisc += subamt * ( holddiscpcnt / 100);
                        holdtot = subamt - holddisc;
                        if (itm.Taxable)
                        {
                            holdtax += subamt * (inv.TaxRate / 100);
                            holdtot += holdtax;
                        }
                        tottot += holdtot;

                        /*Start of code to calcualte the commission and round
                          down to the neares .25 */
                        holdcomamt = (subamt - holddisc);
                        calccomm = holdcomamt * .03M;
                        calcamt = (int)calccomm;
                        resultamt = calccomm- calcamt;
                        if (resultamt <= .25M)
                        {
                            resultamt = calcamt;
                        }
                            else if (resultamt > .25M & (resultamt   <= .75M))
                            {
                                resultamt = calcamt + .50M;
                            }
                                else if (resultamt > .75M)
                                {
                                    resultamt = calcamt + 1;
                                }
                        //End of commission calculation

                        //Start of code to display item information on the Console

                        Console.Write("Desc: " + itm.LineText);
                        Console.Write(", Taxable: " + itm.Taxable);
                        Console.Write(", Quantity: " + itm.Quantity);
                        Console.Write(", Unit Price: " + itm.UnitPrice);
                        Console.Write(", Discount: " + itm.Discount);
                        Console.Write(", SubTotal: " + subamt.ToString("C"));
                        Console.Write(", Commission: " + resultamt.ToString("C"));
                        Console.Write(", TOTAL: " + holdtot.ToString("C"));
                        Console.WriteLine();

                        //Clear reusable fields

                        holdtax = 0;
                        holddisc = 0;
                    }

                    //Start of code to display the invoice information on the console

                Console.Write("INVOICE NUMBER: " + holdinvnb);
                Console.Write(" Company: " + inv.CompanyName);
                Console.Write(", Sub Total: " + tottot.ToString("C"));
                Console.Write(", TOTAL: " + (inv.Shipping + tottot).ToString("C"));
                Console.WriteLine();
                Console.WriteLine();

                //Clear reusable fields

                subamttot = 0;
                tottot = 0;
            }
            return holdtot;
        }
    }
}

/****************** The following is the SQL Code to create the 2 tables  ******************

Use DB_Name_
Go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].Invoice_Backup') AND type in (N'U'))
DROP TABLE [dbo].Invoice_Backup
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].Invoice_Backup(
	[InvoiceNo] [varchar] (50) NULL,
	[CompanyName] [varchar](50) NULL,
	[BillingContact] [varchar](50) NULL,
	[BillingStreet] [varchar](150) NULL,
	[BillingCity] [varchar](150) NULL,
	[BillingState] [varchar](10) NULL,
	[BillingZip] [varchar] (10)NULL,
	[PostedDate] [datetime] NULL,
	[ShippingDate] [datetime] NULL,
	[RequisitionDate] [datetime] NULL,
	[LineItems] [varchar] (MAX) null,
	[TaxRate] [decimal](18,2) NULL,
	[SubTotal] [decimal](18,2) NULL,
	[Shippint] [decimal](18,2) NULL,
	[Total] [decimal] (18,2) NULL
) ON [PRIMARY]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].Invoice_Item_Backup') AND type in (N'U'))
DROP TABLE [dbo].Invoice_Item_Backup
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].Invoice_Item_Backup(
	[LineTest] [varchar] (50) NULL,
	[Taxable] [bit] NULL,
	[Quantity] [Int] NULL,
	[UnitPrice] [decimal](18,2) NULL,
	[Discount] [tinyint] NULL,
	[SubTotal] [decimal](18,2) NULL,
	[Total] [decimal] (18,2) NULL
) ON [PRIMARY]
GO

*/
