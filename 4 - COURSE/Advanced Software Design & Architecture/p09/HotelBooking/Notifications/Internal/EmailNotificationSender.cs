namespace HotelBooking.Notifications.Internal;

// Simulated: counts the emails it would send and needs no network.
public class EmailNotificationSender : INotificationSender
{
    public int Sent { get; private set; }

    public void SendConfirmation(Confirmation confirmation) => Sent++;
}
