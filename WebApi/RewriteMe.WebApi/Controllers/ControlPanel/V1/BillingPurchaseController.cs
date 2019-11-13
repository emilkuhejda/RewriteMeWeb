using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class BillingPurchaseController : ControllerBase
    {
        private readonly IBillingPurchaseService _billingPurchaseService;
        private readonly IApplicationLogService _applicationLogService;

        public BillingPurchaseController(
            IBillingPurchaseService billingPurchaseService,
            IApplicationLogService applicationLogService)
        {
            _billingPurchaseService = billingPurchaseService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet("/api/control-panel/purchases/{userId}")]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            try
            {
                var billingPurchases = await _billingPurchaseService.GetAllByUserAsync(userId).ConfigureAwait(false);

                return Ok(billingPurchases);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("/api/control-panel/purchases/detail/{purchaseId}")]
        public async Task<IActionResult> Get(Guid purchaseId)
        {
            try
            {
                var billingPurchase = await _billingPurchaseService.GetAsync(purchaseId).ConfigureAwait(false);
                if (billingPurchase == null)
                    return BadRequest();

                return Ok(billingPurchase);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}