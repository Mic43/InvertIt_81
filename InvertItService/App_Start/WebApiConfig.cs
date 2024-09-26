using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Http;
using InvertItService.Helpers;
using InvertItService.Migrations;
using Microsoft.WindowsAzure.Mobile.Service;
using InvertItService.Models;

namespace InvertItService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            var options = new ConfigOptions();
            options.LoginProviders.Add(typeof(CustomLoginProvider));            

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));
            config.SetIsHosted(true);

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            //Database.SetInitializer(new InvertItInitializer());
            var migrator = new DbMigrator(new Configuration());
            migrator.Update();
        }
    }

    public class InvertItInitializer : ClearDatabaseSchemaIfModelChanges<InvertItContext>
    {
        protected override void Seed(InvertItContext context)
        {
//            List<TodoItem> todoItems = new List<TodoItem>
//            {
//                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
//                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
//            };
//
//            foreach (TodoItem todoItem in todoItems)
//            {
//                context.Set<TodoItem>().Add(todoItem);
//            }

            base.Seed(context);
        }
    }
}

