using LMS.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LMS.Authorization
{
    public class ResourceOperationRequirementCourseHandler : AuthorizationHandler<ResourceOperationRequirement, Course>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ResourceOperationRequirement requirement, Course course)
        {
            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if ((requirement.ResourceOperation == ResourceOperation.Read) &&
                (course.Users.Any(u => u.Id == userId) || course.AuthorId == userId || context.User.IsInRole("Admin")))
            { 
                context.Succeed(requirement);
            }

            if (requirement.ResourceOperation == ResourceOperation.Respond &&
                (course.Users.Any(u => u.Id == userId) || context.User.IsInRole("Admin")))
            {
                context.Succeed(requirement);
            }

            if ((requirement.ResourceOperation == ResourceOperation.Update ||
                requirement.ResourceOperation == ResourceOperation.Delete) && 
                (course.AuthorId == userId ||
                context.User.IsInRole("Admin")))           
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
