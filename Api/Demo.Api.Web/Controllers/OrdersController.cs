using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Core.Dtos;
using Demo.Core.Interfaces;
using Demo.Core.Models;
using Demo.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderStatus = Demo.Core.Dtos.OrderStatus;

namespace Demo.Api.Web.Controllers
{
    [Route("api/customers/{customerId}/[controller]")]
    public class OrdersController : BaseController
    {
        private ILogger<OrdersController> _logger;
        private IMapper _mapper;
        private IReservationUnitOfWork _uow;

        public OrdersController(IReservationUnitOfWork uow, IMapper mapper, ILogger<OrdersController> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int customerId)
        {
            try
            {
                var result = await _uow.OrderRepository.Get(m => m.CustomerId == customerId, includeProperties: "OrderLines");
                var orders = _mapper.Map<IEnumerable<OrderModel>>(result);
                return Ok(orders);
            }
            catch (Exception e)
            {
                _logger.LogError(3, e, $"Failed to retrieve orders for customer id {customerId}");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}", Name = "OrderGet")]
        public async Task<IActionResult> Get(int customerId, int id)
        {
            try
            {
                var result = await _uow.OrderRepository.GetById(id);
                if (result.CustomerId != customerId)
                {
                    return BadRequest("Order is not associated with this customer");
                }

                var order = _mapper.Map<OrderModel>(result);
                return Ok(order);
            }
            catch (Exception e)
            {
                _logger.LogError(3, e, $"Failed to retrieve orders for customer id {customerId}");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(int customerId, [FromBody] OrderModel model)
        {
            try
            {
                _logger.LogInformation(1, "New order created : {@model}", model);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = _mapper.Map<Order>(model);

                order = _uow.OrderRepository.Insert(order);
                //todo: this is where having a OrderService (domain service) makes sense - raising events should be something from
                //the service and not the controller
                order.Events.Add(new NewOrderCreated(order));
                await _uow.Commit();


                string newUri = Url.Link("OrderGet", new { customerId = customerId, id = order.Id });

                //unique trace identifier : ControllerContext.HttpContext.TraceIdentifier
                return Created(newUri, _mapper.Map<OrderModel>(order));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int customerId, int id, [FromBody] OrderModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = await _uow.OrderRepository.GetById(id);
                if (order == null)
                {
                    return NotFound();
                }

                //automapper bug where the child property, even if set to ignored - automapper does not honor the "ignore" and sets it to null

                //_mapper.Map(model, order);
                switch (model.OrderStatus)
                {
                    case OrderStatus.Active: order.OrderStatus = Core.Models.OrderStatus.Active; break;
                    case OrderStatus.Cancelled: order.OrderStatus = Core.Models.OrderStatus.Cancelled; break;
                    case OrderStatus.Invoiced: order.OrderStatus = Core.Models.OrderStatus.Invoiced;break;
                }


                order.OrderDate = model.OrderDate;
                foreach (var modelOrderLine in model.OrderLines)
                {
                    var orderLine = order.OrderLines.FirstOrDefault(m => m.Id == modelOrderLine.Id);
                    if (orderLine != null)
                    {
                        orderLine.ProductId = modelOrderLine.ProductId;
                        orderLine.Quantity = modelOrderLine.Quantity;
                    }
                    else
                    {
                        order.OrderLines.Add(new OrderLine()
                        {
                            ProductId = modelOrderLine.ProductId,
                            OrderId = id,
                            Quantity = modelOrderLine.Quantity
                        });
                    }

                }
               

                order = _uow.OrderRepository.Update(order);
                await _uow.Commit();

                return Ok(_mapper.Map<OrderModel>(order));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return BadRequest();
        }
    }
}
