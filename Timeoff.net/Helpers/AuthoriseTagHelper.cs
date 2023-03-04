using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Timeoff.Helpers
{
    [HtmlTargetElement(Attributes = "asp-authorise")]
    [HtmlTargetElement(Attributes = "asp-authorise,asp-policy")]
    [HtmlTargetElement(Attributes = "asp-authorise,asp-roles")]
    [HtmlTargetElement(Attributes = "asp-authorise,asp-authentication-schemes")]
    public class AuthoriseTagHelper : TagHelper, IAuthorizeData
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private readonly IPolicyEvaluator _policyEvaluator;

        [HtmlAttributeName("asp-roles")]
        public string? Roles { get; set; }

        public string? Policy { get; set; }
        public string? AuthenticationSchemes { get; set; }

        public AuthoriseTagHelper(
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationPolicyProvider policyProvider,
            IPolicyEvaluator policyEvaluator)
        {
            _httpContextAccessor = httpContextAccessor;
            _policyProvider = policyProvider;
            _policyEvaluator = policyEvaluator;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, new[] { this });

            var authenticateResult = await _policyEvaluator.AuthenticateAsync(policy!, _httpContextAccessor.HttpContext!);

            var authorizeResult = await _policyEvaluator.AuthorizeAsync(policy!, authenticateResult, _httpContextAccessor.HttpContext!, null);

            if (!authorizeResult.Succeeded)
            {
                output.SuppressOutput();
            }
        }
    }
}