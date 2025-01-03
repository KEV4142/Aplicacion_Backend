using FluentValidation;
namespace Aplicacion.Transportistas.TransportistaUpdateEstado;
public class TransportistaUpdateEstadoValidator:AbstractValidator<TransportistaUpdateEstadoRequest>
{
    public TransportistaUpdateEstadoValidator()
    {
        RuleFor(x=>x.Estado)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("El Estado no debe de estar en blanco.")
        .Must(estado => estado == "a" || estado == "b" || estado == "i" || estado == "A" || estado == "B" || estado == "I")
                .WithMessage("El Estado debe ser A, B o I.");
    }

}
