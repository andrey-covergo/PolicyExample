using FluentAssertions;
using PolicyExample.Scripting.Jint;
using Xunit;

namespace PolicyExample.Tests
{
    public class ScriptServiceSetTests
    {
        [Fact]
        public void Given_service_set_When_injecting_existing_services_When_injector_is_called_with_provided_service()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            set.Add(new ScriptService(){Name="Test service", Version = 1}, service );

            object? resolved=null;
            set.Inject(s=>resolved = s, new ScriptService(){Name="Test service", Version = 1});

            resolved.Should().Be(service);
        }
        
        [Fact]
        public void Given_service_set_When_injecting_requiring_not_existing_services_When_error_occurs()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            set.Add(new ScriptService(){Name="Test service", Version = 1}, service );

            object resolved;
            set.Invoking(s => s.Inject(s => { }, new ScriptService(){Name="Test serviceA", Version = 1}))
                .Should().Throw<MissingServiceException>();
        }
        
        [Fact]
        public void Given_service_set_When_injecting_required_existing_services_with_wrong_version_When_error_occurs()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            set.Add(new ScriptService(){Name="Test service", Version = 1}, service );

            set.Invoking(s => s.Inject(s => { }, new ScriptService(){Name="Test service", Version = 2}))
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
            set.TryGetDescription(scriptService, out var schema).Should().BeTrue();
            
            schema.Should().Be(scriptServiceSchema);
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
            set.TryGetDescription(scriptService, out var schema,Language.JavaScriptEs5).Should().BeTrue();
            
            schema.Should().Be(scriptServiceSchema);
        }
        
        [Fact]
        public void Given_service_set_When_getting_description_for_missing_services_Then_result_is_null()
        {
            var set = new ScriptServiceSet();
            var service = new object();
            var scriptService = new ScriptService(){Name="Test service", Version = 1};
            var scriptServiceSchema = new ScriptServiceSchema()
            {
                AccessName = "test"
            };
            
            set.Add(scriptService, service, scriptServiceSchema );
            
            set.TryGetDescription(new ScriptService(){Name="Test service B", Version = 1}, out var schema).Should().BeFalse();
            
            schema.Should().BeNull();
        }
        
           
        [Fact]
        public void Given_service_set_When_getting_description_for_existing_services_but_missing_language_Then_result_is_null()
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
            
            set.TryGetDescription(new ScriptService(){Name="Test service B", Version = 1}, out var schema, Language.JavaScriptEs6)
                .Should().BeFalse();
            
            schema.Should().BeNull();
        }
    }
}