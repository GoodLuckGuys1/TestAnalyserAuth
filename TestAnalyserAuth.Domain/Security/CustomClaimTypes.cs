using System.Security.Claims;

namespace TestAnalyserAuth.Domain.Security
{
    /// <summary>
    /// Наименование пользовательских <see cref="Claim"/>.
    /// </summary>
    public static class CustomClaimTypes
    {
        /// <summary>
        /// Тип <see cref="Claim"/>, в котором записан платежный план пользователя.
        /// </summary>
        public const string PAYMENT_PLAN = nameof(PAYMENT_PLAN);
    }
}
