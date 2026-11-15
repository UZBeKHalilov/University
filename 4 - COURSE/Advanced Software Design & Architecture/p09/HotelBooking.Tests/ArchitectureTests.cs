using HotelBooking.Reservations;
using NetArchTest.Rules;

namespace HotelBooking.Tests;

// The three borders between the modules. Each test names the types that break its rule.
public class ArchitectureTests
{
    private static readonly Types App = Types.InAssembly(typeof(BookingService).Assembly);

    [Fact]
    public void Reservations_uses_only_the_public_contracts_of_other_modules()
    {
        var result = App.That().ResideInNamespace("HotelBooking.Reservations")
            .ShouldNot().HaveDependencyOnAny("HotelBooking.Rooms.Internal", "HotelBooking.Payments.Internal", "HotelBooking.Notifications.Internal")
            .GetResult();
        Assert.True(result.IsSuccessful, string.Join(", ", result.FailingTypeNames ?? []));
    }

    [Fact]
    public void Rooms_does_not_depend_on_any_other_module()
    {
        var result = App.That().ResideInNamespace("HotelBooking.Rooms")
            .ShouldNot().HaveDependencyOnAny("HotelBooking.Reservations", "HotelBooking.Payments", "HotelBooking.Notifications")
            .GetResult();
        Assert.True(result.IsSuccessful, string.Join(", ", result.FailingTypeNames ?? []));
    }

    [Fact]
    public void Payments_and_Notifications_do_not_depend_on_Reservations()
    {
        var result = App.That().ResideInNamespace("HotelBooking.Payments").Or().ResideInNamespace("HotelBooking.Notifications")
            .ShouldNot().HaveDependencyOn("HotelBooking.Reservations")
            .GetResult();
        Assert.True(result.IsSuccessful, string.Join(", ", result.FailingTypeNames ?? []));
    }
}
