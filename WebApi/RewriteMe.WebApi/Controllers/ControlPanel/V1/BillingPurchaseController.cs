using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/purchases")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class BillingPurchaseController : ControllerBase
    {
        private readonly IBillingPurchaseService _billingPurchaseService;

        public BillingPurchaseController(IBillingPurchaseService billingPurchaseService)
        {
            _billingPurchaseService = billingPurchaseService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            var billingPurchases = await _billingPurchaseService.GetAllByUserAsync(userId).ConfigureAwait(false);

            return Ok(billingPurchases);
        }

        [HttpGet("detail/{purchaseId}")]
        public async Task<IActionResult> Get(Guid purchaseId)
        {
            var billingPurchase = await _billingPurchaseService.GetAsync(purchaseId).ConfigureAwait(false);
            if (billingPurchase == null)
                return BadRequest();

            return Ok(billingPurchase);
        }
    }
}