﻿using Contacts37.Domain.Exceptions;
using Contacts37.Domain.ValueObjects;
using FluentAssertions;

namespace Contacts37.Domain.Tests.ValueObjects
{
    public class RegionTests
    {
        [Theory(DisplayName = "Validate region creation with valid DDD code")]
        [Trait("Category", "Create Region - Success")]
        [InlineData(11, "Sudeste")]
        [InlineData(42, "Sul")]
        [InlineData(65, "Centro-Oeste")]
        [InlineData(83, "Nordeste")]
        [InlineData(94, "Norte")]
        public void CreateRegion_ValidDddCode_ShouldCreateRegion(int dddCode, string name)
        {
            // Act
            var region = Region.Create(dddCode);

            // Assert
            region.Should().NotBeNull();
            region.DddCode.Should().Be(dddCode);
            region.Name.Should().Be(name);
        }

        [Theory(DisplayName = "Validate region creation with invalid DDD codes")]
        [Trait("Category", "Create Region - Failure")]
        [InlineData(5)]
        [InlineData(192)]
        [InlineData(-42)]
        public void CreateRegion_InvalidDddCode_ShouldThrowException(int invalidDddCode)
        {
            // Act
            Action act = () => Region.Create(invalidDddCode);

            // Assert
            act.Should().Throw<InvalidDDDException>()
                            .WithMessage($"DDD code '{invalidDddCode}' is invalid or does not belong to any region.");
        }
    }
}
