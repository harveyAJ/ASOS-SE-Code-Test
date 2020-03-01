namespace App.Validation
{
    public interface IValidator<T>
    {
        bool Validate(T item);
    }
}