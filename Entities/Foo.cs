using FakeDbTestModel.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FakeDbTestModel.Entities;

public class Foo : DbRecord
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public static void CreateModel([NotNull] ModelBuilder mb)
    {
        // take care of defining the basics
        CreateModel<Foo>(mb);

        // Define other specifics here (if required)
    }
}
