using Microsoft.AspNetCore.Authorization;

namespace LMS.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update,
        Delete,
        Respond
    }
    public class ResourceOperationRequirement :IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation { get; set; }
        public ResourceOperationRequirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
    }
}
