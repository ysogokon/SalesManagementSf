using Microsoft.EntityFrameworkCore;
using SalesManagementApp.Data;
using SalesManagementApp.Entities;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services;

public class OrderService : IOrderService
{
  private readonly SalesManagementDbContext _salesManagementDbContext;

  public OrderService ( SalesManagementDbContext salesManagementDbContext )
  {
    _salesManagementDbContext = salesManagementDbContext;
  }
  public async Task CreateOrder ( OrderModel orderModel )
  {
    using var dbContextTransaction = await _salesManagementDbContext.Database.BeginTransactionAsync ();
    try
    {
      Order order = new ()
      {
        OrderDateTime = DateTime.Now,
        ClientId = orderModel.ClientId,
        EmployeeId = 9,
        Price = orderModel.OrderItems.Sum ( o => o.Price ),
        Qty = orderModel.OrderItems.Sum ( o => o.Qty )
      };

      var addedOrder = await _salesManagementDbContext.Orders.AddAsync ( order );
      await _salesManagementDbContext.SaveChangesAsync ();
      int orderId = addedOrder.Entity.Id;

      var orderItemsToAdd = ReturnOrderItemsWithOrderId ( orderId, orderModel.OrderItems );
      _salesManagementDbContext.AddRange ( orderItemsToAdd );

      await _salesManagementDbContext.SaveChangesAsync ();

      await UpdateSalesOrderReportsTable ( orderId, order );

      await dbContextTransaction.CommitAsync ();
    }
    catch ( Exception )
    {
      await dbContextTransaction.DisposeAsync ();
      throw;
    }
  }

  private static List<OrderItem> ReturnOrderItemsWithOrderId ( int orderId, List<OrderItem> orderItems )
  {
    return ( from oi in orderItems
             select new OrderItem
             {
               OrderId = orderId,
               Price = oi.Price,
               Qty = oi.Qty,
               ProductId = oi.ProductId
             } ).ToList ();
  }

  private async Task UpdateSalesOrderReportsTable ( int orderId, Order order )
  {
    try
    {
      List<SalesOrderReport> srItems = await ( from oi in _salesManagementDbContext.OrderItems
                                               where oi.OrderId == orderId
                                               select new SalesOrderReport
                                               {
                                                 OrderId = orderId,
                                                 OrderDateTime = order.OrderDateTime,
                                                 OrderPrice = order.Price,
                                                 OrderQty = order.Qty,
                                                 OrderItemId = oi.Id,
                                                 OrderItemPrice = oi.Price,
                                                 OrderItemQty = oi.Qty,
                                                 EmployeeId = order.EmployeeId,
                                                 EmployeeFirstName = _salesManagementDbContext.Employees.FirstOrDefault ( e => e.Id == order.EmployeeId ).FirstName,
                                                 EmployeeLastName = _salesManagementDbContext.Employees.FirstOrDefault ( e => e.Id == order.EmployeeId ).LastName,
                                                 ProductId = oi.ProductId,
                                                 ProductName = _salesManagementDbContext.Products.FirstOrDefault ( p => p.Id == oi.ProductId ).Name,
                                                 ProductCategoryId = _salesManagementDbContext.Products.FirstOrDefault ( p => p.Id == oi.ProductId ).CategoryId,
                                                 ProductCategoryName = _salesManagementDbContext.ProductCategories.FirstOrDefault ( c => c.Id == _salesManagementDbContext.Products.FirstOrDefault ( p => p.Id == oi.ProductId ).CategoryId ).Name,
                                                 ClientId = order.ClientId,
                                                 ClientFirstName = _salesManagementDbContext.Clients.FirstOrDefault ( c => c.Id == order.ClientId ).FirstName,
                                                 ClientLastName = _salesManagementDbContext.Clients.FirstOrDefault ( c => c.Id == order.ClientId ).LastName,
                                                 RetailOutletId = _salesManagementDbContext.Clients.FirstOrDefault ( c => c.Id == order.ClientId ).RetailOutletId,
                                                 RetailOutletLocation = _salesManagementDbContext.RetailOutlets.FirstOrDefault ( r => r.Id == _salesManagementDbContext.Clients.FirstOrDefault ( c => c.Id == order.ClientId ).RetailOutletId ).Location
                                               } ).ToListAsync ();

      _salesManagementDbContext.AddRange ( srItems );
      await _salesManagementDbContext.SaveChangesAsync ();
    }
    catch ( Exception )
    {

      throw;
    }
  }
}
