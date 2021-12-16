using Microsoft.AspNetCore.Mvc;

namespace cmd_proxy.Controllers;

[ApiController]
[Route("[controller]")]
public class CmdProxyController : ControllerBase
{
    private readonly ILogger<CmdProxyController> _logger;

    public CmdProxyController(ILogger<CmdProxyController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Status()
    {
        return Ok("ok");
    }

    [HttpPost]
    public IActionResult ExecCommand([FromBody] Command command)
    {

        if(command.Value.Contains("restart"))
        {
            RunCommand("sudo reboot");
        }

        if(command.Value.Contains("backup"))
        {
            var filename = $"pipipi-{DateTime.Today.ToString("yyyyMMdd")}";
            RunCommand($"sudo dd if=/dev/mmcblk0 of=/mnt/hd/media/Backups/raspberry.bkp/{filename}.img && touch {filename}.done");
        }

        return Ok();
    }

    private void RunCommand(string command)
    {
        using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
        {
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \" " + command + " \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
        }
    }
}

public struct Command
{
    public string Value { get; set; }
}
