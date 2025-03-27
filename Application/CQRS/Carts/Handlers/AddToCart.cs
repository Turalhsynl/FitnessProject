using DAL.SqlServer.Context;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Carts.Handlers;

public class AddToCart
{
    public class AddToCartCommand : IRequest<Cart>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddToCartHandler : IRequestHandler<AddToCartCommand, Cart>
    {
        private readonly AppDbContext _context;

        public AddToCartHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.CartLines)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            if (cart == null)
            {
                cart = new Cart { UserId = request.UserId };
                _context.Carts.Add(cart);
            }

            var cartLine = cart.CartLines.FirstOrDefault(cl => cl.ProductId == request.ProductId);

            if (cartLine != null)
            {
                cartLine.Quantity += request.Quantity;
            }
            else
            {
                var product = await _context.Products.FindAsync(request.ProductId);
                if (product == null) throw new Exception("Product not found");

                cart.CartLines.Add(new CartLine
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Cart = cart
                });
            }

            await _context.SaveChangesAsync();
            return cart;
        }
    }

}
