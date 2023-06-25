﻿using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Notification;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Query;
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
        private readonly INotificationService<Mail> _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public ManageOrderService(IOrderRepository orderRepository, 
            IItemRepository itemRepository,
            INotificationService<Mail> notificationService,
            IUnitOfWork unitOfWork, IUserService userService)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
            _userService = userService;
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


            //if (orderForApproval.Status == OrderStatus.PaymentReceived)
            //    return new OrderResponse("Only confirmed payment can be proccessed!");

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
                       
                    }


                    
                    await _unitOfWork.CompleteAsync();

                   // var mailBody = PrepareMailBody(orderForApproval);

                    await _notificationService.SendNotification(new Mail { Body = "You item has been approved", EmailTo = "ngajugodwin@gmail.com", Subject = "EquipSystems - Approval Mail" });
                    
                    return new OrderResponse(orderForApproval);

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
                //if (orderFromRepo.Status == OrderStatus.PaymentFailed)
                //{

                //    _orderRepository.DeleteOrderAsync(orderFromRepo);
                //    await _unitOfWork.CompleteAsync();
                //    return new OrderResponse(orderFromRepo);
                //}

                // return new OrderResponse("Faild to delete order");

                _orderRepository.DeleteOrderAsync(orderFromRepo);
                await _unitOfWork.CompleteAsync();
                return new OrderResponse(orderFromRepo);
            }
            catch (Exception ex)
            {

                return new OrderResponse($"An error occurred while while approving current booking: {ex.Message}");
            }
            

        }

        private string PrepareMailBody(Order order)
        {
            var mailBody = string.Empty;

            mailBody = $"<p>Dear {order.ShipToAddress.FirstName},</p>" + "</b>"
                        + "<p style=\"text-align: justify;\">Please be informed your booking/order request has been approved and will be delivered to you soon.&nbsp;</p>"
                        + "<p style=\"text-align: justify;\">Below is your booking/order information you've placed:</p>"
                        + "<p style=\"text-align: justify;\">&nbsp;</p>"
                        + "<table style=\"border-collapse: collapse; width: 100%; height: 72px;\" border=\"1\">"
                        + "<tbody>"
                        + "<tr style=\"height: 18px;\">"
                        + "<td style=\"width: 100%; height: 18px;\" colspan=\"2\"><strong>Order/Booking #:&nbsp;</strong></td>"
                        + "/tr>"

                        +"<table style=\"border-collapse: collapse; width:\"100%\"; height: \"72%\" border=\"1\"> "
                        + "< tbody >"
                        +"< tr style = \"`height: 18px;\" >"

                        + $"< td style = \"width: 100%; height: 18px;\" colspan =\" 2\">< strong > Order / Booking #: EQIPSYSTEM0000{order.Id};</strong></td>"
                        +"</ tr >"
                        + "< tr style = \"height: 18px;\">"
                        + "< td style = \"width: 22.1591%; height: 18px;\">+< strong> Duration:\"&nbsp;\"</ strong ></ td >"
                        + "< td style = \"width: 77.8409%; height: 18px;\"></ td >"
                        + "</ tr >"
                        + "< tr style = \"height: 18px;\">"
                        + $"< td style =\"width: 22.1591%; height: 18px;\">< strong > Due Date: {order.BookingInformation.EndDate}</ strong ></ td >"
                        + "< td style = \"width: 77.8409%; height: 18px;\" > &nbsp;</ td >"
                        + "</ tr >"
                        + "< tr style = \"height: 18px;\" >"
                        + $"< td style = \"width: 22.1591%; height: 18px;\" >< strong > Processed By: {order.BookingInformation.Status}</ strong ></ td >"
                        + "< td style = \"width: 77.8409%; height: 18px;\" > &nbsp;</ td >"
                        +"</ tr >"
                        +"</ tbody >"
                        +"</ table >"

                        +"< p style =\"text-align: justify;\">" + "/p>"
                        + "< p style =\"text-align: justify;\">" + "We value your patronage.Thank you for choosing EquipSystems Software.</ p >"
                        + "< p style =\"text-align: justify;\">" + "&nbsp;</ p >"
                        + "< p style =\"text-align: justify;\">" + "< em >< strong >for: EquipSystems Software </ strong ></ em ></ p >"
                        + "< p style =\"text-align: justify;\">" + "< em >< strong > Tel: 123456 </ strong ></ em ></ p >"
                        + "< p style =\"text-align: justify;\">" + "< em >< strong > Email: equipsystems@gmail.com </ strong ></ em ></ p >";


            return mailBody;


        }


    }
}
