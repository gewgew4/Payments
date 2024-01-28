namespace Payments.Domain.Enums
{
    public enum AuthorizationType
    {
        /// <summary>
        /// Cobro
        /// </summary>
        Charge,
        /// <summary>
        /// Devolución
        /// </summary>
        Return,
        /// <summary>
        /// Reversa
        /// </summary>
        Reversal
    }
}
