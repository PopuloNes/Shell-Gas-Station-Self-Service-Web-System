using System.Collections.Generic;
using System.Linq;
using gsst.Interfaces;
using gsst.Model.User;

namespace gsst.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _db;

        public PaymentService(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<PaymentMethod> GetPaymentMethodsForUser(int userId)
        {
            return _db.PaymentMethods.Where(pm => pm.UserId == userId).ToList();
        }

        public PaymentMethod AddPaymentMethod(int userId, string type, string details, bool isDefault)
        {
            if (isDefault)
            {
                var existingDefault = _db.PaymentMethods.FirstOrDefault(pm => pm.UserId == userId && pm.IsDefault);
                if (existingDefault != null)
                {
                    existingDefault.IsDefault = false;
                }
            }

            var method = new PaymentMethod
            {
                UserId = userId,
                Type = type,
                Details = details,
                IsDefault = isDefault
            };

            _db.PaymentMethods.Add(method);
            _db.SaveChanges();
            return method;
        }

        public void RemovePaymentMethod(int methodId)
        {
            var method = _db.PaymentMethods.FirstOrDefault(pm => pm.Id == methodId);
            if (method != null)
            {
                _db.PaymentMethods.Remove(method);
                _db.SaveChanges();
            }
        }
    }
}
