using Microsoft.EntityFrameworkCore;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Extensions
{
  public static class InvoiceExtensions
  {
    public static void UpdatePaidAmounts(this DbContext context)
    {
      // Collecter toutes les factures modifiées ou dont les paiements sont modifiés
      var modifiedInvoices = context.ChangeTracker.Entries<Invoice>()
          .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
          .Select(e => e.Entity)
          .ToDictionary(e => e.Id);

      // IDs des factures avec paiements modifiés
      var invoiceIdsFromPayments = context.ChangeTracker.Entries<Payment>()
          .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
          .Select(e => e.Entity.InvoiceId)
          .Distinct();

      // Traiter les factures déjà en mémoire
      foreach (var invoiceId in invoiceIdsFromPayments)
      {
        Invoice? invoice = null;

        // Si la facture est déjà dans le contexte, l'utiliser
        if (modifiedInvoices.TryGetValue(invoiceId, out var cachedInvoice))
        {
          invoice = cachedInvoice;
        }
        else if (invoiceId != 0) // Sinon, la chercher en base (uniquement si ID valide)
        {
          invoice = context.Set<Invoice>().Find(invoiceId);
        }

        if (invoice != null)
        {
          var payments = context.Set<Payment>()
              .Where(p => p.InvoiceId == invoiceId)
              .ToList();

          invoice.CalculatePaidAmount(payments);
        }
      }
    }
  }
}