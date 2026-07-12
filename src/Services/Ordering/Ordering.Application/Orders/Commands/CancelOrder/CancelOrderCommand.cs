using FluentValidation;

namespace Ordering.Application.Orders.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : ICommand<CancelOrderResult>;

public record CancelOrderResult(bool IsSuccess);

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required");
    }
}

public class CancelOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CancelOrderCommand, CancelOrderResult>
{
    public async Task<CancelOrderResult> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);
        var order = await dbContext.Orders
            .FindAsync(new object[] { orderId }, cancellationToken: cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        order.Cancel();

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CancelOrderResult(true);
    }
}
