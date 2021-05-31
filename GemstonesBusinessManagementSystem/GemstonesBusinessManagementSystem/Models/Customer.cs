﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemstonesBusinessManagementSystem.Models
{
    class Customer
    {
        private int idCustomer;
        private string customerName;
        private string phoneNumber;
        private int idNumber;
        //private long totalPrice;
        private string address;
        public int IdCustomer { get => idCustomer; set => idCustomer = value; }
        public string CustomerName { get => customerName; set => customerName = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public int IdNumber { get => idNumber; set => idNumber = value; }
        //public long TotalPrice { get => totalPrice; set => totalPrice = value; }
        public string Address { get => address; set => address = value; }

        public Customer()
        {

        }

        public Customer(int idCustomer, string customerName,
            string phoneNumber, int idNumber, string address)
        {
            this.idCustomer = idCustomer;
            this.customerName = customerName;
            this.phoneNumber = phoneNumber;
            this.idNumber = idNumber;
            //this.totalPrice = totalPrice;
            this.address = address;
        }
    }
}