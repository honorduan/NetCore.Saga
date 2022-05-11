using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Saga.Abstract.Entity;

namespace NetCore.Saga.Server.Mysql.Context.Entities
{
    public class EventMapping:IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.ToTable("EventTable");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.GlobalId).HasColumnName("GlobalId").HasMaxLength(36).HasComment("全局id");
            builder.Property(c => c.LocalId).HasColumnName("LocalId").HasMaxLength(36).HasComment("本地id");
            builder.Property(c => c.ServiceName).HasColumnName("ServiceName").HasMaxLength(50).HasComment("服务名");
            builder.Property(c => c.ServiceId).HasColumnName("ServiceId").HasMaxLength(36).HasComment("服务id");
            builder.Property(c => c.CompensableMethod).HasColumnName("CompensableMethod").HasMaxLength(100).HasComment("补偿的方法");
            builder.Property(c => c.ImplementMethod).HasColumnName("ImplementMethod").HasMaxLength(100).HasComment("实现的方法");
            builder.Property(c => c.Payloads).HasColumnName("Payloads").HasComment("参数");
            builder.Property(c => c.Type).HasColumnName("Type").HasColumnType("nvarchar(24) ").HasComment("类型");
            builder.Property(c => c.TypeName).HasColumnName("TypeName").HasMaxLength(500).HasComment("方法类型");
            builder.Property(c => c.Retries).HasColumnName("Retries").HasComment("重试次数");
            builder.Property(c => c.Timestamp).HasColumnName("Timestamp").HasComment("时间戳");
            builder.Property(c => c.ExceptionMessage).HasColumnName("ExceptionMessage").HasMaxLength(500).HasComment("异常信息");
        }
    }
}
