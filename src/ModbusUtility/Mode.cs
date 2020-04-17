namespace ModbusUtility
{
    using System.ComponentModel.DataAnnotations;

    public enum Mode
    {
        /// <summary>
        ///     Modbus RTU Mode.
        /// </summary>
        RTU,

        /// <summary>
        ///     Modbus ASCII Mode.
        /// </summary>
        ASCII
    }

    public enum Timeout
    {
        /// <summary>
        ///     15 (Seconds)
        /// </summary>
        [Display(Name = "15 (Seconds)")]
        S15 = 15,

        /// <summary>
        ///     30 (Seconds)
        /// </summary>
        [Display(Name = "30 (Seconds)")]
        S30 = 30,

        /// <summary>
        ///     1 (Minutes)
        /// </summary>
        [Display(Name = "1 (Minutes)")]
        S60 = 60,

        /// <summary>
        ///     1.5 (Minutes)
        /// </summary>
        [Display(Name = "1.5 (Minutes)")]
        S90 = 90,

        /// <summary>
        ///     3 (Minutes)
        /// </summary>
        [Display(Name = "3 (Minutes)")]
        M3 = 180
    }
}