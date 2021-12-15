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

        string result = string.Empty;

        if(command.Value.Contains("restart"))
             result = RunCommand("sudo reboot");
        
        return Ok(result);
    }

    private string RunCommand(string command)
    {
        string result = "";
        using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
        {
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \" " + command + " \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();

            result += proc.StandardOutput.ReadToEnd();
            result += proc.StandardError.ReadToEnd();

            proc.WaitForExit();
        }
        return result;
    }
}

public struct Command
{
    public string Value { get; set; }
}
