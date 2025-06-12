namespace BookingService.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }
        public ValidationException(IEnumerable<string> errors)
            : base("Se encontraron errores de validación")
        {
            Errors = errors;
        }
    }
}
