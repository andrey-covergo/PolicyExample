using System.Collections.Generic;
using FluentAssertions;
using PolicyExample.Scripting.Jint;
using Xunit;

namespace PolicyExample.Tests
{
    public class ScriptServiceSetTests
    {
        [Fact]
        public void Given_service_set_When_injecting_existing_services_When_injector_is_called_with_provided_service_and_schemas()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            var descriptor = new ScriptService(){Name="Test service", Version = 1};
            set.Add(descriptor, service );

            object? resolved=null;
            IReadOnlyCollection<ScriptServiceSchema> schemas=null;
            
            set.Inject((sc,s)=>
            {
                
                resolved = s;
                schemas = sc;

            }, descriptor);

            resolved.Should().Be(service);
            schemas.Should().BeEquivalentTo(ScriptServiceSchema.Default(descriptor));
        }
        
        [Fact]
        public void Given_service_set_When_injecting_requiring_not_existing_services_When_error_occurs()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            set.Add(new ScriptService(){Name="Test service", Version = 1}, service );

            object resolved;
            set.Invoking(s => s.Inject((sc,s) => { }, new ScriptService(){Name="Test serviceA", Version = 1}))
                .Should().Throw<MissingServiceException>();
        }
        
        [Fact]
        public void Given_service_set_When_injecting_required_existing_services_with_wrong_version_When_error_occurs()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            set.Add(new ScriptService(){Name="Test service", Version = 1}, service );

            set.Invoking(s => s.Inject((sc,s) => { }, new ScriptService(){Name="Test service", Version = 2}))
                .Should()
                .Throw<MissingServiceException>();
        }
        
        [Fact]
        public void Given_service_set_When_adding_description_for_existing_services_When_can_get_it_back()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            var scriptService = new ScriptService(){Name="Test service", Version = 1};
            var scriptServiceSchema = new ScriptServiceSchema()
            {
                AccessName = "test",
                Language = Language.JavaScriptEs5
            };
            
            set.Add(scriptService, service, scriptServiceSchema );

            IReadOnlyCollection<ScriptServiceSchema>? resolvedSchema=null;
            object? resolvedService=null;
            set.Inject((sc, s) =>
            {
                resolvedSchema = sc;
                resolvedService = s;
            },scriptService);
            
            resolvedService.Should().Be(service);
            resolvedSchema.Should().BeEquivalentTo(scriptServiceSchema);
        }
        
        [Fact]
        public void Given_service_set_When_adding_description_for_existing_services_When_can_get_it_back_by_language()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            var scriptService = new ScriptService(){Name="Test service", Version = 1};
            var scriptServiceSchema = new ScriptServiceSchema()
            {
                AccessName = "test",
                Language = Language.JavaScriptEs5
            };
            
            set.Add(scriptService, service, scriptServiceSchema );
            
            IReadOnlyCollection<ScriptServiceSchema>? resolvedSchema=null;
            object? resolvedService=null;
            set.Inject((sc, s) =>
            {
                resolvedSchema = sc;
                resolvedService = s;
            },scriptService);
            
            resolvedService.Should().Be(service);
            resolvedSchema.Should().BeEquivalentTo(scriptServiceSchema);
        }
    }
}