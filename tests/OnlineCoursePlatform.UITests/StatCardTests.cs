using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using OnlineCoursePlatform.Client.Components;
using Xunit;

namespace OnlineCoursePlatform.UITests;

public class StatCardTests : IDisposable
{
    private readonly BunitContext _ctx = new();

    public void Dispose() => _ctx.Dispose();

    private IRenderedComponent<T> Render<T>(Action<ComponentParameterCollectionBuilder<T>> parameterBuilder) where T : IComponent
        => _ctx.Render(parameterBuilder);

    [Fact]
    public void StatCard_ShouldRenderLabel()
    {
        var cut = Render<StatCard>(parameters => parameters
            .Add(p => p.Label, "Total Courses")
            .Add(p => p.Value, "25")
            .Add(p => p.Icon, "bi-book"));

        cut.Markup.Should().Contain("Total Courses");
    }

    [Fact]
    public void StatCard_ShouldRenderValue()
    {
        var cut = Render<StatCard>(parameters => parameters
            .Add(p => p.Label, "Certificates")
            .Add(p => p.Value, "10")
            .Add(p => p.Icon, "bi-award"));

        cut.Markup.Should().Contain("10");
    }

    [Fact]
    public void StatCard_ShouldRenderIconClass()
    {
        var cut = Render<StatCard>(parameters => parameters
            .Add(p => p.Label, "Active Courses")
            .Add(p => p.Value, "5")
            .Add(p => p.Icon, "bi-play-circle"));

        cut.Markup.Should().Contain("bi-play-circle");
    }

    [Fact]
    public void StatCard_ShouldHaveCardStructure()
    {
        var cut = Render<StatCard>(parameters => parameters
            .Add(p => p.Label, "Test")
            .Add(p => p.Value, "123")
            .Add(p => p.Icon, "bi-graph-up")
            .Add(p => p.IconClass, "primary"));

        cut.Find(".stat-card").Should().NotBeNull();
        cut.Find(".stat-number").TextContent.Should().Be("123");
    }
}
