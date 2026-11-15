namespace HotelBooking.Notifications.Internal;

// Simulated, like the email sender.
public class SmsNotificationSender : INotificationSender
{
    public int Sent { get; private set; }

    public void SendConfirmation(Confirmation confirmation) => Sent++;
}
