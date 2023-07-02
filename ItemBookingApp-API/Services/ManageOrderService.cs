using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Notification;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Query;
using MailKit.Search;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ItemBookingApp_API.Services
{
    public class ManageOrderService :IManageOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService<Mail> _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public ManageOrderService(IOrderRepository orderRepository, 
            IItemRepository itemRepository,
            IPaymentService paymentService,
            INotificationService<Mail> notificationService,
            IUnitOfWork unitOfWork, IUserService userService)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _paymentService = paymentService;
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int id)
        {
            var order =  await _orderRepository.GetOrderByIdAsync(id);

            if (order == null)
                return new OrderResponse("Order not found");

            return new OrderResponse(order);
        }

        public async Task<PagedList<Order>> GetBookingsListForModerationAsync(OrderQuery orderQuery)
        {
            return await _orderRepository.GetOrdersListForModerationAsync(orderQuery);
        }

        public async Task<OrderResponse> ApproveOrder(string approvedByUserEmail, int orderId)
        {
            var approvalUser = await _userService.GetUserByEmailAsync(approvedByUserEmail);

            if (approvalUser == null)
                return new OrderResponse("Approval user not found");

            var orderForApproval = await _orderRepository.GetOrderByIdAsync(orderId);

            if (orderForApproval == null)
                return new OrderResponse("Order not found");

            try
            {
                if (orderForApproval.BookingInformation.Status == ApprovalStatus.Pending)
                {
                    var itemIds = orderForApproval.OrderItems.Select(x => x.ItemOrdered.ItemId).ToArray();
                    var items = await _itemRepository.ListAsync(itemIds);

                    var result = _itemRepository.SetItemState(items, ItemState.Borrowed);

                    if (result)
                    {
                        orderForApproval.BookingInformation.Status = ApprovalStatus.Approved;
                        orderForApproval.Status = OrderStatus.PaymentReceived;
                        await _unitOfWork.CompleteAsync();
                        var mailBody = PrepareApprovalMailBody(orderForApproval);
                        await _notificationService.SendNotification(new Mail { Body = mailBody, EmailTo = orderForApproval.BorrowerEmail, Subject = "EquipSystemsSoftware - Approval Mail" });
                        return new OrderResponse(orderForApproval);
                    }

                    return new OrderResponse("Unable to approve request");
                }
                else
                {
                    return new OrderResponse("Only order marked as pending can be approved");
                }
            }
            catch (Exception ex)
            {

                return new OrderResponse($"An error occurred while while approving current booking: {ex.Message}");
            }         
        }

        public async Task<OrderResponse> CloseOrder(int orderId)
        {
            var orderFromRepo = await _orderRepository.GetOrderByIdAsync(orderId);

            if (orderFromRepo == null)
                return new OrderResponse("Order not found");

            if (orderFromRepo.BookingInformation.Status == ApprovalStatus.Approved && orderFromRepo.BookingInformation.ReturnedDate == null)
            {
                orderFromRepo.BookingInformation.ReturnedDate = DateTime.Now;
                orderFromRepo.BookingInformation.Status = ApprovalStatus.Closed;

                var itemIds = orderFromRepo.OrderItems.Select(x => x.ItemOrdered.ItemId).ToArray();
                var items = await _itemRepository.ListAsync(itemIds);

                var result = _itemRepository.SetItemState(items, ItemState.Available);

                if (result)
                {
                    await _unitOfWork.CompleteAsync();

                    var mailBody = PrepareClosingMailBody(orderFromRepo);
                    await _notificationService.SendNotification(new Mail { Body = mailBody, EmailTo = orderFromRepo.BorrowerEmail, Subject = "EquipSystemsSoftware - Approval Mail" });

                    return new OrderResponse(orderFromRepo);
                }

                return new OrderResponse("Failed to close booking");
            }
            else
            {
                return new OrderResponse("You must approve a booking before closing");
            }
        }

        public async Task<OrderResponse> RejectOrder(int orderId)
        {
            var orderFromRepo = await _orderRepository.GetOrderByIdAsync(orderId);

            if (orderFromRepo == null)
                return new OrderResponse("Booking not found");

            if (orderFromRepo.BookingInformation.Status == ApprovalStatus.Closed || orderFromRepo.BookingInformation.Status == ApprovalStatus.Approved)
            {
                return new OrderResponse("A booking marked as closed or approved cannot be rejected");
            }


            try
            {

                _orderRepository.DeleteOrderAsync(orderFromRepo);
                await _unitOfWork.CompleteAsync();
                return new OrderResponse(orderFromRepo);
            }
            catch (Exception ex)
            {

                return new OrderResponse($"An error occurred while while approving current booking: {ex.Message}");
            }            

        }

        private string PrepareApprovalMailBody(Order order)
        {
            var mailBody = string.Empty;

            mailBody = $"<p>Dear {order.ShipToAddress.FirstName},</p>" + "</b>"
                        + "<p style=\"text-align: justify;\">Please be informed your booking/order request has been approved and will be delivered to you soon.&nbsp;</p>"

                        + "<p style=\"text-align: justify;\">We value your patronage. Thank you for choosing EquipSystems Software.</p>"
                        + "<br>"
                        + "<br>"
                        + "<p style=\"text-align: justify;\">for: EquipSystems Software.</p>"
                        +"<p style=\"text-align: justify;\">Tel: 123456</p>"
                        + "<p style=\"text-align: justify;\">Email: equipsystems@gmail.com</p>";

            return mailBody;
        }

        private string PrepareClosingMailBody(Order order)
        {
            var mailBody = string.Empty;

            mailBody = $"<p>Dear {order.ShipToAddress.FirstName},</p>" + "</b>"
                        + "<p style=\"text-align: justify;\">Please be informed your booking/order request has been closed.&nbsp;</p>"

                        + "<p style=\"text-align: justify;\">We value your patronage. Thank you for choosing EquipSystems Software.</p>"
                        + "<br>"
                        + "<br>"
                        + "<p style=\"text-align: justify;\">for: EquipSystems Software.</p>"
                        + "<p style=\"text-align: justify;\">Tel: 123456</p>"
                        + "<p style=\"text-align: justify;\">Email: equipsystems@gmail.com</p>";

            return mailBody;
        }


    }
}
