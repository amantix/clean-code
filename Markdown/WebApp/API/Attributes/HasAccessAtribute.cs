using API.Filters;
using Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Attributes;

public class HasAccessAttribute : TypeFilterAttribute
{
    public HasAccessAttribute(DocumentAccessRoles requiredRole) 
        : base(typeof(HasAccessFilter))
    {
        Arguments = [requiredRole];
    }
}