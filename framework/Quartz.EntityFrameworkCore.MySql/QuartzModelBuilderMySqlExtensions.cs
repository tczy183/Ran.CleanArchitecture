﻿namespace Quartz.EntityFrameworkCore.MySql;

public static class QuartzModelBuilderMySqlExtensions
{
    public static IQuartzModelBuilder UseMySql(this IQuartzModelBuilder builder, string prefix = "QRTZ_")
    {
        builder.UseEntityTypeConfigurations(context =>
        {
            context.ModelBuilder.ApplyConfiguration(
                new QuartzJobDetailEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzTriggerEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzSimpleTriggerEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzSimplePropertyTriggerEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzCronTriggerEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzBlobTriggerEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzCalendarEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzPausedTriggerGroupEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzFiredTriggerEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzSchedulerStateEntityTypeConfiguration(prefix));

            context.ModelBuilder.ApplyConfiguration(
                new QuartzLockEntityTypeConfiguration(prefix));
        });

        return builder;
    }
}
