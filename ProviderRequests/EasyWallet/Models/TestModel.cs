using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProviderRequests.EasyWallet.Models
{
    public class TestModel
    {
        public string MyProperty { get; set; }
    }

    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.MyProperty).NotNull().NotEmpty().WithMessage("test");
        }
    }
}
