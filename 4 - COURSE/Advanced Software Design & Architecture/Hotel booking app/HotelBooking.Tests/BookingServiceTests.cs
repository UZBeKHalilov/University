using HotelBooking.Notifications;
using HotelBooking.Notifications.Internal;
using HotelBooking.Reservations;
using HotelBooking.Reservations.Internal;
using HotelBooking.Rooms;

namespace HotelBooking.Tests;

// A stand-in sender for tests: remembers what it would send, or fails on purpose.
public class FakeNotificationSender(bool fail = false) : INotificationSender
{
    public List<string> Sent { get; } = [];

    public void SendConfirmation(Confirmation confirmation)
    {
        if (fail) throw new InvalidOperationException("The mail server is down.");
        Sent.Add(confirmation.ReservationId);
    }
}

public class BookingServiceTests
{
    private static readonly Room R7 = new("R7", "Standard");
    private static readonly Room R8 = new("R8", "Deluxe");
    private static readonly Guest Maya = new("Maya", "maya@example.com");
    private static readonly Guest Omar = new("Omar", "omar@example.com");
    private static readonly Guest Nora = new("Nora", "nora@example.com");

    private static DateRange Nov(int checkIn, int checkOut) =>
        new(new DateOnly(2026, 11, checkIn), new DateOnly(2026, 11, checkOut));

    // The only place that builds the service: after the refactoring it takes any sender.
    private static (BookingService Service, InMemoryReservationStore Store, FakeNotificationSender Sender) NewService(bool failingSender = false)
    {
        var store = new InMemoryReservationStore();
        var sender = new FakeNotificationSender(failingSender);
        return (new BookingService([R7, R8], store, sender), store, sender);
    }

    [Fact]
    public void Maya_gets_R7_when_it_is_free()
    {
        var (service, _, _) = NewService();
        Assert.Equal("Accepted R101", service.Reserve(Maya, R7, Nov(12, 14)).ToString());
    }

    [Fact]
    public void Omar_is_refused_when_a_night_is_shared()
    {
        var (service, store, _) = NewService();
        service.Reserve(Maya, R7, Nov(12, 14));
        Assert.Equal("Unavailable", service.Reserve(Omar, R7, Nov(13, 15)).ToString());
        Assert.Equal(1, store.Count);
    }

    [Fact]
    public void Nora_can_arrive_on_the_day_Maya_leaves()
    {
        var (service, _, _) = NewService();
        service.Reserve(Maya, R7, Nov(12, 14));
        Assert.Equal("Accepted R102", service.Reserve(Nora, R7, Nov(14, 16)).ToString());
    }

    [Fact]
    public void Check_out_must_be_after_check_in()
    {
        var (service, store, _) = NewService();
        Assert.Equal("InvalidDates", service.Reserve(Omar, R8, Nov(15, 15)).ToString());
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void Cancelling_frees_the_room_again()
    {
        var (service, _, _) = NewService();
        service.Reserve(Maya, R7, Nov(12, 14));
        Assert.True(service.Cancel("R101"));
        Assert.Contains(R7, service.FindFreeRooms(Nov(12, 14)));
    }

    [Fact]
    public void One_confirmation_per_accepted_booking()
    {
        var (service, _, sender) = NewService();
        service.Reserve(Maya, R7, Nov(12, 14));
        service.Reserve(Omar, R7, Nov(13, 15));
        service.Reserve(Nora, R7, Nov(14, 16));
        Assert.Equal(new[] { "R101", "R102" }, sender.Sent);
    }

    [Fact]
    public void A_failed_confirmation_keeps_the_booking()
    {
        var (service, store, _) = NewService(failingSender: true);
        var result = service.Reserve(Maya, R7, Nov(12, 14));
        Assert.Equal("Accepted R101", result.ToString());
        Assert.True(result.NotificationFailed);
        Assert.Equal(1, store.Count);
    }

    [Fact]
    public void Sms_confirmations_need_no_change_to_BookingService()
    {
        var sms = new SmsNotificationSender();
        var service = new BookingService([R7, R8], new InMemoryReservationStore(), sms);
        service.Reserve(Maya, R7, Nov(12, 14));
        Assert.Equal(1, sms.Sent);
    }
}
