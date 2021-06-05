﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemstonesBusinessManagementSystem.Models
{
    class Bill
    {
        private int idBill;
        private int idAccount;
        private DateTime billDate;
        private int status;
        private long totalPrice;
        private int idCustomer;

        public int IdBill { get => idBill; set => idBill = value; }
        public int IdAccount { get => idAccount; set => idAccount = value; }
        public DateTime BillDate { get => billDate; set => billDate = value; }
        public long TotalPrice { get => totalPrice; set => totalPrice = value; }
        public int IdCustomer { get => idCustomer; set => idCustomer = value; }
        public int Status1 { get => status; set => status = value; }

        public Bill()
        {

        }
        public Bill(int idBill, int idAccount, DateTime billDate, int status, long totalPrice, int idCustomer)
        {
            this.idBill = idBill;
            this.idAccount = idAccount;
            this.billDate = billDate;
            this.status = status;
            this.totalPrice = totalPrice;
            this.idCustomer = idCustomer;
        }
    }
}