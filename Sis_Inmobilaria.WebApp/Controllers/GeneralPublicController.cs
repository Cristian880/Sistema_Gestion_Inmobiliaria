using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Application.ViewModels.Property;
using Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType;

namespace Sis_Inmobiliaria.WebApp.Controllers
{
    [Authorize(Roles = "GeneralPublic")]
    public class GeneralPublicController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper) : Controller
    {
        // GET: /GeneralPublic/Index
        public async Task<IActionResult> Index()
        {
            var dtos = await propertyService.GetAllWithInclude();

            var listEntityVms = mapper.Map<List<PropertyViewModel>>(dtos);

            return View(listEntityVms);
        }
    }
}