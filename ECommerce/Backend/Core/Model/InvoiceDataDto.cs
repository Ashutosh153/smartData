using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class InvoiceDataDto
    {
       public string BillId {  get; set; }
        public string Country {  get; set; }
        public string State {  get; set; }
        public int Total {  get; set; }

    public string Date { get ; set; }   
    public int ProductId {  get; set; }
        public string ProductName {  get; set; }
  public int quentity {  get; set; }
  public int price {  get; set; }
 public string FirstName {  get; set; }
  public string LastName {  get; set; }

    }
}
