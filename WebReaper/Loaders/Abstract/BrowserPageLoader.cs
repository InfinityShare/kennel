﻿using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using WebReaper.PageActions;

namespace WebReaper.Loaders.Abstract;

/// <summary>
/// Base class for implementing a browser page loader
/// </summary>
public abstract class BrowserPageLoader
{
    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Interactive browser actions that can be performed on the page
    /// </summary>
    protected readonly Dictionary<PageActionType, Func<IPage, object[], Task>> PageActions = new()
    {
        { PageActionType.ScrollToEnd, async (page, data) => await page.EvaluateExpressionAsync("window.scrollTo(0, document.body.scrollHeight);") },
        { PageActionType.Wait, async (page, data) => await Task.Delay((int)data.First()) },
        { PageActionType.WaitForNetworkIdle, async (page, data) => await page.WaitForNetworkIdleAsync() },
        { PageActionType.Click, async (page, data) => await page.ClickAsync((string)data.First()) }
    };
    
    /// <summary>
    /// Constructor that takes ILogger argument
    /// </summary>
    /// <param name="logger"></param>
    protected BrowserPageLoader(ILogger logger) => Logger = logger;
}