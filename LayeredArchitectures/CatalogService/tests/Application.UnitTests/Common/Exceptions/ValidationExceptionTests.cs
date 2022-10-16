using CatalogService.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace CatalogService.Application.UnitTests.Common.Exceptions;

public class ValidationExceptionTests
{
    [Test]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Test]
    public void SingleValidationFailureCreatesASingleElementErrorDictionary()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Key", "Error message"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "Key" });
        actual["Key"].Should().BeEquivalentTo(new string[] { "Error message" });
    }

    [Test]
    public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Key 1", "must be 1"),
                new ValidationFailure("Key 1", "must be 2"),
                new ValidationFailure("Key 2", "must be 1"),
                new ValidationFailure("Key 2", "must be 2"),
                new ValidationFailure("Key 2", "must be 3"),
                new ValidationFailure("Key 2", "must be 4"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "Key 1", "Key 2" });

        actual["Key 1"].Should().BeEquivalentTo(new string[]
        {
                "must be 1",
                "must be 2",
        });

        actual["Key 2"].Should().BeEquivalentTo(new string[]
        {
                "must be 1",
                "must be 2",
                "must be 3",
                "must be 4",
        });
    }
}
