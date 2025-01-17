﻿using GemstonesBusinessManagementSystem.DAL;
using GemstonesBusinessManagementSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GemstonesBusinessManagementSystem.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        public ICommand MouseLeaveCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public ICommand GetUidCommand { get; set; }
        public ICommand HiddenGridChangeInfoCommand { get; set; }

        private string uid;
        public string Uid { get => uid; set => uid = value; }
        public HomeViewModel()
        {
            NavigateCommand = new RelayCommand<MainWindow>((p) => true, (p) => Navigate(p));
            GetUidCommand = new RelayCommand<Button>((p) => true, (p) => Uid = p.Uid);
            HiddenGridChangeInfoCommand = new RelayCommand<MainWindow>(p => true, p => HiddenGridChangeInfo(p));
            MouseLeaveCommand = new RelayCommand<Button>(p => true, p => MouseLeave(p));
            LogOutCommand = new RelayCommand<MainWindow>(p => true, p => p.Close());
            NavigateCommand = new RelayCommand<MainWindow>(p => true, p => Navigate(p));
            GetUidCommand = new RelayCommand<Button>(p => true, p => { uid = p.Uid; Hover(p); });
        }
        void MouseLeave(Button btn)
        {
            if (btn.IsFocused)
            {
                return;
            }
            string fore = "#666666";
            string back = "#FFFFFF";
            btn.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            btn.Background = (Brush)new BrushConverter().ConvertFrom(back);
        }
        void Hover(Button btn)
        {
            string foreFocus = "#FFF5D577";
            string backFocus = "#FF00329E";
            btn.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
            btn.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
        }
        public void Navigate(MainWindow window)
        {
            int index = int.Parse(Uid);

            window.grdHome.Visibility = Visibility.Collapsed;
            window.grdSale.Visibility = Visibility.Collapsed;
            window.grdStock.Visibility = Visibility.Collapsed;
            window.grdEmployee.Visibility = Visibility.Collapsed;
            window.grdGoods.Visibility = Visibility.Collapsed;
            window.grdImport.Visibility = Visibility.Collapsed;
            window.grdService.Visibility = Visibility.Collapsed;
            window.grdSupplier.Visibility = Visibility.Collapsed;
            window.grdCustomer.Visibility = Visibility.Collapsed;
            window.grdBill.Visibility = Visibility.Collapsed;
            window.grdPayService.Visibility = Visibility.Collapsed;
            window.grdBillService.Visibility = Visibility.Collapsed;
            window.grdSetting.Visibility = Visibility.Collapsed;

            string fore = "#666666";
            string back = "#FFFFFF";
            string foreFocus = "#FFF5D577";
            string backFocus = "#FF00329E";

            window.btnHome.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnStore.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnService.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnStock.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnImport.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnSupplier.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnCustomer.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnEmployee.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnGoods.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnServiceM.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnBillService.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnRevenue.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);
            window.btnSetting.Foreground = (Brush)new BrushConverter().ConvertFrom(fore);

            window.btnHome.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnStore.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnService.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnStock.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnImport.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnSupplier.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnCustomer.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnEmployee.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnGoods.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnServiceM.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnBillService.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnRevenue.Background = (Brush)new BrushConverter().ConvertFrom(back);
            window.btnSetting.Background = (Brush)new BrushConverter().ConvertFrom(back);

            switch (index)
            {
                //home
                case 0:
                    window.txbTabName.Text = "Trang chủ";
                    window.grdHome.Visibility = Visibility.Visible;
                    ReportViewModel reportViewModel = window.grdHome.DataContext as ReportViewModel;
                    reportViewModel.Init(window);
                    window.btnHome.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnHome.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;

                //bán hàng
                case 10:
                    window.txbTabName.Text = "Tạo phiếu bán hàng";
                    window.grdSale.Visibility = Visibility.Visible;
                    CloseExpander(window, 0);
                    window.btnStore.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnStore.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;
                //dịch vụ
                case 11:
                    window.grdPayService.Visibility = Visibility.Visible;
                    window.txbTabName.Text = "Tạo phiếu dịch vụ";
                    BusinessServiceViewModel businessServiceViewModel = new BusinessServiceViewModel();
                    businessServiceViewModel.LoadSaleServices(window);
                    CloseExpander(window, 0);
                    window.btnService.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnService.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;

                //tồn kho
                case 20:
                    window.txbTabName.Text = "Báo cáo tồn kho";
                    window.grdStock.Visibility = Visibility.Visible;
                    StockViewModel stockVM = (StockViewModel)window.grdStock.DataContext;
                    stockVM.LoadStockList(window, 0);
                    CloseExpander(window, 1);
                    window.btnStock.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnStock.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;
                //nhập hàng
                case 21:
                    window.txbTabName.Text = "Danh sách phiếu nhập kho";
                    window.grdImport.Visibility = Visibility.Visible;
                    ImportGoodsViewModel importVM = (ImportGoodsViewModel)window.grdImport.DataContext;
                    importVM.Init(window);
                    CloseExpander(window, 1);
                    window.btnImport.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnImport.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;

                //nhà cc
                case 30:
                    window.txbTabName.Text = "Danh sách nhà cung cấp";
                    window.grdSupplier.Visibility = Visibility.Visible;
                    SupplierViewModel supplierViewModel = window.grdSupplier.DataContext as SupplierViewModel;
                    supplierViewModel.Init(window);
                    CloseExpander(window, 2);
                    window.btnSupplier.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnSupplier.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;
                //khách hàng
                case 31:
                    window.grdCustomer.Visibility = Visibility.Visible;
                    window.txbTabName.Text = "Danh sách khách hàng";
                    CustomerViewModel customerViewModel = window.grdCustomer.DataContext as CustomerViewModel;
                    customerViewModel.customerList = CustomerDAL.Instance.ConvertDBToList();
                    customerViewModel.Load(window);
                    CloseExpander(window, 2);
                    window.btnCustomer.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnCustomer.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;

                //ql nhân viên
                case 40:
                    window.txbTabName.Text = "Quản lý nhân viên";
                    window.grdEmployee.Visibility = Visibility.Visible;
                    EmployeeViewModel employeeVM = (EmployeeViewModel)window.grdEmployee.DataContext;
                    employeeVM.LoadEmployeeList(window, 0);
                    CloseExpander(window, 3);
                    window.btnEmployee.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    window.btnEmployee.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    break;
                //ql hàng hóa
                case 41:
                    window.txbTabName.Text = "Quản lý hàng hóa";
                    window.grdGoods.Visibility = Visibility.Visible;
                    CloseExpander(window, 3);
                    window.btnGoods.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnGoods.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;
                //ql dịch vụ
                case 42:
                    window.txbTabName.Text = "Quản lý dịch vụ";
                    window.grdService.Visibility = Visibility.Visible;
                    ServiceViewModel serviceViewModel = window.grdService.DataContext as ServiceViewModel;
                    serviceViewModel.LoadServices(window, serviceViewModel.currentPage);
                    CloseExpander(window, 3);
                    window.btnServiceM.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnServiceM.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;

                //báo cáo phiếu
                case 50:
                    window.txbTabName.Text = "Danh sách phiếu dịch vụ";
                    if (window.dpStartDateBS.SelectedDate != null)
                    {
                        window.grdBillService.Visibility = Visibility.Visible;
                        DateTime temp = (DateTime)window.dpStartDateBS.SelectedDate;
                        window.dpStartDateBS.SelectedDate = null;
                        window.dpStartDateBS.SelectedDate = temp;
                    }
                    window.grdBillService.Visibility = Visibility.Visible;
                    CloseExpander(window, 4);
                    window.btnBillService.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnBillService.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;
                //báo cáo doanh thu
                case 51:
                    window.grdBill.Visibility = Visibility.Visible;
                    window.txbTabName.Text = "Danh sách hóa đơn bán hàng";
                    CloseExpander(window, 4);
                    window.btnRevenue.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnRevenue.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;
                //cài đặt
                case 6:
                    RoleViewModel roleViewModel = window.grdSetting.DataContext as RoleViewModel;
                    roleViewModel.positionList = EmployeePositionDAL.Instance.GetList();
                    roleViewModel.LoadEmployeePosition(window);
                    window.txbTabName.Text = "Cài đặt";
                    window.grdSetting.Visibility = Visibility.Visible;
                    window.btnSetting.Foreground = (Brush)new BrushConverter().ConvertFrom(foreFocus);
                    window.btnSetting.Background = (Brush)new BrushConverter().ConvertFrom(backFocus);
                    break;
            }
        }
        void CloseExpander(MainWindow window, int index)
        {
            switch (index)
            {
                case 0:
                    window.expWarehouse.IsExpanded = false;
                    window.expPartner.IsExpanded = false;
                    window.expManage.IsExpanded = false;
                    window.expReport.IsExpanded = false;
                    break;
                case 1:
                    window.expStore.IsExpanded = false;
                    window.expPartner.IsExpanded = false;
                    window.expManage.IsExpanded = false;
                    window.expReport.IsExpanded = false;
                    break;
                case 2:
                    window.expStore.IsExpanded = false;
                    window.expWarehouse.IsExpanded = false;
                    window.expManage.IsExpanded = false;
                    window.expReport.IsExpanded = false;
                    break;
                case 3:
                    window.expStore.IsExpanded = false;
                    window.expWarehouse.IsExpanded = false;
                    window.expPartner.IsExpanded = false;
                    window.expReport.IsExpanded = false;
                    break;
                case 4:
                    window.expStore.IsExpanded = false;
                    window.expWarehouse.IsExpanded = false;
                    window.expPartner.IsExpanded = false;
                    window.expManage.IsExpanded = false;
                    break;
            }
        }
        void HiddenGridChangeInfo(MainWindow main)
        {
            if (!main.grdChangeInfo.IsMouseOver)
                main.grdChangeInfo.Visibility = Visibility.Hidden;
        }
    }
}
