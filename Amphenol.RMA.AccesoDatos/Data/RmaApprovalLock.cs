using Microsoft.EntityFrameworkCore;
using System;

namespace Amphenol.RMA.AccesoDatos.Data
{
    // A SQL session lock also covers the M10 save after the ERP commit.
    // Unlike an in-process lock, it serializes approvals across app instances.
    internal sealed class RmaApprovalLock : IDisposable
    {
        private readonly DbContextM10 _m10Db;
        private readonly string _resource;

        public RmaApprovalLock(DbContextM10 m10Db, int requestId)
        {
            _m10Db = m10Db;
            _resource = $"RMA:M10:Approval:{requestId}";
            _m10Db.Database.OpenConnection();
            try
            {
                _m10Db.Database.ExecuteSqlInterpolated($@"
                    DECLARE @result int;
                    EXEC @result = sys.sp_getapplock
                        @Resource = {_resource}, @LockMode = 'Exclusive',
                        @LockOwner = 'Session', @LockTimeout = 30000;
                    IF @result < 0
                        THROW 51000, 'Another approval is in progress. Please retry.', 1;");
            }
            catch
            {
                _m10Db.Database.CloseConnection();
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _m10Db.Database.ExecuteSqlInterpolated($@"
                    EXEC sys.sp_releaseapplock
                        @Resource = {_resource}, @LockOwner = 'Session';");
            }
            finally
            {
                _m10Db.Database.CloseConnection();
            }
        }
    }
}
