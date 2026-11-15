namespace HotelBooking.Payments.Internal;

// Simulated: no card and no network. A positive amount is Paid; anything else is Declined.
public class SimulatedPaymentService : IPaymentService
{
    public PaymentStatus TakeDeposit(string reservationId, decimal amount) =>
        amount > 0 ? PaymentStatus.Paid : PaymentStatus.Declined;
}
