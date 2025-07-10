# Ran.CleanArchitecture

一个基于`ASP.NET Core`实现的`领域驱动设计`落地`战术`框架。

A `tactical` framework for `Domain-Driven Design` based on `ASP.NET Core`.

核心特性：

+ 领域驱动设计实践支持
+ CQRS
+ Event Driven
+ 分布式事务（事件处理的最终一致性）
+ 多租户
+ 多环境（灰度发布）

## 愿景

随着 .NET
技术生态的发展，其在云原生时代的微服务架构已经发展得非常成熟，而领域驱动设计的落地也得到了非常好的支持。同时随着各行各业的信息化、数字化发展诉求越发强烈，更多的企业和团队也对如何有效地组织研发团队以及实现自己的业务架构这个课题开始投入关注。

本项目的核心目的是帮助企业快速构建一套基于领域驱动设计的技术实现框架，同时在领域驱动设计方法论方面进行沉淀和探讨，从而让更多的企业和团队得到帮助。

## Roadmap

规划提供的能力

+ [x] 支持灵活配置与部署的网关
+ [x] 基于 `ASP.NET Core`和开源组件的快速开发框架
+ [x] 提供领域驱动设计实现的代码模板工程脚手架
+ [x] 实现具备业务扩展性的整体灰度解决方案
+ [x] 实现具备业务扩展性的租户能力
+ [x] 基于领域驱动设计的微服务架构实践
+ [x] 模块化的设计，可按需使用、按需替换
+ [x] 提供详实的文档
+ [x] 提供带有可视化操作界面的微服务基础设施
    + [x] 基于 .NET Aspire

## 组件说明

+ [x] Context Passing
    + [x] AspNetCore (HTTP Request)
    + [x] HttpClient
    + [x] RabbitMQ (Based on DotNetCore.CAP)
+ [x] Domain
    + [x] Entity
    + [x] AggregateRoot
    + [x] DomainEvent
+ [x] Repository (Based On EntityFrameworkCore)
+ [x] Transaction
    + [x] UnitOfWork
    + [x] Distributed Transaction
        + [x] Outbox(Based on DotNetCore.CAP)
+ [x] IdGeneration
    + [x] Snowflake
        + [x] Snowflake with Etcd
        + [x] Snowflake with Redis
        + [x] Snowflake with Consul
+ [x] Primitives
    + [x] Exception Handling
    + [x] Clock
+ [x] Service Discovery
    + [x] Microsoft Service Discovery (Aspire)
    + [x] Service Discovery Kubernetes
+ [x] Multi Tenant
+ [x] Multi Environment
    + [x] Gray Environment
