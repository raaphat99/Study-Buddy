using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            if (context != null)
            {
                var entries = context.ChangeTracker.Entries()
                    .Where(entry => (entry.Entity is Message || entry.Entity is Room) && (entry.State == EntityState.Added || entry.State == EntityState.Modified));

                foreach (var entry in entries)
                {
                    ProcessEntity(entry);
                }
            }
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        private void ProcessEntity(EntityEntry entry)
        {
            switch (entry.Entity)
            {
                case Message message:
                    UpdateTimestamps(message, entry.State);
                    break;

                case Room room:
                    UpdateTimestamps(room, entry.State);
                    break;
            }
        }

        private void UpdateTimestamps(dynamic entity, EntityState state)
        {
            if (state == EntityState.Added)
            {
                entity.Created = DateTime.UtcNow;
            }
            entity.Updated = DateTime.UtcNow;
        }
    }
}
