using DAL.SqlServer.Context;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Carts.Handlers;

public class RemoveFromCart
{
    public class RemoveFromCartCommand : IRequest<Cart>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, Cart>
    {
        private readonly AppDbContext _context;

        public RemoveFromCartHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.CartLines)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            if (cart == null) throw new Exception("Cart not found");

            var cartLine = cart.CartLines.FirstOrDefault(cl => cl.ProductId == request.ProductId);
            if (cartLine != null)
            {
                cart.CartLines.Remove(cartLine);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }

}
