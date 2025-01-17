﻿using ClosedXML.Excel;
using GemstonesBusinessManagementSystem.DAL;
using GemstonesBusinessManagementSystem.Models;
using GemstonesBusinessManagementSystem.Resources.UserControls;
using GemstonesBusinessManagementSystem.Views;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GemstonesBusinessManagementSystem.ViewModels
{
    public class ServiceViewModel : BaseViewModel
    {
        public int currentPage = 0;
        private string oldName;
        private MainWindow mainWindow;
        private ServiceControl selectedUCService;
        List<Service> services = ServiceDAL.Instance.ConvertDBToList();
        private string price;
        public string Price { get => price; set => price = value; }

        //UC Service
        public ICommand OpenUpdateWindowCommand { get; set; }
        public ICommand DeleteServiceCommand { get; set; }

        //Grid Service in mainWindow
        public ICommand LoadServiceCommand { get; set; }
        public ICommand OpenAddServiceWinDowCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand GoToNextPageCommand { get; set; }
        public ICommand GoToPreviousPageCommand { get; set; }
        public ICommand FindServiceCommand { get; set; }
        public ICommand RestoreServiceCommand { get; set; }
        public ICommand FilterServiceCommand { get; set; }
        public ICommand SortServiceCommand { get; set; }
        //AddService Window
        public ICommand AddServiceCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SeparateThousandsCommand { get; set; }

        public ServiceViewModel()
        {
            //UC Service  - AddService Window
            OpenUpdateWindowCommand = new RelayCommand<ServiceControl>((parameter) => true, (parameter) => OpenUpdateWindow(parameter));
            DeleteServiceCommand = new RelayCommand<ServiceControl>((parameter) => true, (parameter) => DeleteService(parameter));
            AddServiceCommand = new RelayCommand<AddServiceWindow>((parameter) => true, (parameter) => AddService(parameter));
            ExitCommand = new RelayCommand<AddServiceWindow>((parameter) => true, (parameter) => parameter.Close());
            //Grid Service in MainWindow
            LoadServiceCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => LoadServices(parameter, 0));
            OpenAddServiceWinDowCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => OpenAddServiceWinDow(parameter));
            ExportExcelCommand = new RelayCommand<Window>((parameter) => true, (parameter) => ExportExcel());
            GoToNextPageCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => GoToNextPage(parameter, ++currentPage));
            GoToPreviousPageCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => GoToPreviousPage(parameter, --currentPage));
            FindServiceCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => FindService(parameter));
            RestoreServiceCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => RestoreService(parameter));
            FilterServiceCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => FilterService(parameter));
            SortServiceCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => SortService(parameter));
            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));

        }
        //UC Service  - AddService Window
        public void OpenUpdateWindow(ServiceControl uCService)
        {
            selectedUCService = uCService;
            AddServiceWindow addService = new AddServiceWindow();
            Binding binding = BindingOperations.GetBinding(addService.txtNameOfService, TextBox.TextProperty);
            binding.ValidationRules.Clear();
            addService.txtIdService.Text = uCService.txbSerial.Text;
            oldName = addService.txtNameOfService.Text = uCService.txbName.Text;
            addService.txtPriceOfService.Text = uCService.txbPrice.Text;
            addService.cboStatus.SelectedIndex = uCService.txbStatus.Text == "Đang hoạt động" ? 1 : 0; // kiểm tra isActived
            addService.Title = "Sửa thông tin dịch vụ";
            addService.btnSave.Content = "Cập nhật";
            addService.btnSave.ToolTip = "Cập nhật dịch vụ";
            addService.ShowDialog();
        }
        public void AddService(AddServiceWindow addServiceWindow)
        {
            if (CheckData(addServiceWindow)) // kiểm tra dữ liệu đầu vào
            {

                if (ConvertToID(addServiceWindow.txtIdService.Text) > ServiceDAL.Instance.FindMaxId()) // Tạo dịch vụ mới vì id mới
                {
                    if (!ServiceDAL.Instance.IsExisted(addServiceWindow.txtNameOfService.Text)) // kiểm tra tên dịch vụ mới
                    {
                        Service service = new Service(ConvertToID(addServiceWindow.txtIdService.Text), addServiceWindow.txtNameOfService.Text, ConvertToNumber(addServiceWindow.txtPriceOfService.Text), addServiceWindow.cboStatus.SelectedIndex, 0);
                        if (ServiceDAL.Instance.Add(service))
                        {
                            CustomMessageBox.Show("Thêm dịch vụ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            addServiceWindow.Close();

                            ServiceControl uCService = new ServiceControl();
                            uCService.txbSerial.Text = AddPrefix("DV", service.IdService);
                            uCService.txbName.Text = service.Name;
                            uCService.txbPrice.Text = SeparateThousands(service.Price.ToString());
                            uCService.txbStatus.Text = service.IsActive == 1 ? "Đang hoạt động" : "Dừng hoạt động";
                            if (mainWindow.cboSelectFilter.SelectedIndex == service.IsActive || mainWindow.cboSelectFilter.SelectedIndex == -1) // trùng trạng thái với filter thì thêm vào stk
                            {
                                services.Add(service);
                                if (currentPage == (services.Count - 1) / 10)
                                {
                                    mainWindow.stkService.Children.Add(uCService);
                                }
                            }
                        }
                        else
                        {
                            CustomMessageBox.Show("Thêm dịch vụ thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("Tên dịch vụ đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else // id đã xuất hiện thì cập nhật
                {
                    if (!(ServiceDAL.Instance.IsExisted(addServiceWindow.txtNameOfService.Text) && oldName != addServiceWindow.txtNameOfService.Text))
                    {
                        Service service = new Service(ConvertToID(addServiceWindow.txtIdService.Text), addServiceWindow.txtNameOfService.Text, ConvertToNumber(addServiceWindow.txtPriceOfService.Text), addServiceWindow.cboStatus.SelectedIndex, 0);
                        if (ServiceDAL.Instance.Update(service))
                        {
                            CustomMessageBox.Show("Cập nhật dịch vụ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            addServiceWindow.Close();
                            selectedUCService.txbName.Text = service.Name;
                            selectedUCService.txbPrice.Text = SeparateThousands(service.Price.ToString());
                            selectedUCService.txbStatus.Text = service.IsActive == 1 ? "Đang hoạt động" : "Dừng hoạt động";
                            if (service.IsActive != mainWindow.cboSelectFilter.SelectedIndex && mainWindow.cboSelectFilter.SelectedIndex != -1) // kiểm tra trạng thái để remove uc ra khỏi stk 
                            {
                                mainWindow.stkService.Children.Remove(selectedUCService);
                                services.RemoveAll(x => x.IdService == service.IdService);
                                LoadServices(mainWindow, 0);
                            }
                            else // trạng thái trùng với filter hoặc chưa có nên giữ nguyên
                            {
                                int selectedSort = mainWindow.cboSelectSort.SelectedIndex;
                                FindService(mainWindow);
                                if (selectedSort == -1)
                                    services = services.OrderBy(x => x.IdService).ToList();
                                else
                                    mainWindow.cboSelectSort.SelectedIndex = selectedSort;
                            }
                        }
                        else
                        {
                            CustomMessageBox.Show("Cập nhật thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("Tên dịch vụ đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            int start = 0, end = 0;
            LoadInfoOfPage(ref start, ref end); //load lại thông tin pagination
        }
        public void DeleteService(ServiceControl uCService)
        {
            if (CustomMessageBox.Show("Xác nhận xóa dịch vụ?", "Thông báo?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (ServiceDAL.Instance.Delete(ConvertToID(uCService.txbSerial.Text).ToString()))
                {
                    mainWindow.stkService.Children.Remove(uCService);
                    services.RemoveAll(x => x.IdService == ConvertToID(uCService.txbSerial.Text));
                    if (mainWindow.stkService.Children.Count == 0 && currentPage != 0) // kiểm tra có hết trang để chuyển qua trang trước
                        LoadServices(mainWindow, currentPage - 1);
                    else
                        LoadServices(mainWindow, currentPage);
                }
                else
                {
                    CustomMessageBox.Show("Xóa dịch vụ thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            int start = 0, end = 0;
            LoadInfoOfPage(ref start, ref end);
        }
        public bool CheckData(AddServiceWindow addServiceWindow)
        {
            if (string.IsNullOrEmpty(addServiceWindow.txtNameOfService.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên dịch vụ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (string.IsNullOrEmpty(addServiceWindow.txtPriceOfService.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập giá dịch vụ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        //Grid Service in MainWindow
        public void LoadServices(MainWindow mainWindow, int currentPage)
        {
            this.mainWindow = mainWindow;
            mainWindow.stkService.Children.Clear();
            int start = 0, end = 0;
            this.currentPage = currentPage;
            LoadInfoOfPage(ref start, ref end);
            for (int i = start; i < end; i++)
            {
                ServiceControl uCService = new ServiceControl();
                uCService.txbSerial.Text = AddPrefix("DV", services[i].IdService);
                uCService.txbName.Text = services[i].Name;
                uCService.txbPrice.Text = SeparateThousands(services[i].Price.ToString());
                uCService.txbStatus.Text = services[i].IsActive == 1 ? "Đang hoạt động" : "Dừng hoạt động";
                mainWindow.stkService.Children.Add(uCService);
            }

        }
        public void GoToNextPage(MainWindow mainWindow, int currentPage)
        {
            LoadServices(mainWindow, currentPage);
        }
        public void GoToPreviousPage(MainWindow mainWindow, int currentPage)
        {
            LoadServices(mainWindow, currentPage);
        }
        public void FindService(MainWindow mainWindow)
        {
            mainWindow.cboSelectFilter.SelectedIndex = -1;
            mainWindow.cboSelectSort.SelectedIndex = -1;
            services = ServiceDAL.Instance.FindByName(mainWindow.txtSearchService.Text);
            currentPage = 0;
            LoadServices(mainWindow, currentPage);
        }
        public void OpenAddServiceWinDow(MainWindow mainWindow)
        {
            AddServiceWindow addServiceWindow = new AddServiceWindow();
            addServiceWindow.txtIdService.Text = AddPrefix("DV", (ServiceDAL.Instance.FindMaxId() + 1));
            addServiceWindow.txtNameOfService.Text = null;
            addServiceWindow.txtPriceOfService.Text = null;
            addServiceWindow.ShowDialog();
        }
        public void ExportExcel()
        {
            string filePath = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel |*.xlsx"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage p = new ExcelPackage())
                {
                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "Danh sách dịch vụ";
                    p.Workbook.Worksheets.Add("sheet");

                    ExcelWorksheet ws = p.Workbook.Worksheets[0];
                    ws.Name = "DSDV";
                    ws.Cells.Style.Font.Size = 11;
                    ws.Cells.Style.Font.Name = "Calibri";
                    ws.Cells.Style.WrapText = true;
                    ws.Column(1).Width = 10;
                    ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 30;
                    ws.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Tạo danh sách các column header
                    string[] arrColumnHeader = { "STT", "Tên dịch vụ", "Đơn giá", "Trạng thái"};

                    var countColHeader = arrColumnHeader.Count();

                    // merge các column lại từ column 1 đến số column header
                    // gán giá trị cho cell vừa merge
                    ws.Row(1).Height = 15;
                    ws.Cells[1, 1].Value = "Danh sách dịch vụ";
                    ws.Cells[1, 1, 1, countColHeader].Merge = true;

                    ws.Cells[1, 1, 1, countColHeader].Style.Font.Bold = true;
                    ws.Cells[1, 1, 1, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int colIndex = 1;
                    int rowIndex = 2;
                    //tạo các header từ column header đã tạo từ bên trên
                    foreach (var item in arrColumnHeader)
                    {
                        ws.Row(rowIndex).Height = 15;
                        var cell = ws.Cells[rowIndex, colIndex];
                        //set màu thành gray
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(255, 29, 161, 242);
                        cell.Style.Font.Bold = true;
                        //căn chỉnh các border
                        var border = cell.Style.Border;
                        border.Bottom.Style =
                            border.Top.Style =
                            border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                        cell.Value = item;
                        colIndex++;
                    }

                    // lấy ra danh sách nhà cung cấp
                    for (int i = 0; i < services.Count; i++)
                    {
                        Service service = services[i];
                        ws.Row(rowIndex).Height = 15;
                        colIndex = 1;
                        rowIndex++;
                        string address = "A" + rowIndex + ":E" + rowIndex;
                        ws.Cells[address].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        if (rowIndex % 2 != 0)
                        {
                            ws.Cells[address].Style.Fill.BackgroundColor.SetColor(255, 255, 255, 255);
                        }
                        else
                        {
                            ws.Cells[address].Style.Fill.BackgroundColor.SetColor(255, 229, 241, 255);
                        }

                        ws.Cells[rowIndex, colIndex++].Value = i + 1;
                        ws.Cells[rowIndex, colIndex++].Value = service.Name;
                        ws.Cells[rowIndex, colIndex++].Value = service.Price;
                        ws.Cells[rowIndex, colIndex++].Value = (service.IsActive == 1) ? "Đang hoạt động" : "Dừng hoạt động";
                    }
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
                CustomMessageBox.Show("Xuất danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
            }

        }
        public void RestoreService(MainWindow mainWindow)
        {
            int selectedSort = mainWindow.cboSelectSort.SelectedIndex; //Lưu thông tin về filter, sort
            int selectedFilter = mainWindow.cboSelectFilter.SelectedIndex;
            if (ServiceDAL.Instance.RestoreData())
            {
                //Load lại dựa trên search, filter, sort 
                FindService(mainWindow);
                mainWindow.cboSelectSort.SelectedIndex = selectedSort;
                mainWindow.cboSelectFilter.SelectedIndex = selectedFilter;
                CustomMessageBox.Show("Khôi phục dữ liệu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                CustomMessageBox.Show("Khôi phục dữ liệu thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void LoadInfoOfPage(ref int start, ref int end)
        {
            mainWindow.btnPrePageService.IsEnabled = currentPage == 0 ? false : true;
            mainWindow.btnNextPageService.IsEnabled = currentPage == (services.Count - 1) / 10 ? false : true;

            start = currentPage * 10;
            end = (currentPage + 1) * 10;
            if (currentPage == services.Count / 10)
                end = services.Count;

            mainWindow.txtNumOfService.Text = String.Format("Trang {0} trên {1} trang", currentPage + 1, (services.Count - 1) / 10 + 1);
        }
        public void FilterService(MainWindow mainWindow)
        {
            services = ServiceDAL.Instance.FindByName(mainWindow.txtSearchService.Text);
            if (mainWindow.cboSelectFilter.SelectedIndex != 2)
            {
                services.RemoveAll(x => x.IsActive != mainWindow.cboSelectFilter.SelectedIndex);
            }
            if (mainWindow.cboSelectSort.SelectedIndex >= 0)
            {
                SortService(mainWindow);
            }
            else
            {
                LoadServices(mainWindow, 0);
            }
        }
        public void SortService(MainWindow mainWindow)
        {
            switch (mainWindow.cboSelectSort.SelectedIndex)
            {
                case 0:
                    services = services.OrderBy(x => x.Name).ToList();
                    break;
                case 1:
                    services = services.OrderByDescending(x => x.Name).ToList();
                    break;
                case 2:
                    services = services.OrderBy(x => x.Price).ToList();
                    break;
                case 3:
                    services = services.OrderByDescending(x => x.Price).ToList();
                    break;
            }
            LoadServices(mainWindow, 0);
        }
    }
}
