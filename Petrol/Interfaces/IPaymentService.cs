using System.Collections.Generic;
using gsst.Model.User;

namespace gsst.Interfaces
{
    public interface IPaymentService
    {
        IEnumerable<PaymentMethod> GetPaymentMethodsForUser(int userId);
        PaymentMethod AddPaymentMethod(int userId, string type, string details, bool isDefault);
        void RemovePaymentMethod(int methodId);
    }
}
