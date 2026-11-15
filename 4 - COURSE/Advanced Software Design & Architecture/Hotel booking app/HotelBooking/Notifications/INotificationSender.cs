namespace HotelBooking.Notifications;

// Notifications' public contract: "tell the guest". How the message travels is its own business.
public interface INotificationSender
{
    void SendConfirmation(Confirmation confirmation);
}
