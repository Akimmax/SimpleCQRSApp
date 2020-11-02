using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleCQRSApp.Domain;
using SimpleCQRSApp.Domain.ProductItem;
using EventStore.ClientAPI;
using SimpleCQRSApp.Infrastructure.Handlers;
using SimpleCQRSApp.Infrastructure.Persistence;
using SimpleCQRSApp.Infrastructure.Persistence.EventStore;
using SimpleCQRSApp.Infrastructure.PubSub;
using SimpleCQRSApp.Infrastructure.Read;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SimpleCQRSApp.Services;

namespace SimpleCQRSApp
{
    public class Startup
    {

        private const string ReadModelDBName = "cqrs1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton(x => EventStoreConnection.Create(new Uri("tcp://eventstore:1113")));
            services.AddTransient<ITransientDomainEventPublisher, TransientDomainEventPubSub>();
            services.AddTransient<ITransientDomainEventSubscriber, TransientDomainEventPubSub>();


            services.AddTransient<IRepository<Product, ProductId>, EventSourcingRepository<Product, ProductId>>();
            services.AddSingleton<IEventStore, EventStoreEventStore>();
            services.AddSingleton(x => new MongoClient("mongodb://mongo:27017"));
            services.AddSingleton(x => x.GetService<MongoClient>().GetDatabase(ReadModelDBName));


            services.AddTransient<IReadOnlyRepository<ProductDto>, MongoDBRepository<ProductDto>>();
            services.AddTransient<IRepository<ProductDto>, MongoDBRepository<ProductDto>>();
            services.AddTransient<IReadOnlyRepository<ProductPriceDto>, MongoDBRepository<ProductPriceDto>>();
            services.AddTransient<IRepository<ProductPriceDto>, MongoDBRepository<ProductPriceDto>>();


            services.AddTransient<IDomainEventHandler<ProductId, ProductItemCreated>, ProductProjection>();
            services.AddTransient<IDomainEventHandler<ProductId, ProductItemChanged>, ProductProjection>();
            services.AddTransient<IDomainEventHandler<ProductId, ProductPriceChanged>, ProductProjection>();


            services.AddTransient<IProductWriteService, ProductWriteService>();
            services.AddTransient<IProductReadService, ProductReadService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
