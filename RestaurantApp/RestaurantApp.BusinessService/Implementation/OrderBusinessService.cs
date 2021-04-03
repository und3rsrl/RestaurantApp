using AutoMapper;
using RestaurantApp.BusinessEntities.DTOs.Order;
using RestaurantApp.BusinessEntities.DTOs.Waiter;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using RestaurantApp.DataServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Implementation
{
    public class OrderBusinessService : IOrderBusinessService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IWaiterRepository _waiterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderBusinessService
            (
                IOrderRepository orderRepository,
                IUnitOfWork unitOfWork,
                IMapper mapper
            )
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAll();
        }

        public async Task<IEnumerable<PreviousOrderDetails>> GetUserPreviousOrders(string userEmail)
        {
            var previousOrders = await _orderRepository.GetUserPreviousOrders(userEmail);
            return _mapper
                .Map<IEnumerable<PreviousOrderDetails>>(previousOrders)
                .OrderByDescending(o => o.SubmitDate);
        }

        public async Task<IEnumerable<WaiterOrderInfoDetails>> GetWaiterActiveOrders(string waiterEmail)
        {
            var waiterActiveOrders = await _orderRepository.GetWaiterActiveOrders(waiterEmail);
            return _mapper.Map<IEnumerable<WaiterOrderInfoDetails>>(waiterActiveOrders);
        }

        public Task<Order> GetActiveOrder(int id)
        {
            return _orderRepository.GetActiveOrder(id);
        }

        public Task<Order> GetUserActiveOrder(string userEmail)
        {
            return _orderRepository.GetUserActiveOrder(userEmail);
        }

        public async Task<OperationResult> CreateOrder(OrderDTO orderDetails)
        {
            try
            {
                var order = new Order
                {
                    IsPaid = false,
                    SubmitDateTime = DateTime.Now,
                    Submitter = orderDetails.Submitter,
                    Total = orderDetails.Total,
                    Table = orderDetails.Table
                };

                // Check waiter status
                var waiters = await _waiterRepository.GetActiveWaiters();

                var randomGenerator = new Random();
                var waiterIndex = randomGenerator.Next(waiters.Count());
                order.Waiter = waiters[waiterIndex].Waiter;
                order.OrderItems = _mapper.Map<ICollection<OrderItem>>(orderDetails.OrderItems);

                _orderRepository.Add(order);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> UpdateOrder(int id, Order order)
        {
            try
            {
                if (id != order.Id)
                {
                    return OperationResult.Failed;
                }

                _orderRepository.Update(order);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> DeleteOrder(int id)
        {
            try
            {
                _orderRepository.DeleteById(id);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> MarkAsPaid(int orderId)
        {
            try
            {
                var order = _orderRepository.GetById(orderId);

                if (order == null)
                {
                    return OperationResult.Failed;
                }

                order.IsPaid = true;

                _orderRepository.Update(order);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> MarkAsWaiterPayment(int orderId)
        {
            try
            {
                var order = _orderRepository.GetById(orderId);

                if (order == null)
                {
                    return OperationResult.Failed;
                }

                order.WaiterPayment = true;

                _orderRepository.Update(order);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }
    }
}
