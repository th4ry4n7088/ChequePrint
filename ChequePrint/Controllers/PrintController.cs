using ChequePrint.ActionFilters;
using ChequePrint.Models;
using ChequePrint.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChequePrint.Controllers
{
    public class PrintController : ApiController
    {
        private readonly IPrinter _printer;

        // Constructor with dependency injection
        public PrintController(IPrinter printer)
        {
            _printer = printer;
        }

        // POST: api/Print
        [ModelStateValidation]
        [ValidateAntiForgeryHeader]
        public IHttpActionResult Post([FromBody]ChequeModel model)
        {
            if (ModelState.IsValid) // Make sure the model state is valid
            {
                decimal amount = decimal.Parse(model.Amount);

                // Get the printed text of the cheque amount
                string printedAmount = _printer.PrintChequeAmount(amount);

                ChequePrintModel result = new ChequePrintModel()
                {
                    Name = model.Name,
                    PrintedAmount = printedAmount
                };

                return Ok(result);
            }
            else
                return BadRequest();
        }
    }
}
