using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[Route("versions")]
public class VersionTrackerServicePageController : Controller
{
    private readonly IVersionTrackerService _versionTrackerService;

    public VersionTrackerServicePageController(IVersionTrackerService versionTrackerService)
    {
        _versionTrackerService = versionTrackerService;
    }

    [HttpGet]
    //add cshtml file
    public async Task<ActionResult> Index()
    {
        var versionTrackerResponse = await _versionTrackerService.GetAllVersionTracker();
        var versionTrackers = versionTrackerResponse.Data;
        return View(versionTrackers);
    }

    // get by version
    [HttpGet("{version}")]
    public async Task<ActionResult> VersionDetails(string version)
    {
        var versionTrackerResponse = await _versionTrackerService.GetVersionTrackerByVersion(version);
        var versionTracker = versionTrackerResponse.Data;
        return View(versionTracker);
    }
}