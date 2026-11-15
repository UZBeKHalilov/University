namespace HotelBooking.Payments;

// Payments' public contract. Booking does not call it yet: the booking saga uses it in P17.
public interface IPaymentService
{
    PaymentStatus TakeDeposit(string reservationId, decimal amount);
}
