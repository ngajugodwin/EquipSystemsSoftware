using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Persistence.Repositories;
using ItemBookingApp_API.Resources.Query;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;

namespace ItemBookingApp_API.Services
{
    public class ReportOrderService : IReportOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService _userService;

        public ReportOrderService(IOrderRepository orderRepository, IUserService userService)
        {
            _orderRepository = orderRepository;
            _userService = userService;
        }

        public async Task<PagedList<Order>> GetOrderReport(OrderReportQuery orderReportQuery)
        {
            var orderReportFromRepo = await _orderRepository.GetOrderReport(orderReportQuery);

            return orderReportFromRepo;
        }

        public async Task<MemoryStream> ExportOrders(OrderReportQuery orderReportQuery)
        {
            // get order report
            var data = await GetOrderReport(orderReportQuery);
            var orders = data.ToList();            
            var tempFile = Path.GetTempFileName(); //create a temp file
            var memory = new MemoryStream();
            try
            {
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Int16.MaxValue))
                {
                    IWorkbook workbook; // create a workgroup using library
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Order_Data");
                    IRow row = excelSheet.CreateRow(0);
                    int rowCounter = 1;

                    //create header rows in excel
                    row.CreateCell(0).SetCellValue("S/N");
                    row.CreateCell(1).SetCellValue("Name");
                    row.CreateCell(2).SetCellValue("Email");
                    row.CreateCell(3).SetCellValue("Duration");                          
                    row.CreateCell(4).SetCellValue("CreatedAt");
                    row.CreateCell(5).SetCellValue("Items");

                    //iterate over the list of orders
                    foreach (var order in orders)
                    {
                        row = excelSheet.CreateRow(rowCounter++);
                        row.CreateCell(0).SetCellValue(rowCounter - 1);
                        row.CreateCell(1).SetCellValue(await GetUserFullName(order.BorrowerEmail));
                        row.CreateCell(2).SetCellValue(order.BorrowerEmail);
                        row.CreateCell(3).SetCellValue(order.BookingInformation.StartDate.Date.ToShortDateString() + " - " + order.BookingInformation.EndDate.Date.ToShortDateString());                                       
                        row.CreateCell(4).SetCellValue(order.OrderDate.ToString("dd/MM/yyyy"));
                        row.CreateCell(5).SetCellValue(GetItems(order));
                    }
                    workbook.Write(fs, false); //write value
                }
                using (var stream = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Int16.MaxValue, FileOptions.DeleteOnClose))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0; 
                return memory; //return memory object
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> GetUserFullName(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = await _userService.GetUserByEmailAsync(email);

                if (user != null)
                {
                    var fullName = string.Empty;

                    fullName = user.FirstName + " " + user.LastName;

                    return fullName;
                }

                return string.Empty;
            }

            return string.Empty;
        }

        private string GetItems(Order order)
        {

            if (order.OrderItems.Count() <= 0)
                return string.Empty;

            if (order != null && order.OrderItems.Count() > 0)
            {
                var itemNames = order.OrderItems.Select(r => r.ItemOrdered.Name);

                return (itemNames.Count() > 1) ? string.Join(", ", itemNames) : itemNames.First();
            }

            return string.Empty;
        }
    }
}
