using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using FBPortal.Domain.Entities;
using FBPortal.API.Models;
using FBPortal.Domain.Abstract;
using FBPortal.Domain.Concrete;

namespace FBPortal.API.Controllers
{
    public class InvoicesController : ApiController
    {

        IInvoiceRepository repository;

        public InvoicesController(IInvoiceRepository repo) { repository = repo; }

        private static readonly Expression<Func<Invoice, Models.DTOs.Invoice>> AsInvoiceDTO = i => new Models.DTOs.Invoice
        {
            InvoiceId = i.InvoiceId,
            Name = i.Name,
            Vendor = i.Vendor,
            AmountPaid = i.AmountPaid

        };

        // GET api/Invoices
        public IQueryable<Models.DTOs.Invoice> GetInvoices()
        {
            return repository.Invoices.Select(AsInvoiceDTO);
        }

        // GET api/Invoices/5
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> GetInvoice(Guid id)
        {
            Invoice invoice = await repository.Invoices.SingleAsync(i => i.InvoiceId.Equals(id));
            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        // PUT api/Invoices/5
        public async Task<IHttpActionResult> PutInvoice(Guid id, Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }

            //db.Entry(invoice).State = EntityState.Modified;

            try
            {
                await repository.Edit(invoice);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Invoices
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> PostInvoice(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await repository.Create(invoice); ;
            }
            catch (DbUpdateException)
            {
                if (InvoiceExists(invoice.InvoiceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = invoice.InvoiceId }, invoice);
        }

        // DELETE api/Invoices/5
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> DeleteInvoice(Guid id)
        {
            
            Invoice invoice = await repository.Invoices.SingleAsync(i=>i.InvoiceId.Equals(id));

            if (invoice == null)
            {
                return NotFound();
            }

            await repository.Delete(invoice);

            return Ok(invoice);
        }

        private bool InvoiceExists(Guid id)
        {
            return repository.Invoices.Count(e => e.InvoiceId.Equals(id)) > 0;
        }
    }
}