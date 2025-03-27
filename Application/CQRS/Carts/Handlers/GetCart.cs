using DAL.SqlServer.Context;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Carts.Handlers;

public class GetCart
{
    public class GetCartQuery : IRequest<Cart>
    {
        public int UserId { get; set; }
    }

    public class GetCartHandler : IRequestHandler<GetCartQuery, Cart>
    {
        private readonly AppDbContext _context;

        public GetCartHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.CartLines)
                .ThenInclude(cl => cl.Product)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            return cart ?? new Cart { UserId = request.UserId };
        }
    }

}
