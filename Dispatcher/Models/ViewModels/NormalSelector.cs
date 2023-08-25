using Dispatcher.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dispatcher.Models.ViewModels;

public class NormalSelector
{
    public static SelectList TrueOrFalseSelectList => new SelectList(TrueOrFalseChoose.Models,"Right","Label");
    public static SelectList PricingSelectList => new SelectList(PricingMethodModel.Models,
        "Value",
        "Name");
}