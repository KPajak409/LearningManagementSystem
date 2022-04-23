using LMS.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LMS.Authorization
{
    public class ResourceOperationRequirementActivityHandler : AuthorizationHandler<ResourceOperationRequirement, Activity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Activity activity)
        {
            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (requirement.ResourceOperation == ResourceOperation.Read &&
                (activity.Course.Users.Any(u => u.Id == userId) || activity.Course.AuthorId == userId || context.User.IsInRole("Admin")))
            {
                context.Succeed(requirement);
            }        

            if ((requirement.ResourceOperation == ResourceOperation.Update ||
                requirement.ResourceOperation == ResourceOperation.Delete) &&
                activity.Course.AuthorId == userId ||
                context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
