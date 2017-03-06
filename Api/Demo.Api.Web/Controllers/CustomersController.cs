using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Core.Dtos;
using Demo.Core.Models;
using Demo.Core.Repositories;
using Demo.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.Api.Web.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomersController : BaseController
    {
        private ICustomerService _service;
        private IMapper _mapper;
        private ILogger<CustomersController> _logger;

        //public CustomersController()
        //{
        //    var resContext = new ReservationContext();
        //    _service = new CustomerService(new ReservationUnitOfWork(resContext, new CustomerRepository(resContext), new OrderRepository(resContext)));
        //}
        public CustomersController(ICustomerService service, IMapper mapper, ILogger<CustomersController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        // GET: /<controller>/
        public async Task<IActionResult> Get()
        {
            var customers = await _service.GetCustomers(null);
            return Ok(_mapper.Map<IEnumerable<CustomerModel>>(customers));
        }

        [HttpGet("{customerId}", Name = "CustomerGet")]
        [ProducesResponseType(typeof(CustomerModel), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 500)]
       public async Task<IActionResult> Get(int customerId)
        {
            //Customer customer = includeOrders
            //    ? await _service.GetCustomerWithOrders(customerId)
            //    : await _service.GetCustomer(customerId);

            Customer customer = await _service.GetCustomerWithOrders(customerId);
            if (customer == null)
            {
                return NotFound($"Customer {customerId} not found");
            }

            return Ok(_mapper.Map<CustomerModel>(customer));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerModel customerModel)
        {
            try
            {
                _logger.LogInformation(1, "New customer created : {@customerModel}", customerModel);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var customer = _mapper.Map<Customer>(customerModel);

                await _service.InsertCustomer(customer);
                string newUri = Url.Link("CustomerGet", new { customerId = customer.Id });

                //uniqye trace identifier : ControllerContext.HttpContext.TraceIdentifier
                return Created(newUri, _mapper.Map<CustomerModel>(customer));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CustomerModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var customer = await _service.GetCustomer(id);
                if (customer == null)
                {
                    return NotFound();
                }

                _mapper.Map(model, customer);

                customer = await _service.UpdateCustomer(customer);

                return Ok(_mapper.Map<CustomerModel>(customer));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var customer = await _service.GetCustomer(id);
                if (customer == null)
                {
                    return NotFound();
                }

                _service.DeleteCustomer(id);

                return Ok();
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
